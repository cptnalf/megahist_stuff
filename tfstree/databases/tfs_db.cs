
/*
 */

using System.Collections.Generic;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace TFSTree
{
	using IEnumerable = System.Collections.IEnumerable;
	using RevisionCont = treelib.AVLTree<Revision>;
	using BranchContainer = treelib.AVLTree<string, StringSorterInsensitive>;
	using ChangesetIdx = treelib.AVLDict<int,Revision,IntSorterDesc>;
	using BranchChangesets = 
		treelib.AVLDict<string,treelib.AVLDict<int,Revision,IntSorterDesc>,StringSorterInsensitive>;

	
	internal class TFSDB : IRevisionDB
	{
		private ChangesetIdx _changesetIdx = new ChangesetIdx();
		private BranchContainer _branches;
		private string _tfsServerName;
		private VersionControlServer _vcs;
		
		internal TFSDB() {	}
		
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
			if (it != _branchChangesets.end())
				{
					if (it.value().size() < limit) { _requery(branch, limit); }
					
					ulong count = (it.value().size() > limit) ? limit : it.value().size();
					ChangesetIdx.iterator csit = it.value().begin();
					
					for(ulong i=0; i < count; ++i, ++csit)
						{
							if (csit == it.value().end()) { break; }
							revisions.Add(csit.value().ID, csit.value());
						}
				}
			else
				{
					/* try to populate it... */
					_getRevisions(branch, limit);
				}
			
			return revisions;
		}
		
		public void load(string filename)
		{
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
		
		private void _getRevisions(string branch, ulong limit)
		{
			Item itm = _vcs.GetItem(branch);
			ChangesetVersionSpec srcVer = new ChangesetVersionSpec(itm.ChangesetId);
			
			IEnumerable foo =
				_vcs.QueryHistory(itm.ServerItem, srcVer, 0, RecursionType.Full,
													null, null, null, /* user, from ver, to ver */
													(int)limit, 
													true, false, false); 
			/* inc changes, slot mode, inc download info. */
			
			foreach (object o in foo)
				{
					Changeset cs = o as Changeset;
					
					/* i need to find a path to use as a base item for this merge history query. 
					 *
					 * this could skew the results a little bit.
					 * unfortunately, to support renames, i need to get the previous name, 
					 * which might be in the changeset history (?)
					 * 
					 * however, this won't detect situations where the user checked-in
					 * changes on two separate branches.
					 */
					List<string> branchParts = megahistory.Utils.FindChangesetBranches(cs);
					
					for(int bpi = 0; bpi < branchParts.Count; ++bpi)
						{
							Item itm = _vcs.GetItem(branchParts[bpi], new ChangesetVersionSpec(cs.ChangesetId));
							
							_queryMerge(_vcs, cs, branchParts[bpi], cs.ChangesetId, itm.DeletionId, 1, 
													_addRevision, 
													(data, parentID) => { ((Revision)data).addParent(parentID); });
						}
				}
		}

		private object _addRevision(string branch, Changeset cs)
		{
			Revision rev = new Revision(cs.ChangesetId, branchPart, 
																	cs.Owner, cs.CreationDate, 
																	cs.Comment);
										
			ChangesetIdx.iterator csit = _changesetIdx.find(cs.ChangesetId);
			if (csit != _changesetIdx.end())
				{
					_changesetIdx.insert(cs.ChangesetId, rev); 
					
					BranchChangesets.iterator it = _branchChangesets.find(branch);
					if (it != _branchChangesets.end())
						{ it.value().insert(cs.ChangesetId, rev); }
					else
						{
							ChangesetIdx revisionsIdx = new ChangesetIdx();
							revisionsIdx.insert(cs.ChangesetId, rev);
							_branchChangesets.insert(branch, revisionsIdx);
						}
				}
			
			return rev;
		}
		
		internal delegate object QueryMergeOp(string branch, Changeset cs);
		internal delegate void QueryMergeParentOp(object data, int parentID);
		
		internal static void _queryMerge(VersionControlServer vcs, Changeset cs,
																		 string targetPath, int targetChangesetID, int targetDelID,
																		 int distance, QueryMergeOp op, QueryMergeParentOp parentOp)
		{
			string srcPath = null;
			VersionSpec srcVer = null;
			int srcDelID = 0;
			VersionSpec targetVer = new ChangesetVersionSpec(targetChangesetID);
			VersionSpec fromVer = targetVer;
			VersionSpec toVer = targetVer;
			RecursionType recurType = RecursionType.None; /* assume these are all files */
			ChangesetMergeDetails mergedetails;
			
			SortedDictionary<int, SortedList<string,string>> visitedItems =
				new SortedDictionary<int, SortedList<string,string>>();
			/* map a changeset id to a list of items in that changeset which were merged.
			 * (that we happen to care about.
			 */
			
			/* don't do queries when we've reached the distance limit. */
			if (distance == 0) { return; }
			
			{
				/* pull down the item type. 
				 * i need this to determine params for the querymergedetails call.
				 */
				Item itm = vcs.GetItem(targetPath, targetVer, targetDelID, false);
				if (itm.ItemType != ItemType.File) { recurType = RecursionType.Full; }
			}
			
			mergedetails = 
				vcs.QueryMergesWithDetails(srcPath, srcVer, srcDelID,
																	 targetPath, targetVer, targetDelID, 
																	 fromVer, toVer, recurType);
			
			
			/* this should always be 1 when the targetpath is a file. */
			foreach(ItemMerge m in mergedetails.MergedItems)
				{
					if (m.TargetVersionFrom != targetChangesetID)
						{
							Console.WriteLine("merged to a different changeset? {0}=>{1} vs {2}",
																m.SourceVersionFrom, m.TargetVersionFrom,
																targetChangesetID);
						}
					
					/* pull a list of changesets we want to visit from the merged items we get. */
					SortedList<string,string> itemList;
					
					if (! visitedItems.TryGetValue(m.SourceVersionFrom, out itemList))
						{
							/* add the new item. */
							itemList = new SortedList<string,string>();
							itemList.Add(m.SourceServerItem, m.SourceServerItem);
							
							visitedItems.Add(m.SourceVersionFrom, itemList);
						}
					else { itemList.Add(m.SourceServerItem, m.SourceServerItem); }
				}
			
			object data = op(targetPath, cs);
			
			/* now walk the list of compiled changesetid + itempath and 
			 * construct a versioned item for each that we can actually go and query.
			 */
			List<KeyValuePair<int,Item>> items = new List<KeyValuePair<int,Item>>();
			foreach(KeyValuePair<int,SortedList<string,string>> pair in visitedItems)
				{
					ChangesetVersionSpec mergedSrcVer = new ChangesetVersionSpec(pair.Key);
					Item itm = null;
				
					//Console.WriteLine("parent: {0}", pair.Key);
					if (data != null) { parentOp(data, pair.Key); }
					
					if (pair.Value.Count > 1)
						{
							/* so, find out what this branch is and use the changeset id. */
							string thisBranch = GetEGSBranch(pair.Value.Values[0]);
							string pathPart = GetPathPart(targetPath);
						
							//Console.WriteLine("---- {0} => {1} + {2}", pair.Value.Values[0], thisBranch, pathPart);
							if (! string.IsNullOrEmpty(thisBranch))
								{
									itm = vcs.GetItem(thisBranch + "/EGS/" + pathPart, mergedSrcVer, 0, false);
								}
							else
								{
									Console.WriteLine("---[e] {0}", pair.Value.Values[0]);
								}
						}
					else
						{
							/* so, this is the only file we noticed for this changeset,
							 * spawn a new query for just this file (folder?)
							 */
						
							itm = vcs.GetItem(pair.Value.Values[0], mergedSrcVer, 0, false);
						}
				
					/* queue it. */
					if (itm != null) { items.Add(new KeyValuePair<int,Item>(pair.Key, itm)); }
				}
			
			{
				/* run the parent queries. */
				foreach(KeyValuePair<int,Item> itm in items)
					{
						Console.WriteLine("[q={0},{1}, {2}, {3}]", 
															targetChangesetID, itm.Value.ServerItem, itm.Key, itm.Value.DeletionId);
						
						
						test_any(vcs, targetChangesetID, itm.Value.ServerItem, itm.Key, itm.Value.DeletionId, 
										 (distance-1));
					}
			}
		}
	}
	
}