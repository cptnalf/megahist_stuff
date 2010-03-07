
using Microsoft.TeamFoundation.VersionControl.Client;
using System.Collections.Generic;

namespace megahistorylib
{
	using ChangesetCont = treelib.AVLTree<Changeset, ChangesetSorter>;
	using SortedPaths_T = treelib.AVLTree<string, treelib.StringSorterInsensitive>;
	using ChangesetDict_T =
		treelib.AVLDict<int, treelib.AVLTree<string, treelib.StringSorterInsensitive>>;
	
	/// <summary>
	/// 
	/// </summary>
	public partial class MegaHistory
	{
		/// <summary>
		/// the number of threads to create when querying
		/// </summary>
		public static int THREAD_COUNT = 8;
		
		/// <summary>
		/// get a changeset.
		/// </summary>
		/// <param name="csID"></param>
		/// <returns></returns>
		public Changeset getCS(int csID)
		{
			saastdlib.Timer t = new saastdlib.Timer();
			t.start();
			Changeset cs = _vcs.GetChangeset(csID);
			t.stop();
			
			lock(_gct)
				{
					++_gcc;
					_gct.TotalT += t.DeltaT;
				}
			
			return cs;
		}
		
		/// <summary>
		/// 
		/// </summary>
		public IMergeResults Results
		{ get { return _results; } set { _results = value; } }
		
		/// <summary>
		/// run the merge query threaded.
		/// </summary>
		public bool Threaded { get { return _threaded; } }
		/// <summary>
		/// base query depth
		/// do this many queries, no more.
		/// when distance = 0, no more queries are done, 
		/// nor is the target changeset visited
		/// 
		/// so:
		/// BaseDistance=3
		/// #1 csid=12, branch=main,       distance=3
		/// #2 csid=10, branch=dev_adv,    distance=2
		/// #3 csid=8,  branch=dev_adv_cr, distance=1
		/// 
		/// </summary>
		public int BaseDistance { get { return _baseDistance; } }
		
		/// <summary>
		/// the number of calls to QueryMergeDetails
		/// </summary>
		public ulong QueryCount { get { return _qc; } }
		/// <summary>
		/// number of GetItem calls.
		/// </summary>
		public ulong GetItemCount { get { return _gic; } }
		/// <summary>
		/// number of getCS calls.
		/// </summary>
		public ulong GetChangesetCount { get { return _gcc; } }
		
