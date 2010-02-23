
using System.Collections.Generic;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace StarTree.Plugin.TFSDB
{
	using Revision = StarTree.Plugin.Database.Revision;
	using Snapshot = StarTree.Plugin.Database.Snapshot;
	using StringEnumerable = System.Collections.Generic.IEnumerable<string>;
	using RevisionCont = treelib.AVLTree<StarTree.Plugin.Database.Revision, StarTree.Plugin.Database.RevisionSorterDesc>;
	using BranchContainer = treelib.AVLTree<string, treelib.StringSorterInsensitive>;
	using RevisionIdx = treelib.AVLDict<string, StarTree.Plugin.Database.Revision>;
	using BranchChangesets =
		treelib.AVLDict<string, treelib.AVLDict<string, StarTree.Plugin.Database.Revision>, treelib.StringSorterInsensitive>;
		
	/// <summary>
	/// 
	/// </summary>
	public partial class TFSDB : StarTree.Plugin.SQLiteCache.SQLiteCache
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
		private BranchContainer _branches = new BranchContainer();
		
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
		
		public override void load(string filename)
		{
			_tfsServerName = filename;
			_vcs = megahistory.Utils.GetTFSServer(filename);
			base.load(filename);
		}
		
		/// <summary>
		/// close
		/// </summary>
		public override void close()
		{
			base.close();
		}
		
		public override string Name { get { return _tfsServerName; } }
		
		/// <summary>
		/// prepopulate
		/// </summary>
		public override System.Collections.Generic.IEnumerable<string> BranchNames
		{
			get
				{
					if (_branches == null || _branches.empty()) { _queryBranches(); }
					return _branches;
				}
		}
		
		public override Snapshot getBranch(string branch, ulong limit)
		{
			Snapshot sn = null;
			
			/* it doesn't matter how many items i have in the cache, 
			 * i want the last X in connected descending order.
			 */
			_runQuery(branch, limit, null);
			
			/* the full list should be in the cache now. */
			sn = base.getBranch(branch, limit);
			
			return sn;
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
			
			foreach(string branch in base.BranchNames) { _branches.insert(branch); }
		}
	}		
}

