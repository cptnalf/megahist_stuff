
using System.Collections.Generic;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace TFSTree.Databases.TFSDB
{
	using StringEnumerable = System.Collections.Generic.IEnumerable<string>;
	using RevisionCont = treelib.AVLTree<Revision, RevisionSorterDesc>;
	using BranchContainer = treelib.AVLTree<string, treelib.StringSorterInsensitive>;
	using RevisionIdx = treelib.AVLDict<string, Revision>;
	using BranchChangesets =
		treelib.AVLDict<string, treelib.AVLDict<string, Revision>, treelib.StringSorterInsensitive>;
		
	/// <summary>
	/// 
	/// </summary>
	public partial class TFSDB : RevisionRepoBase, IRevisionRepo
	{
		internal static int THREAD_COUNT = 8;
		internal static log4net.ILog logger = log4net.LogManager.GetLogger("tfsdb_logger");
		
		internal static string MakeID(int id) { return string.Format("{0:d6}", id); }
		
		/* 1 = just populate the requested branch's revisions
		 * 2 = populate the requested branch + the parent's of the requested branch.
		 */
		private static readonly int RECURSIVE_QUERY_COUNT = 2;
		
		private string _tfsServerName;
		private VersionControlServer _vcs;
		private SQLiteCache.SQLiteCache _cache;
		
		public event System.EventHandler<ProgressArgs> OnProgress;
		
		public TFSDB()
		{
			if (! logger.Logger.Repository.Configured)
				{
					System.Reflection.Assembly asm = 
						System.Reflection.Assembly.GetExecutingAssembly();
					System.IO.FileStream fs;
					
					try{
						fs = new System.IO.FileStream(asm.Location+".config", 
																					System.IO.FileMode.Open, 
																					System.IO.FileAccess.Read, 
																					System.IO.FileShare.ReadWrite);
						log4net.Config.XmlConfigurator.Configure(fs);
					}
#if DEBUG
					catch(System.Exception e)
						{
							int q = 1;
							q -= 12;
							if (e.HelpLink == null ) { q +=12; }
						}
#else
					catch(System.Exception) { }
#endif
				}
		}
		
		public void load(string filename)
		{
			_tfsServerName = filename;
			_vcs = megahistory.Utils.GetTFSServer(filename);
			_cache = new SQLiteCache.SQLiteCache();
			_cache.load(filename);
		}
		
		public void close()
		{
			_cache.close();
		}
		
		public string Name { get { return _tfsServerName; } }
		
		public override System.Collections.Generic.IEnumerable<string> BranchNames
		{
			get
				{
					if (_branches == null) { _queryBranches(); }
					return _branches;
				}
		}
		
		/// <summary>
		/// grab a revision.
		/// </summary>
		/// <remarks>
		/// this will consult the local cache db to see if it exists in there
		/// so:
		/// cached-in-memory index
		/// database
		/// </remarks>
		/// <param name="id"></param>
		/// <returns>null if it's not been loaded.</returns>
		public override Revision rev(string id)
		{
			Revision rev = null;
			rev = base.rev(id);
			if (rev == null) { rev = _cache.rev(id); }
			return rev;
		}
		
		public override RevisionCont getBranch(string branch, ulong limit)
		{
			RevisionCont revisions = null;
			BranchChangesets.iterator it = _branchChangesets.find(branch);
			
			if (it != _branchChangesets.end() )
				{
					/* we have the branch in the cache, check what we've got. */
					if ( it.value().size() < limit)
						{
							/* try db, this could fail... */
							revisions = _cache.getBranch(branch, limit);
							
							if (revisions != null && revisions.size() < limit)
								{
									/* so, we don't actually have everything, 
									 * let's try to get the rest...
									 */
									RevisionCont.reverse_iterator rit = revisions.rbegin();
									ulong delta = limit - revisions.size();
									
									/* this will grab the stuffs from tfs 
									 * and dump the deltas out to the database.
									 */
									_runQuery(branch, delta, rit.item().ID);
									
									/* the full list should be in the memory cache now. */
									revisions = base.getBranch(branch, limit);
								}
						}
					else { revisions = base.getBranch(branch, limit); }
				}
			
			if (revisions == null)
				{
					/* full retrievial. */
					_runQuery(branch, limit, null);
					
					revisions = base.getBranch(branch, limit);
				}
			
			return revisions;
		}
		
		private void _queryBranches()
		{
			_branches = new BranchContainer();
			
			/* replace this with a dynamic tfs query.
			 * maybe cache that too.
			 */
			
			_branches.insert("$/IGT_0803/development/dev_adv/EGS/");
			_branches.insert("$/IGT_0803/development/dev_adv_cr/EGS/");
			_branches.insert("$/IGT_0803/development/dev_build/EGS/");
			_branches.insert("$/IGT_0803/development/dev_ABS/EGS/");
			_branches.insert("$/IGT_0803/development/dev_sb/EGS/");
			_branches.insert("$/IGT_0803/main/EGS/");
			_branches.insert("$/IGT_0803/release/EGS8.2/dev_sp/EGS/");
		}
	}		
}

