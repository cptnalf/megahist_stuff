
using System.Linq;
using System.Windows.Forms;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace StarTree.Plugin.TFS
{
	using DisplayNames = StarTree.Plugin.Database.DisplayNames;
	using Revision = StarTree.Plugin.Database.Revision;
	using Snapshot = StarTree.Plugin.Database.Snapshot;
	using BranchCont = treelib.AVLTree<string, treelib.StringSorterInsensitive>;
	using ItemDict = treelib.TreapDict<int, Item>;
	
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
			
			//TFSServersSelectorForm form = new TFSServersSelectorForm();
			//DialogResult result = form.ShowDialog();
			//if (result == DialogResult.OK)
				{
					//string serverName = form.getSelection();
					string serverName = "rnotfsat";
					_name = serverName;
					_vcs = megahistory.Utils.GetTFSServer(serverName);
					_cache = new SQLiteStorage.SQLiteCache();
					_cache.load(serverName+".db");
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
			BranchCont brs = new BranchCont();
			ItemDict branchItems = 
				megahistory.SCMUtils.GetBranches(_vcs, 
																			"$/IGT_0803/main/EGS/", VersionSpec.Latest);
			
			/* throw the tfs branches into the branch container. */
			for(ItemDict.iterator it = branchItems.begin();
					it != branchItems.end();
					++it)
				{
					string full_branch = it.value().ServerItem + "/";
					BranchCont.iterator bit = brs.find(full_branch);
					if (bit == brs.end()) { brs.insert(full_branch); }
				}
			
			if (_cache.BranchNames.Any())
				{
					bs = _cache.BranchNames.ToArray();
					
					foreach(string str in bs)
						{
							BranchCont.iterator bit = brs.find(str);
							if (bit == brs.end()) { brs.insert(str); }
							else
								{
									if (bit.item() != str)
										{
											/* well shit. tfs screwed up.
											 * correct tfs.
											 */
											string tmp = bit.item();
											
											bit = null;
											/* this call will invalidate the iterator. */
											brs.remove(tmp);
											brs.insert(str);
										}
								}
						}
				}
			
			if (brs.size() > 0)
				{
					bs = new string[brs.size()];
					int i=0;
					foreach(string str in brs)
						{
							bs[i] = str;
							++i;
						}
				}
			else 
				{ 
					/* this code should never run. */
					bs = TFSDB.DefaultBranches(); 
				}
			
			return bs;
		}

		public override Snapshot getBranch(string branch, long limit)
		{
			Snapshot sn = null;
			
			/* get the last x changesets from tfs. */
			TFSDB db = TFSDB.QueryHistory(_vcs, branch, (ulong)limit, null, _cache);
			
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
			db.queryMerges();
			
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
			TFSDB db = new TFSDB(_vcs, rev.Branch, _cache);
			
			/* add this revision to the visitor */
			db.visitor.addRevision(rev);
			
			/* generate a bunch of queries given this revision's parents.
			 * then run the queries.
			 */
			db.queueParents(rev);
			
			/* now dump the result back to the cache. */
			db.visitor.save(_cache);
			
			/* now run the query against the cache. */
			sn = _cache.queryMerges(rev);
			
			return sn;
		}
	}
}
