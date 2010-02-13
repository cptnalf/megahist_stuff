
using Microsoft.TeamFoundation.VersionControl.Client;
using System.Collections.Generic;

namespace megahistory
{
	using SortedPaths_T = treelib.AVLTree<string, treelib.StringSorterInsensitive>;
	using ChangesetDict_T = 
		treelib.AVLDict<int, treelib.AVLTree<string, treelib.StringSorterInsensitive> >;
	
	public interface IVisitor<T>
	{
		T visit(string branch, Changeset cs);
		void addParent(T data, int parentID);
		bool visited(string branch, int csID);
	}
	
	public class MergeHistQueryRec
	{
		public int id { get;set;}
		public Item item { get;set; }
		public int distance { get;set; }
	}
	
	public class MergeHist<T>
	{
		private IVisitor<T> _visitor;
		private VersionControlServer _vcs;
		
		private ulong _qc = 0;
		private Timer _qt = new Timer();
		
		private ulong _gic = 0;
		private Timer _git = new Timer();

		private ulong _gcc = 0;
		private Timer _gct = new Timer();
		
		private Item _getItem(string targetPath, int csID, int deletionID, bool downloadInfo)
		{
			Timer t = new Timer();
			VersionSpec targetVer = new ChangesetVersionSpec(csID);
			
			t.start();
			Item itm = _vcs.GetItem(targetPath, targetVer, 0, false);
			t.stop();
			
			lock(_git)
				{
					++_gic;
					_git.TotalT += t.DeltaT;
				}
			
			return itm;
		}
		
		private Item _getItem(string targetPath) { return _vcs.GetItem(targetPath); }

