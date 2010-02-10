
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
	}
	
	public class MergeHist<T>
	{
		private IVisitor<T> _visitor;
		private VersionControlServer _vcs;
		
		public MergeHist(VersionControlServer vcs, IVisitor<T> visitor)
		{
			_vcs = vcs;
			_visitor = visitor;
		}
		
		/// <summary>
		/// this function acts as a proxy for the function below it.
		/// </summary>
		public void queryMerge(Changeset cs, string targetPath, int distance)
		{
			VersionSpec targetVer = new ChangesetVersionSpec(cs.ChangesetId);

			/* pull down the item type. 
			 * i need this to determine params for the querymergedetails call.
			 */
			Item itm = _vcs.GetItem(targetPath, targetVer, 0, false);
			
			queryMerge(cs, itm, distance);
		}
		
		public void queryMerge(Changeset cs, Item targetItem, int distance)
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
			if (distance == 0) { return; }
			
			if (targetItem.ItemType != ItemType.File) 
				{ recurType = RecursionType.Full; }
			
			mergedetails = 
				_vcs.QueryMergesWithDetails(srcPath, srcVer, srcDelID,
																	 targetItem.ServerItem, targetVer, 
																	 targetItem.DeletionId, 
																	 fromVer, toVer, recurType);
			
			
			visitedItems = _processDetails(cs.ChangesetId, mergedetails);
						
			data = _visitor.visit(targetItem.ServerItem, cs);
			
			/* now walk the list of compiled changesetid + itempath and 
			 * construct a versioned item for each that we can actually go and query.
			 */
			List<KeyValuePair<int,Item>> items = new List<KeyValuePair<int,Item>>();
			for(ChangesetDict_T.iterator it= visitedItems.begin();
					it != visitedItems.end();
					++it)
				{
					ChangesetVersionSpec mergedSrcVer = new ChangesetVersionSpec(it.item());
					Item itm = null;
					SortedPaths_T.iterator pathsIt = it.value().begin();
					
					//Console.WriteLine("parent: {0}", pair.Key);
					_visitor.addParent(data, it.item());
					
					if (it.value().size() > 1)
						{
							/* so, find out what this branch is and use the changeset id. */
							string thisBranch = Utils.GetEGSBranch(pathsIt.item());
							string pathPart = Utils.GetPathPart(targetItem.ServerItem);
						
							//Console.WriteLine("---- {0} => {1} + {2}", pair.Value.Values[0], thisBranch, pathPart);
							if (! string.IsNullOrEmpty(thisBranch))
								{
									itm = _vcs.GetItem(thisBranch + "/EGS/" + pathPart, mergedSrcVer, 0, false);
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
							
							itm = _vcs.GetItem(pathsIt.item(), mergedSrcVer, 0, false);
						}
					
					/* queue it. */
					if (itm != null) { items.Add(new KeyValuePair<int,Item>(it.item(), itm)); }
				}
			
			{
				/* run the parent queries. */
				foreach(KeyValuePair<int,Item> itm in items)
					{
						Changeset newcs = _vcs.GetChangeset(itm.Key);
						System.Console.WriteLine("[q={0}, {1}, {2}]", 
																		 itm.Value.ServerItem, 
																		 itm.Key, itm.Value.DeletionId);
						
						queryMerge(newcs, itm.Value, (distance-1));
					}
			}
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
