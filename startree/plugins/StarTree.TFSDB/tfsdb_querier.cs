
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
	using BranchChangesets =
		treelib.AVLDict<string, treelib.AVLDict<string, StarTree.Plugin.Database.Revision>, treelib.StringSorterInsensitive>;
	
	public partial class TFSDB
	{
		private void _runQuery(string branch, ulong limit, string startID)
		{
			TFSDBVisitor visitor = new TFSDBVisitor();
			treelib.AVLTree<Changeset,ChangesetDescSorter> history = 
				new treelib.AVLTree<Changeset,ChangesetDescSorter>();
			AsyncQueue<QueryRec> queries = new AsyncQueue<QueryRec>(int.MaxValue);
			
			VersionSpec fromVer = null;
			VersionSpec toVer = null;
			
			if (startID != null) { toVer = new ChangesetVersionSpec(startID); }

			logger.DebugFormat("qh[{0},{1}]", branch, limit);
			IEnumerable foo =
				_vcs.QueryHistory(branch, VersionSpec.Latest, 0, RecursionType.Full,
													null, fromVer, toVer, /* user, from ver, to ver */
													(int)limit, 
													true, false, false); 
			/* inc changes, slot mode, inc download info. */
			
			/* sort the changesets in Descending order */
			foreach (object o in foo) { history.insert(o as Changeset); }
			
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
			
			visitor.primeBranches(_branches.begin(), _branches.end());
			_insertQueries(history, queries, branch, visitor);
			
			/* do the queries in parallel.
			 * after that's done, walk the list of changesets marking parents correctly.
			 */
			QueryProcessor qp = new QueryProcessor(visitor, _vcs);
			qp.runThreads();
			//_onProgress(60, "finished merge queries");
			
			{
				int i = 0;
				treelib.AVLTree<Changeset, ChangesetDescSorter>.iterator it;
				Revision prev = null;
				bool inMemory = false;
				List<Revision> delRevs = new List<Revision>();
				
				for(it = history.begin(); it != history.end(); ++it)
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
						string id = TFSDB.MakeID(cs.ChangesetId);
						
						if (prev != null)
							{
								if (!inMemory)
									{
										/* did we already query this far or further back?
										 * does this parent already exist?
										 */
										bool found = false;
										foreach(string parent in prev.Parents)
											{ if (id == parent) { found = true; break; } }
										
										if (!found)
											{
												/* deltas would be nice, 
												 * but i think i'm going to purge 
												 * and allow below to add back.
												 */
												delRevs.Add(prev);
												
												/* this will modify the in-memory copy
												 * this ensures that the graphs will look as they are supposed to.
												 */
												prev.addParent(id);
												visitor.addRevision(prev);
											}
									}
								else
									{
										prev.addParent(id);
									}
							}
						
						Revision rev = visitor.rev(id);
						
						if (rev != null)
							{
								inMemory = true;
								/* fill in the item from the queried one. */
								//rev.addParent(prevID);
							}
						else
							{
								/* ok so it wasn't there, try my memory cache. 
								 * this is the original changeset list, 
								 * so if i found these in the db cache, 
								 * they should be in my memory cache.
								 */
								//rev = _cache.rev(id);
								rev = this.rev(id);
								inMemory = false;
							}
						
						prev = rev;
						++i;
					}
				
				logger.DebugFormat("del {0} revs", delRevs.Count);
				foreach(Revision dr in delRevs)
					{
						logger.DebugFormat("d[{0}]", dr.ID);
						this.del(dr);
					}
			}
			//_onProgress(10, "finished history fixage");
			
			/* now we need to now save the stuff we just queried */
			visitor.save(this);
			
			//_onProgress(10, "finished persisting the results");
		}
		
		private bool _handleVisit(Changeset cs, AsyncQueue<QueryRec> queries, int depth)
		{
			/* grab the branches for this changeset. */
			List<string> branchParts = megahistory.Utils.FindChangesetBranches(cs);
			
			for (int bpi = 0; bpi < branchParts.Count; ++bpi)
				{
					Item itm = _vcs.GetItem(branchParts[bpi],
																	new ChangesetVersionSpec(cs.ChangesetId));

					logger.DebugFormat("qm[{0},{1}]", itm.ServerItem, cs.ChangesetId);
					/* queue the visiting work. */
					QueryRec rec = new QueryRec
						{
							id = cs.ChangesetId,
							item = itm,
							distance = RECURSIVE_QUERY_COUNT,
						};

					queries.push(rec);
				}
			
			return branchParts.Count > 0;
		}
		
		private void _insertQueries(treelib.AVLTree<Changeset,ChangesetDescSorter> history,
																AsyncQueue<QueryRec> queries, 
																string branch, 
																TFSDBVisitor visitor)
		{
			treelib.AVLTree<Changeset,ChangesetDescSorter>.iterator it = history.begin();
			
			for(; it != history.end(); ++it)
				{					
					logger.DebugFormat("saw[{0}]", it.item().ChangesetId);

					/* do a cache lookup first. */
					Revision rev = base.rev(MakeID(it.item().ChangesetId));
					
					if (rev != null)
						{
							visitor.addRevision(rev);
							foreach(string parent in rev.Parents)
								{
									Revision pr = base.rev(parent);
									if (pr == null)
										{
											/* so, we need to do a 2nd level query, but not a first level. */
											Changeset cs = _vcs.GetChangeset(int.Parse(parent));
											
											_handleVisit(cs, queries, RECURSIVE_QUERY_COUNT -1);
										}
								}
						}
					else
						{
							if (! _handleVisit(it.item(), queries, RECURSIVE_QUERY_COUNT))
								{
									/* manufacture a visit since this changeset
									 * has no merge actions to get branches from
									 */
									visitor.visit(branch, it.item());
								}
						}
				}
		}
	}
}