		/// <summary>
		/// time taken by QueryMergeDetails.
		/// </summary>
		public System.TimeSpan QueryTime { get { return _qt.Total; } }
		/// <summary>
		/// time taken by GetItem call.
		/// </summary>
		public System.TimeSpan GetItemTime { get { return _git.Total; } }
		/// <summary>
		/// time taken by getCS
		/// </summary>
		public System.TimeSpan GetChangesetTime { get { return _gct.Total; } }
		
		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="tfsServerName"></param>
		/// <param name="distance"></param>
		public MegaHistory(string tfsServerName, int distance)
		{
			_vcs = tfsinterface.SCMUtils.GetTFSServer(tfsServerName);
			_baseDistance = distance;
			
			Logger.LoadLogger();
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="vcs"></param>
		/// <param name="distance"></param>
		public MegaHistory(VersionControlServer vcs, int distance)
		{
			_vcs = vcs;
			_baseDistance = distance;
			
			Logger.LoadLogger();
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="pathVer"></param>
		/// <param name="limit"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="user"></param>
		public void query(string path, VersionSpec pathVer, 
											int limit, VersionSpec from, VersionSpec to, 
											string user)
		{
			Item item = _getItem(path, pathVer, 0, false);
			query(item, limit, from, to, user);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="limit"></param>
		public void query(Item path, int limit) 
		{ query(path, limit, null, null, null); }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="limit"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="user"></param>
		public void query(Item path, int limit, 
											VersionSpec from, VersionSpec to, string user)
		{
			ChangesetVersionSpec ver = new ChangesetVersionSpec(path.ChangesetId);
			ChangesetCont history = new ChangesetCont();
			
			System.Collections.IEnumerable stuff = 
				_vcs.QueryHistory(path.ServerItem, ver, path.DeletionId,
													RecursionType.Full, user, from, to, limit, 
													true, true, false);
			
			foreach(object o in stuff)
				{
					Changeset cs = o as Changeset;
					history.insert(cs);
				}

			{
				/* this is used by the 'FindChangesetBranches' to figure out which
				 * changes to pay attention to.
				 */
				tfsinterface.Utils.ChangeTypeToConsiderDelegate IsCngToConsider =
					(cng) =>
						{
							/* only merge = screw you change
							 * if branch or merge & other stuff = ok
							 */
							return
								(
								 (cng.ChangeType != ChangeType.Merge)
								 && (
										 ((cng.ChangeType & ChangeType.Merge) == ChangeType.Merge)
										 || ((cng.ChangeType & ChangeType.Branch) == ChangeType.Branch)
										 )
								 );
						};
			}
			
			/* prime the result-set. */
			for (ChangesetCont.iterator it = history.begin();
					 it != history.end();
					 ++it)
				{ Revision r = _results.getRevision(it.item().ChangesetId); }
			
			QueryProcessor qp = new QueryProcessor(this, THREAD_COUNT);
			string branch = tfsinterface.Utils.GetEGSBranch(path.ServerItem);
			
			for(ChangesetCont.iterator it = history.begin();
			    it != history.end();
			    ++it)
				{
					Revision rev = _results.getRevision(it.item().ChangesetId);
					
					if (rev != null)
						{ _queueOrVisit(qp, it.item(), this._baseDistance -1, branch); }
					else { _queueParents(qp, rev); }
				}
			
			qp.runThreads();
			
			{
				Revision prev = null;
				
				for(ChangesetCont.iterator it = history.begin();
						it != history.end();
						++it)
					{
						int id = it.item().ChangesetId;
						
						if (prev != null)
							{
								/* fix up the parent.
								 * this will only add the parent if they're not already in the list.
								 */
								
								prev.addParent(id);
							}
						
						prev = _results.getRevision(id);
						_results.addPrimaryID(id);
					}
			}
		}
		
		/// <summary>
		/// this function acts as a proxy for the function below it.
		/// </summary>
		public IList<QueryRec> queryMerge(Changeset cs, string targetPath, int distance)
		{
			saastdlib.Timer t = new saastdlib.Timer();
			t.start();
			
			/* pull down the item type. 
			 * i need this to determine params for the querymergedetails call.
			 */
			Item itm = _getItem(targetPath, cs.ChangesetId, 0, false);
			
			IList<QueryRec> queries = queryMerge(cs, itm, distance);
			
			t.stop();
			Logger.logger.DebugFormat("qm[{0} queries took {1}]", this.QueryCount, this.QueryTime);
			Logger.logger.DebugFormat("qm[{0} get items took {1}]", this.GetItemCount, this.GetItemTime);
			Logger.logger.DebugFormat("qm[{0} get changesets took {1}]", this.GetChangesetCount, this.GetChangesetTime);
			Logger.logger.DebugFormat("qm[total time {1}]", t.Total);
			
			return queries;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cs"></param>
		/// <param name="targetItem"></param>
		/// <param name="distance"></param>
		/// <returns></returns>
		public IList<QueryRec> queryMerge(Changeset cs, Item targetItem, int distance)
		{
			string srcPath = null;
			VersionSpec srcVer = null;
			int srcDelID = 0;
			VersionSpec targetVer = new ChangesetVersionSpec(cs.ChangesetId);
			VersionSpec fromVer = targetVer;
			VersionSpec toVer = targetVer;
			RecursionType recurType = RecursionType.None; /* assume these are all files */
			ChangesetMergeDetails mergedetails;
			Revision revision;
			
			ChangesetDict_T visitedItems;
			/* map a changeset id to a list of items in that changeset which were merged.
			 * (that we happen to care about.
			 */
			
			/* don't do queries when we've reached the distance limit. */
			if (distance == 0) { return new List<QueryRec>(); }
			
			if (targetItem.ItemType != ItemType.File) { recurType = RecursionType.Full; }
			
			saastdlib.Timer t = new saastdlib.Timer();
			t.start();
			mergedetails = 
				_vcs.QueryMergesWithDetails(srcPath, srcVer, srcDelID,
																	 targetItem.ServerItem, targetVer, 
																	 targetItem.DeletionId, 
																	 fromVer, toVer, recurType);
			t.stop();
			
			lock(_qt)
				{
					++_qc;
					_qt.TotalT += t.DeltaT;
				}
			t = null;
			
			visitedItems = _processDetails(cs.ChangesetId, mergedetails);
			
			{
				string itemPath = targetItem.ServerItem;
				if (targetItem.ItemType != ItemType.File) { itemPath += '/'; }
				
				Logger.logger.DebugFormat("v[{0}{1}]", itemPath, cs.ChangesetId);
				revision = _results.construct(itemPath, cs);
			}
			
			/* now walk the list of compiled changesetid + itempath and 
			 * construct a versioned item for each that we can actually go and query.
			 */
			List<QueryRec> items = new List<QueryRec>();
			for(ChangesetDict_T.iterator it= visitedItems.begin();
					it != visitedItems.end();
					++it)
				{
					Item itm = null;
					SortedPaths_T.iterator pathsIt = it.value().begin();
					
					//Console.WriteLine("parent: {0}", pair.Key);
					revision.addParent(it.item());
					
					if ((distance -1) > 0)
						{
							/* only query the item info if we're going to do a query on it. */
							
							if (it.value().size() > 1)
								{
									/* so, find out what this branch is and use the changeset id. */
									string thisBranch = tfsinterface.Utils.GetEGSBranch(pathsIt.item());
									string pathPart = tfsinterface.Utils.GetPathPart(targetItem.ServerItem);
									
									//Console.WriteLine("---- {0} => {1} + {2}", pair.Value.Values[0], thisBranch, pathPart);
									if (! string.IsNullOrEmpty(thisBranch))
										{
											Logger.logger.DebugFormat("iqm[{0},{1}]", 
																								thisBranch+"/EGS/"+pathPart, 
																								it.item());
											
											itm = _getItem(thisBranch + "/EGS/" + pathPart, 
																		 it.item(), 0, false);
										}
									else { System.Console.WriteLine("---[e] {0}", pathsIt.item()); }
								}
							else
								{
									/* so, this is the only file we noticed for this changeset,
									 * spawn a new query for just this file (folder?)
									 */
									
									Logger.logger.DebugFormat("iq1[{0},{1}]", pathsIt.item(), it.item());
							
									try {
										itm = _getItem(pathsIt.item(), it.item(), 0, false);
									} catch(System.Exception ex)
										{
											Logger.logger.Fatal("fatal item query:", ex);
									
											try {
												/* try just the path, i doubt this will work either, but *shrug* */
												itm = _getItem(pathsIt.item());
											} catch(System.Exception) { itm = null; }
										}
								}
					
							/* queue it. */
							if (itm != null) 
								{
									if (! _results.visited(it.item()))
										{
											QueryRec rec = new QueryRec
												{
													id = it.item(),
													item = itm,
													distance = (distance -1),
												};
											
											items.Add(rec);
										}
								}
						}
				}
			
			return items;
		}
	}
}
