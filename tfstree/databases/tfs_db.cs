
/*
 */
using System.Collections.Generic;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace TFSTree.Databases
{
	using IEnumerable = System.Collections.IEnumerable;
	using RevisionCont = treelib.AVLTree<Revision>;
	using BranchContainer = treelib.AVLTree<string, treelib.StringSorterInsensitive>;
	using ChangesetIdx = treelib.AVLDict<int,Revision,treelib.IntSorterDesc>;
	using BranchChangesets = 
		treelib.AVLDict<string,treelib.AVLDict<int,Revision,treelib.IntSorterDesc>,treelib.StringSorterInsensitive>;

	internal class ChangesetDescSorter : IComparer<Changeset>
	{
		public ChangesetDescSorter() { }
		
		public int Compare(Changeset c1, Changeset c2)
		{
			int res = 0;
			
			if (c1 == null || c2 == null)
				{
					/* figure out which one... */
					if (c1 == null) { res = -1; }
					else { res = 1; }
				}
			else { res = c2.ChangesetId.CompareTo(c1.ChangesetId); }
			
			return res;
		}
	}

	internal class TFSDB : IRevisionRepo, megahistory.IVisitor<Revision>
	{
		internal static log4net.ILog logger = log4net.LogManager.GetLogger("tfsdb_logger");
		
		/* 1 = just populate the requested branch's revisions
		 * 2 = populate the requested branch + the parent's of the requested branch.
		 */
		private static readonly int RECURSIVE_QUERY_COUNT = 2;
		
		private ChangesetIdx _changesetIdx = new ChangesetIdx();
		private BranchContainer _branches;
		private BranchChangesets _branchChangesets = new BranchChangesets();
		private string _tfsServerName;
		private VersionControlServer _vcs;
		
		internal TFSDB()
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
		
		public string FileName { get { return _tfsServerName; } }
		
		public System.Collections.Generic.IEnumerable<string> BranchNames
		{
			get
				{
					if (_branches == null) { _queryBranches(); }
					return _branches;
				}
		}
		
		public Revision rev(string id)
		{
			Revision rev = null;
			int csid = System.Int32.Parse(id);
			ChangesetIdx.iterator it = _changesetIdx.find(csid);
			if (it != _changesetIdx.end()) { rev = it.value(); }
			return rev;
		}
		
		public System.Collections.Generic.Dictionary<string,Revision> revs(string branch, ulong limit)
		{
			System.Collections.Generic.Dictionary<string,Revision> revisions = 
				new System.Collections.Generic.Dictionary<string,Revision>();
			
			BranchChangesets.iterator it = _branchChangesets.find(branch);
			if (it == _branchChangesets.end())
				{
					/* try to populate it... */
					_getRevisions(branch, limit);
					
					/* try the search again... */
					it = _branchChangesets.find(branch);
				}
			
			
			if (it != _branchChangesets.end())
				{
					if (it.value().size() < limit) { _requery(branch, limit); }
					
					ulong count = (it.value().size() > limit) ? limit : it.value().size();
					ChangesetIdx.iterator csit = it.value().begin();
					/* i really need to dump out an ordered container, 
					 * but since i'm lazy, let's order these inserts with an ordered container first.
					 */
					RevisionCont tmp = new RevisionCont();
					
					for(ulong i=0; i < count; ++i, ++csit)
						{
							if (csit == it.value().end()) { break; }
							tmp.insert(csit.value());
						}
					
					for(RevisionCont.iterator rit = tmp.begin();
							rit != tmp.end();
							++rit)
						{
							revisions.Add(rit.item().ID, rit.item());
						}
				}
			
			return revisions;
		}
		
		public void load(string filename)
		{
			_tfsServerName = filename;
			_vcs = megahistory.Utils.GetTFSServer(filename);
		}
		
		public void loadfolder(string filename) { }
		
		private void _queryBranches()
		{
			_branches = new BranchContainer();
			
			_branches.insert("$/IGT_0803/development/dev_adv/EGS/");
			_branches.insert("$/IGT_0803/development/dev_build/EGS/");
			_branches.insert("$/IGT_0803/development/dev_ABS/EGS/");
			_branches.insert("$/IGT_0803/development/dev_sb/EGS/");
			_branches.insert("$/IGT_0803/main/EGS/");
			_branches.insert("$/IGT_0803/release/EGS8.2/dev_sp/EGS/");
		}
		
		private void _requery(string branch, ulong limit)
		{
			/* this would grab more revisions, starting where we left off. */
		}
		
		public Revision visit(string branch, Changeset cs)
		{
			Revision rev = new Revision(cs.ChangesetId, branch,
																	cs.Owner, cs.CreationDate,
																	cs.Comment);
			
			logger.DebugFormat("{0}=>{1}", branch, cs.ChangesetId);
						
			ChangesetIdx.iterator csit = _changesetIdx.find(cs.ChangesetId);
			if (csit == _changesetIdx.end())
				{
					_changesetIdx.insert(cs.ChangesetId, rev);

					BranchChangesets.iterator it = _branchChangesets.find(branch);
					if (it != _branchChangesets.end())
						{ 
							logger.DebugFormat("inserted {0}", rev.ID);
							it.value().insert(cs.ChangesetId, rev); 
						}
					else
						{
							logger.DebugFormat("adding {0} and inserting {1}", rev.Branch, rev.ID);
							ChangesetIdx revisionsIdx = new ChangesetIdx();
							revisionsIdx.insert(cs.ChangesetId, rev);
							_branchChangesets.insert(branch, revisionsIdx);
						}
				} 
			return rev;
		}
		
		public void addParent(Revision rev, int parentID)
		{
			rev.addParent(parentID.ToString());
		}

		public bool visited(string branch, int csID)
		{
			bool visited = false;
			
			visited = _changesetIdx.find(csID) != _changesetIdx.end();
			return visited;
		}
		
		private void _getRevisions(string branch, ulong limit)
		{
			megahistory.MergeHist<Revision> mergeHist = new megahistory.MergeHist<Revision>(_vcs, this);
			treelib.AVLTree<Changeset,ChangesetDescSorter> history = 
				new treelib.AVLTree<Changeset,ChangesetDescSorter>();
			
			IEnumerable foo =
				_vcs.QueryHistory(branch, VersionSpec.Latest, 0, RecursionType.Full,
													null, null, null, /* user, from ver, to ver */
													(int)limit, 
													true, false, false); 
			/* inc changes, slot mode, inc download info. */
			
			logger.DebugFormat("q[{0},{1}]", branch, limit);
			
			foreach (object o in foo)
				{
					Changeset cs = o as Changeset;
					history.insert(cs);
				}
			
			string prevID = null;
			for(treelib.AVLTree<Changeset,ChangesetDescSorter>.iterator it = history.begin();
					it != history.end();
					++it)
				{
					/* i need to find a path to use as a base item for this merge history query. 
					 *
					 * this could skew the results a little bit.
					 * unfortunately, to support renames, i need to get the previous name, 
					 * which might be in the changeset history (?)
					 * 
					 * however, this won't detect situations where the user checked-in
					 * changes on two separate branches.
					 */
					Changeset cs = it.item();
					List<string> branchParts = megahistory.Utils.FindChangesetBranches(cs);
					
					for(int bpi = 0; bpi < branchParts.Count; ++bpi)
						{
							Item itm = _vcs.GetItem(branchParts[bpi], new ChangesetVersionSpec(cs.ChangesetId));
							
							logger.DebugFormat("qm[{0},{1}]", itm.ServerItem, cs.ChangesetId);
							mergeHist.queryMerge(cs, itm, RECURSIVE_QUERY_COUNT);
							
							if (prevID != null)
								{
									BranchChangesets.iterator mhit = _branchChangesets.find(branchParts[bpi]);
									if (mhit != _branchChangesets.end())
										{
											ChangesetIdx.iterator mhcsit = mhit.value().find(cs.ChangesetId);
											if (mhcsit != mhit.value().end())
												{
													mhcsit.value().addParent(prevID);
												}
										}
								}
							
							prevID = cs.ChangesetId.ToString();
						}
				}
		}
	}
}