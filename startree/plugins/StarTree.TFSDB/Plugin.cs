
using System.Linq;
using System.Windows.Forms;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace StarTree.Plugin.TFSDB
{
	using DisplayNames = StarTree.Plugin.Database.DisplayNames;
	using Revision = StarTree.Plugin.Database.Revision;
	using Snapshot = StarTree.Plugin.Database.Snapshot;
	using BranchCont = treelib.AVLTree<string, treelib.StringSorterInsensitive>;
	
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
				{
					bs = _cache.BranchNames.ToArray();
					BranchCont brs = TFSDB.GetBranches(_vcs);
					
					foreach(string str in bs)
						{
							BranchCont.iterator it = brs.find(str);
							if (it == brs.end()) { brs.insert(str); }
						}
					
					bs = new string[brs.size()];
					int i=0;
					foreach(string str in brs)
						{
							bs[i] = str;
							++i;
						}
				}
			else { bs = TFSDB.DefaultBranches(); }
			
			return bs;
		}

		public override Snapshot getBranch(string branch, long limit)
		{
			Snapshot sn = null;
			
			/* get the last x changesets from tfs. */
			TFSDB db = TFSDB.QueryHistory(_vcs, branch, (ulong)limit, null);
			
			{
				string[] bs = branches();
				db.visitor.primeBranches(bs);
			}
			
			/* copy stuff we've already seen from the db cache into the visitor cache.
			 * this only does the top-level stuff
			 * any further queries we get will be passed to the cache.
			 */
			foreach(Changeset cs in db.history)
				{
					Revision rev = _cache.rev(TFSDB.MakeID(cs.ChangesetId));
					if (rev != null) { db.visitor.addRevision(rev); }
				}
			
			/* prime the merge history querying.
			 * this will take out all top-level queries which have already been done.
			 * already been done = exists in cache.
			 * 
			 * this will also query tfs for the merge info.
			 */
			db.queryMerges(_cache);
			
			/* at this point, ALL the changesets in 'history' MUST be in the visitor.
			 * if not, then there's a problem with:
			 *  insertQueries or the QueryProcessor.
			 */
			
			/* this adds in the branch's history to the parent lists of the
			 * appropriate changesets.
			 * this will also update the cache
			 */
			db.fixHistory();
			
			/* now dump everything in the visitor to the cache. */
			db.visitor.save(_cache);
			
			db = null;
			
			/* the full list should be in the cache now. */
			sn = _cache.getBranch(branch, (ulong)limit);

			return sn;
		}

		public override Revision getRevision(string id) { return _cache.rev(id); }
		
		public override Snapshot queryMerges(Revision rev)
		{
			Snapshot sn = null;
			TFSDB db = new TFSDB(_vcs, rev.Branch);
			
			/* add this revision to the visitor */
			db.visitor.addRevision(rev);
			
			QueryProcessor qp = new QueryProcessor(db.visitor, _vcs);
			
			/* generate a bunch of queries given this revision's parents. */
			db.populateQueries(rev, qp, _cache);
			
			/* run the TFS queries. */
			qp.runThreads();
			
			/* now dump the result back to the cache. */
			db.visitor.save(_cache);
			
			/* now run the query against the cache. */
			sn = _cache.queryMerges(rev);
			
			return sn;
		}
	}
}
