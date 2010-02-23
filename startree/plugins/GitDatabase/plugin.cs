
using System.Windows.Forms;
using System.Linq;

namespace Git.Database
{
	[System.AddIn.AddIn("GitPlugin", Description="Git StarTree plugin for git source control", Version="1.0.0.0")]
	public class PluginInterface : StarTree.Plugin.Database.Plugin
	{
		private FolderBrowserDialog fbd = new FolderBrowserDialog();
		private StarTree.Plugin.Database.DisplayNames _names;
		private Database _db;
		
		public PluginInterface()
		{
			fbd.Description = "Folder containing the git repository, or container for the repository";
			fbd.ShowNewFolderButton = false;
			
			_names = new StarTree.Plugin.Database.DisplayNames
				{
					name = "git",
					id = "commit",
					author = "Author",
					parent = "parent",
					log = null,
					date = "date",
				};
		}
		
		public override StarTree.Plugin.Database.DisplayNames names { get { return _names; } }
		public override string currentName
		{
			get
				{
					string name = null;
					if (_db != null) { name = _db.Name; }
					return name;
				}
		}
		
		public override void open()
		{
			_db = null;
			
			DialogResult result = fbd.ShowDialog();
			if (result == DialogResult.OK)
				{
					_db = new Database(@"c:\programs\git\bin\git.exe", fbd.SelectedPath);
				}
		}
		
		public override void close()
		{
			_db = null;
		}

		public override string[] branches()
		{
			string[] branches = null;
			if (_db != null) { branches = _db.BranchNames.ToArray(); }
			
			return branches;
		}

		public override StarTree.Plugin.Database.Snapshot getBranch(string branch, long limit)
		{
			StarTree.Plugin.Database.Snapshot sn = _db.getBranch(branch, (ulong)limit);
			return sn;
		}
		public override StarTree.Plugin.Database.Revision getRevision(string id)
		{ return _db.rev(id); }
	}
}
