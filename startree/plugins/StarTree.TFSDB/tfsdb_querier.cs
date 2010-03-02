
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
		private ChangesetsDesc _history;
		private QueryProcessor _qp;
		private SQLiteStorage.SQLiteCache _cache;
		
		internal ChangesetsDesc history { get { return _history; } }
		internal TFSDBVisitor visitor { get { return _visitor; } }
		
		internal TFSDB(VersionControlServer vcs, string branch, 
									 SQLiteStorage.SQLiteCache cache)
		{
			_vcs = vcs;
			_branch = branch;
			_cache = cache;
			
			_visitor = new TFSDBVisitor();
			
			_qp = new QueryProcessor( _visitor, _vcs);
		}
		
		internal void queryMerges()
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
			_insertQueries();
			
			_qp.runThreads();
			//_onProgress(60, "finished merge queries");
		}
		
		/// <summary>
		/// this adds in the parents as part of the branch's standard history list.
		/// </summary>
		/// <param name="history"></param>
		/// <param name="visitor"></param>
		internal void fixHistory()
		{
			Revision prev = null;
						
			foreach(Changeset cs in _history)
				{
					string id = TFSDB.MakeID(cs.ChangesetId);
					
					if (prev != null)
						{
							/* fix up the parent.
							 * this will only add the parent if they're not already in the list.
							 */
							prev.addParent(id);
						}
					
					/* lookup the item for the next round. 
					 * everything we're looking for should already be in the visitor
					 * if it's not, there's a problem with the algorithm.
					 */
					Revision rev = _visitor.rev(id);
					
					/* sanity check. */
					if (rev == null) { logger.ErrorFormat("didn't find {0}!", id); }
					
					prev = rev;
				}
		}
		
		private bool _handleVisit(Changeset cs, int depth)
		{
			/* grab the branches for this changeset,
			 * looking for merge branches, because we want to decompose if possible
			 */
			/* @Note
			 * worst case here is that you do a checkin to a branch
			 * of 100k items (say an add or edit).
			 * in that case the process will probably end up trying to find branches
			 * twice :/
			 */
			List<string> branchParts = megahistory.Utils.FindChangesetBranches(cs);

			/* @Note
			 * i need to find a path to use as a base item for this merge history query. 
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
					_qp.push(cs.ChangesetId, itm, depth);
				}
			
			return branchParts.Count > 0;
		}
		
		/// <summary>
		/// insert queries into the processor that haven't already been done.
		/// (eg, in the cache)
		/// </summary>
		private void _insertQueries()
		{
			ChangesetsDesc.iterator it = _history.begin();
			
			for(; it != _history.end(); ++it)
				{
					logger.DebugFormat("saw[{0}]", it.item().ChangesetId);
					
					/* do a cache lookup first. */
					Revision rev = _visitor.rev(MakeID(it.item().ChangesetId));
					
					if (rev == null)
						{ _queueOrVisit(it.item(), RECURSIVE_QUERY_COUNT, _branch); }
					else
						{ _queueParents(rev); }
				}
		}
		
		private void _queueOrVisit(Changeset cs, int distance, string branch)
		{
			/* check the database cache. */
			Revision rev = _cache.rev(cs.ChangesetId);
			
			if (rev == null)
				{
					/* ok, ok, we really don't have it... 
					 * didn't find it, so do the first level query. 
					 */
					if (!_handleVisit(cs, distance))
						{
							/* manufacture a visit since this changeset
							 * has no merge actions to get branches from
							 */
							if (branch == null)
								{
									/* determine a branch from the changesets's changes 
									 * we're not queuing it, so we look for _all_ branches.
									 * (eg, 2nd level :[ ) 
									 */
									List<string> branches = 
										megahistory.Utils.FindChangesetBranches(cs, (cng) => true );
									
									foreach(string b in branches) { _visitor.visit(b, cs); }
								}
							else { _visitor.visit(branch, cs); }
						}
				}
			else
				{
					/* hah! found it in the cache, 
					 * so add it to our in-memory lookup 
					 * this will ensure that the merge-queries are able to see this
					 * already-looked-up item.
					 */
					_visitor.addRevision(rev);
				}
		}
		
		private void _queueParents(Revision rev)
		{
			/* did find it, so try the second level queries. */
			foreach(string parent in rev.Parents)
				{
					/* so, we need to do a 2nd level query, but not a first level. */
					Changeset cs = _vcs.GetChangeset(int.Parse(parent));
					
					_queueOrVisit(cs, RECURSIVE_QUERY_COUNT -1, null);
				}
		}
		
		/// <summary>
		/// this populates the 2nd level queries.
		/// </summary>
		internal void queueParents(Revision rev)
		{
			_queueParents(rev);
			_qp.runThreads();
		}
	}
}