		public Changeset getCS(int csID)
		{
			Timer t = new Timer();
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
		
		public ulong QueryCount { get { return _qc; } }
		public ulong GetItemCount { get { return _gic; } }
		public ulong GetChangesetCount { get { return _gcc; } }
		
		public System.TimeSpan QueryTime { get { return _qt.Total; } }
		public System.TimeSpan GetItemTime { get { return _git.Total; } }
		public System.TimeSpan GetChangesetTime { get { return _gct.Total; } }
		
		public MergeHist(VersionControlServer vcs, IVisitor<T> visitor)
		{
			_vcs = vcs;
			_visitor = visitor;
			
			MegaHistory.LoadLogger();
		}
		
		/// <summary>
		/// this function acts as a proxy for the function below it.
		/// </summary>
		public List<MergeHistQueryRec> queryMerge(Changeset cs, string targetPath, int distance)
		{
			Timer t = new Timer();
			t.start();
			
			/* pull down the item type. 
			 * i need this to determine params for the querymergedetails call.
			 */
			Item itm = _getItem(targetPath, cs.ChangesetId, 0, false);
			
			List<MergeHistQueryRec> queries = queryMerge(cs, itm, distance);
			
			t.stop();
			MegaHistory.logger.DebugFormat("qm[{0} queries took {1}]", this.QueryCount, this.QueryTime);
			MegaHistory.logger.DebugFormat("qm[{0} get items took {1}]", this.GetItemCount, this.GetItemTime);
			MegaHistory.logger.DebugFormat("qm[{0} get changesets took {1}]", this.GetChangesetCount, this.GetChangesetTime);
			MegaHistory.logger.DebugFormat("qm[total time {1}]", t.Total);
			
			return queries;
		}
		
		public List<MergeHistQueryRec> queryMerge(Changeset cs, Item targetItem, int distance)
		{
			string srcPath = null;
			VersionSpec srcVer = null;
			int srcDelID = 0;
			VersionSpec targetVer = new ChangesetVersionSpec(cs.ChangesetId);
			VersionSpec fromVer = targetVer;
			VersionSpec toVer = targetVer;
			RecursionType recurType = RecursionType.None; /* assume these are all files */
			ChangesetMergeDetails mergedetails;
			T data;
			
			ChangesetDict_T visitedItems;
			/* map a changeset id to a list of items in that changeset which were merged.
			 * (that we happen to care about.
			 */
			
			/* don't do queries when we've reached the distance limit. */
			if (distance == 0) { return new List<MergeHistQueryRec>(); }
			
			if (targetItem.ItemType != ItemType.File) 
				{ recurType = RecursionType.Full; }
			
			Timer t = new Timer();
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
				
				MegaHistory.logger.DebugFormat("v[{0}{1}]", itemPath, cs.ChangesetId);
				data = _visitor.visit(itemPath, cs);
			}
			
			/* now walk the list of compiled changesetid + itempath and 
			 * construct a versioned item for each that we can actually go and query.
			 */
			List<MergeHistQueryRec> items = new List<MergeHistQueryRec>();
			for(ChangesetDict_T.iterator it= visitedItems.begin();
					it != visitedItems.end();
					++it)
				{
					Item itm = null;
					SortedPaths_T.iterator pathsIt = it.value().begin();
					
					//Console.WriteLine("parent: {0}", pair.Key);
					_visitor.addParent(data, it.item());
					
					if ((distance -1) > 0)
						{
							/* only query the item info if we're going to do a query on it. */
							
							if (it.value().size() > 1)
								{
									/* so, find out what this branch is and use the changeset id. */
									string thisBranch = Utils.GetEGSBranch(pathsIt.item());
									string pathPart = Utils.GetPathPart(targetItem.ServerItem);
									
									//Console.WriteLine("---- {0} => {1} + {2}", pair.Value.Values[0], thisBranch, pathPart);
									if (! string.IsNullOrEmpty(thisBranch))
										{
											MegaHistory.logger.DebugFormat("iqm[{0},{1}]", thisBranch+"/EGS/"+pathPart, it.item());
											
											itm = _getItem(thisBranch + "/EGS/" + pathPart, it.item(), 0, false);
										}
									else
										{
											System.Console.WriteLine("---[e] {0}", pathsIt.item());
										}
								}
							else
								{
									/* so, this is the only file we noticed for this changeset,
									 * spawn a new query for just this file (folder?)
									 */
									
									MegaHistory.logger.DebugFormat("iq1[{0},{1}]", pathsIt.item(), it.item());
							
									try {
										itm = _getItem(pathsIt.item(), it.item(), 0, false);
									} catch(System.Exception ex)
										{
											MegaHistory.logger.Fatal("fatal item query:", ex);
									
											try{
												/* try just the path, i doubt this will work either, but *shrug* */
												itm = _getItem(pathsIt.item());
											}catch(System.Exception) { itm = null; }
										}
								}
					
							/* queue it. */
							if (itm != null) 
								{
									if (!_visitor.visited(itm.ServerItem, it.item()))
										{
											MergeHistQueryRec rec = new MergeHistQueryRec
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
		
		private ChangesetDict_T _processDetails(int csID, ChangesetMergeDetails mergedetails)
		{
			ChangesetDict_T visitedItems = new ChangesetDict_T();
			/* this should always be 1 when the targetpath is a file. */
			foreach(ItemMerge m in mergedetails.MergedItems)
				{
					if (m.TargetVersionFrom != csID)
						{
							System.Console.WriteLine("merged to a different changeset? {0}=>{1} vs {2}",
																			 m.SourceVersionFrom, m.TargetVersionFrom,
																			 csID);
						}
						
					/* pull a list of changesets we want to visit from the merged items we get. */
					ChangesetDict_T.iterator dictIt = visitedItems.find(m.SourceVersionFrom);
					
					if (dictIt == visitedItems.end())
						{
							SortedPaths_T itemList = new SortedPaths_T();
							
							/* add the new item. */
							itemList.insert(m.SourceServerItem);
							visitedItems.insert(m.SourceVersionFrom, itemList);
						}
					else { dictIt.value().insert(m.SourceServerItem); }
				}
			return visitedItems;
		}
	}
}
