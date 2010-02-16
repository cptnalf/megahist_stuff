
using System.Text.RegularExpressions;
﻿
namespace Git.Database
{
	using StringList = System.Collections.Generic.List<string>;
	using RevisionIdx = treelib.AVLTree<TFSTree.Databases.Revision, TFSTree.Databases.RevisionSorterDesc>;
	using Revision = TFSTree.Databases.Revision;
	
	internal class Database : TFSTree.Databases.IRevisionRepo
	{
	#region IRevisionRepo Members
		private string _path;
		private StringList _branches;
		private string _formatArg = "--format=\"format:%H%n%P%n%an|%ae|%ai%n%s%n%b%n########################%n\"";
		private RunCmd _runCmd;
		
		internal Database(string git, string dir)
		{
			_path = dir;
			_runCmd = new RunCmd(git, dir);
		}
		
		public event System.EventHandler<TFSTree.Databases.ProgressArgs> OnProgress;
		
		public string Name { get { return _path; } }

		public System.Collections.Generic.IEnumerable<string>  BranchNames
		{
			get
				{
					if (_branches == null) { _branches = _getBranches(); }
					return _branches;
				}
		}

		public TFSTree.Databases.Revision rev(string id)
		{
 			_runCmd.run(string.Format("log -1 {0} {1}", _formatArg, id));
			
			Revision rev = null;
			
			rev = _parseRevision(_runCmd.rdr());
			
			_runCmd.waitForExit();
			return rev;
		}
		
		public RevisionIdx getBranch(string branch, ulong limit)
		{
			RevisionIdx revisions = new RevisionIdx();
			
			_runCmd.run(string.Format("log {0} -{1} {2}", _formatArg, limit, branch));
			System.IO.StreamReader rdr = _runCmd.rdr();
			
			Revision rev = null;
			
			do
				{
					rev = _parseRevision(rdr);
					if (rev != null) { revisions.insert(rev); }
				}
			while (rev != null);
			
			_runCmd.waitForExit();
			
			return revisions;
		}
		
		private Revision _parseRevision(System.IO.StreamReader rdr)
		{
			Revision rev = null;
			string line = rdr.ReadLine();
			
			if (line != null)
				{
					rev = new Revision();
					rev.Branch = "master"; /* @todo FIX ME! */
					
					while (line == string.Empty) { line = rdr.ReadLine(); }
					rev.ID = line;
					
					line = rdr.ReadLine();
					if (line != string.Empty)
						{
							string[] parts = line.Split(' ');
							for(int i=0; i < parts.Length; ++i) { rev.addParent(parts[i]); }
						}
					
					line = rdr.ReadLine();
					{
						string[] parts = line.Split('|');
						rev.Author = string.Format("{0} <{1}>", parts[0], parts[1]);
						rev.Date = System.DateTime.Parse(parts[2]);
					}
					
					System.Text.StringBuilder bldr = new System.Text.StringBuilder();
					line = rdr.ReadLine();
					while(line != null && line != "########################")
						{
							bldr.AppendLine(line);
							line = rdr.ReadLine();
						}
					rev.Log = bldr.ToString();
				}
			
			return rev;
		}
		
		private StringList _getBranches()
		{
			StringList branches = new StringList();
			Regex linere = new Regex("[ *]+([^ ]+)");
			
			_runCmd.run("branch");
			
			System.IO.StreamReader rdr = _runCmd.rdr();
			
			/* this generates
			 * ' master'
			 * or
			 * '* master'
			 */
			string line = rdr.ReadLine();
			while (line != null)
				{
					Match m = linere.Match(line);
					
					if (m != null)
						{
							branches.Add(m.Groups[1].Value);
						}
					line = rdr.ReadLine();
				}
			
			_runCmd.waitForExit();
			
			return branches;
		}

	#endregion
	}
}
