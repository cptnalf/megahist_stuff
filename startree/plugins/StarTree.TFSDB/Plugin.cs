
using System.Linq;
using System.Windows.Forms;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace StarTree.Plugin.TFSDB
{
	using DisplayNames = StarTree.Plugin.Database.DisplayNames;
	using Revision = StarTree.Plugin.Database.Revision;
	using Snapshot = StarTree.Plugin.Database.Snapshot;
	
	[System.AddIn.AddIn("TFSDB", Description="plugin for TFS")]
	public class PluginInterface : StarTree.Plugin.Database.Plugin
	{
		private DisplayNames _dn;
		private string _name;
		private SQLiteStorage.SQLiteCache _cache;
		private VersionControlServer _vcs;
		
		public PluginInterface()
		{
			_dn = new DisplayNames
				{
					name = "tfs",
					id = "Changeset",
					author = "Committer",
					date = "CreationDate",
					log = "Comment",
					parent = "Parent",
				};
		}
				
		public override void open()
		{
			if (_cache != null)
				{
					_cache.close();
					_cache = null;
					_vcs = null;
					_name = null;
				}
			
			TFSServersSelectorForm form = new TFSServersSelectorForm();
			DialogResult result = form.ShowDialog();
			if (result == DialogResult.OK)
				{
					string serverName = form.getSelection();
					
					_name = serverName;
					_vcs = megahistory.Utils.GetTFSServer(serverName);
					_cache = new SQLiteStorage.SQLiteCache();
					_cache.load(serverName);
				}
		}

		public override void close()
		{
			_vcs = null;
			_cache.close();
			_cache = null;
		}

		public override string currentName { get { return _name; } }
		public override DisplayNames names { get { return _dn; } }
		
		public override string[] branches()
		{
			string[] bs = null;
			if (_cache.BranchNames.Any())
				{ bs = _cache.BranchNames.ToArray(); }
			else { bs = TFSDB.DefaultBranches(); }
			
			return bs;
		}

		public override Snapshot getBranch(string branch, long limit)
		{
			Snapshot sn = null;
			TFSDBVisitor visitor = new TFSDBVisitor();
			TFSDB db = new TFSDB(_vcs, branch, visitor);
			
			visitor.primeBranches(branches());

			/* it doesn't matter how many items i have in the cache, 
			 * i want the last X in connected descending order.
			 */
			treelib.AVLTree<Changeset, ChangesetDescSorter> history = db.queryHistory((ulong)limit, null);
			
			/* populate from the cache. */
			foreach(Changeset cs in history)
				{
					Revision rev = _cache.rev(TFSDB.MakeID(cs.ChangesetId));
					if (rev != null) { visitor.addRevision(rev); }
				}
			
			/* do the merge querying. 
			 * the cache is passed in to alievate the need for already
			 * executed merge queries.
			 */
			db.queryMerges(history, _cache);
			
			/* this adds in the branch's history to the parent lists of the
			 * appropriate changesets.
			 * this will also update the cache
			 */
			db.fixHistory(history);
			
			/* now we need to now save the stuff we just queried */
			visitor.save(_cache);

			//_onProgress(10, "finished persisting the results");
			/* the full list should be in the cache now. */
			sn = _cache.getBranch(branch, (ulong)limit);

			return sn;
		}

		public override Revision getRevision(string id) { return _cache.rev(id); }
	}
}
