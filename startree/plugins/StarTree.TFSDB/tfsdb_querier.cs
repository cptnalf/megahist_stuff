
using Microsoft.TeamFoundation.VersionControl.Client;
using System.Collections.Generic;

namespace StarTree.Plugin.TFSDB
{
	using Revision = StarTree.Plugin.Database.Revision;
	using MergeHist = megahistory.MergeHist<StarTree.Plugin.Database.Revision>;
	using QueryRec = megahistory.MergeHistQueryRec;
	using IEnumerable = System.Collections.IEnumerable;
	using RevisionCont = treelib.AVLTree<StarTree.Plugin.Database.Revision, StarTree.Plugin.Database.RevisionSorterDesc>;
	using BranchContainer = treelib.AVLTree<string, treelib.StringSorterInsensitive>;
	using RevisionIdx = treelib.AVLDict<string, StarTree.Plugin.Database.Revision>;
	using ChangesetsDesc = treelib.AVLTree<Changeset,ChangesetDescSorter>;
	
	internal partial class TFSDB
	{
		private VersionControlServer _vcs;
		private string _branch;
		private TFSDBVisitor _visitor;
		
		internal TFSDB(VersionControlServer vcs, string branch, TFSDBVisitor visitor)
		{
			_vcs = vcs;
			_branch = branch;
			_visitor = visitor;
		}
		
		internal ChangesetsDesc queryHistory(ulong limit, string startID)
		{
			ChangesetsDesc history = new ChangesetsDesc();
			
			VersionSpec fromVer = null;
			VersionSpec toVer = null;
			
			if (startID != null) { toVer = new ChangesetVersionSpec(startID); }

			logger.DebugFormat("qh[{0},{1}]", _branch, limit);
			IEnumerable foo =
				_vcs.QueryHistory(_branch, VersionSpec.Latest, 0, RecursionType.Full,
													null, fromVer, toVer, /* user, from ver, to ver */
													(int)limit, 
													true, false, false); 
			/* inc changes, slot mode, inc download info. */
			
			/* sort the changesets in Descending order */
			foreach (object o in foo) { history.insert(o as Changeset); }
			
			return history;
		}

		internal void queryMerges(ChangesetsDesc history, SQLiteStorage.SQLiteCache cache)
		{
			{
				/* this is used by the 'FindChangesetBranches' to figure out which
				 * changes to pay attention to.
				 */
				megahistory.MegaHistory.IsChangeToConsider =
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
			//_onProgress(20, "finished history query");

			/* do the queries in parallel.
			 * after that's done, walk the list of changesets marking parents correctly.
			 */
			QueryProcessor qp = new QueryProcessor(_visitor, _vcs);
			
			_insertQueries(history, qp, cache);
			
			qp.runThreads();
			//_onProgress(60, "finished merge queries");
		}
		
		/// <summary>
		/// this adds in the parents as part of the branch's standard history list.
		/// </summary>
		/// <param name="history"></param>
		/// <param name="visitor"></param>
		internal void fixHistory(ChangesetsDesc history)
		{
			int i = 0;
			treelib.AVLTree<Changeset, ChangesetDescSorter>.iterator it;
			Revision prev = null;
						
			for(it = history.begin(); it != history.end(); ++it)
				{
					Changeset cs = it.item();
					string id = TFSDB.MakeID(cs.ChangesetId);
					
					if (prev != null)
						{
							/* fix up the parent.
							 * this will only add the parent if they're not already in the list.
							 */
							prev.addParent(id);
							if (null == _visitor.rev(prev.ID)) { _visitor.addRevision(prev, true); }
						}
					
					Revision rev = _visitor.rev(id);
					
					if (rev == null)
						{
							logger.ErrorFormat("didn't find {0}!", id);
						}
										
					prev = rev;
					++i;
				}
		
			//_onProgress(10, "finished history fixage");
		}
		
		private bool _handleVisit(Changeset cs, QueryProcessor qp, int depth)
		{
			/* grab the branches for this changeset. */
			List<string> branchParts = megahistory.Utils.FindChangesetBranches(cs);

			/* i need to find a path to use as a base item for this merge history query. 
			 *
			 * this could skew the results a little bit.
			 * unfortunately, to support renames, i need to get the previous name, 
			 * which might be in the changeset history (?)
			 * 
			 * however, this won't detect situations where the user checked-in
			 * changes on two separate branches.
			 */			
			for (int bpi = 0; bpi < branchParts.Count; ++bpi)
				{
					Item itm = _vcs.GetItem(branchParts[bpi],
																	new ChangesetVersionSpec(cs.ChangesetId));

					logger.DebugFormat("qm[{0},{1}]", itm.ServerItem, cs.ChangesetId);
					qp.push(cs.ChangesetId, itm, depth);
				}
			
			return branchParts.Count > 0;
		}
		
		private void _insertQueries(ChangesetsDesc history,
																QueryProcessor qp,
																SQLiteStorage.SQLiteCache cache)
		{
			ChangesetsDesc.iterator it = history.begin();
			
			for(; it != history.end(); ++it)
				{					
					logger.DebugFormat("saw[{0}]", it.item().ChangesetId);

					/* do a cache lookup first. */
					Revision rev = cache.rev(it.item().ChangesetId);
					
					if (rev != null)
						{
							_visitor.addRevision(rev);
							foreach(string parent in rev.Parents)
								{
									Revision pr = cache.rev(parent);
									if (pr == null)
										{
											/* so, we need to do a 2nd level query, but not a first level. */
											Changeset cs = _vcs.GetChangeset(int.Parse(parent));
											
											_handleVisit(cs, qp, RECURSIVE_QUERY_COUNT -1);
										}
								}
						}
					else
						{
							if (! _handleVisit(it.item(), qp, RECURSIVE_QUERY_COUNT))
								{
									/* manufacture a visit since this changeset
									 * has no merge actions to get branches from
									 */
									_visitor.visit(_branch, it.item());
								}
						}
				}
		}
	}
}
