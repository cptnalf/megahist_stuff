
using System.Threading;
using Microsoft.TeamFoundation.VersionControl.Client;
using System.Collections.Generic;

namespace TFSTree.Databases.TFSDB
{
	using MergeHist = megahistory.MergeHist<Revision>;
	using QueryRec = megahistory.MergeHistQueryRec;
	using IEnumerable = System.Collections.IEnumerable;
	using RevisionCont = treelib.AVLTree<Revision, RevisionSorterDesc>;
	using BranchContainer = treelib.AVLTree<string, treelib.StringSorterInsensitive>;
	using RevisionIdx = treelib.AVLDict<string, Revision>;
	using BranchChangesets =
		treelib.AVLDict<string, treelib.AVLDict<string, Revision>, treelib.StringSorterInsensitive>;
	
	public partial class TFSDB
	{
		private Mutex _csMux = new Mutex();

		private void _runQuery(string branch, ulong limit, string startID)
		{
			TFSDBVisitor visitor = new TFSDBVisitor();
			treelib.AVLTree<Changeset,ChangesetDescSorter> history = 
				new treelib.AVLTree<Changeset,ChangesetDescSorter>();
			AsyncQueue<QueryRec> queries = new AsyncQueue<QueryRec>(int.MaxValue);
			
			VersionSpec fromVer = null;
			VersionSpec toVer = null;
			
			if (startID != null) { toVer = new ChangesetVersionSpec(startID); }
			
			IEnumerable foo =
				_vcs.QueryHistory(branch, VersionSpec.Latest, 0, RecursionType.Full,
													null, fromVer, toVer, /* user, from ver, to ver */
													(int)limit, 
													true, false, false); 
			/* inc changes, slot mode, inc download info. */
			
			logger.DebugFormat("q[{0},{1}]", branch, limit);
			
			/* sort the changesets in Descending order */
			foreach (object o in foo)
				{
					Changeset cs = o as Changeset;
					history.insert(cs);
				}
			
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
			
			if (OnProgress != null) { OnProgress(this, new ProgressArgs(20, "finished history query")); }
			
			_insertQueries(history, queries, branch, visitor);

			/* do the queries in parallel.
			 * after that's done, walk the list of changesets marking parents correctly.
			 */
			_runThreads(queries, visitor);
			if (OnProgress != null) { OnProgress(this, new ProgressArgs(60, "finished merge queries")); }
			
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
						_cache.del(dr);
					}
			}
			if (OnProgress != null) { OnProgress(this, new ProgressArgs(10, "finished history fixage")); }
			
			
			
			/* now we need to now save the stuff we just queried */
			BranchChangesets vBC = visitor.getBranchChangesets();
			for(BranchChangesets.iterator bit = vBC.begin();
					bit != vBC.end();
					++bit)
				{
					for(RevisionIdx.iterator rit = bit.value().begin();
							rit != bit.value().end();
							++rit)
						{
							logger.DebugFormat("s[{0}]", rit.value().ID);
							
							/* database. */
							_cache.save(rit.value());
							
							/* local cache. */
							_addRevision(rit.value());
						}
				}
			if (OnProgress != null) { OnProgress(this, new ProgressArgs(10, "finished persisting the results")); }
		}

		private void _runThreads(AsyncQueue<QueryRec> queries, TFSDBVisitor visitor)
		{
			MergeHist mergeHist = new MergeHist(_vcs, visitor);
			Thread[] threads = new Thread[THREAD_COUNT];
			Timer t = new Timer();			
			
			t.start();
			for(int i=0; i < threads.Length; ++i)
				{
					threads[i] = new Thread(new ParameterizedThreadStart(_qm_worker));
						
						threads[i].Priority = ThreadPriority.Lowest;
						threads[i].Start(new object[] { queries, mergeHist, visitor });
				}
			
			for(int i=0; i < threads.Length; ++i) { threads[i].Join(); }
			t.stop();
			
			/* sanity check. */
			if (queries.Count > 0) { throw new System.Exception("fuck!"); }
		
			/* report some statistics. */
			logger.DebugFormat("qm[{0} queries took {1}]", 
												 mergeHist.QueryCount, mergeHist.QueryTime);
			logger.DebugFormat("qm[{0} get items took {1}]", 
												 mergeHist.GetItemCount, mergeHist.GetItemTime);
			logger.DebugFormat("qm[{0} get changesets took {1}]", 
												 mergeHist.GetChangesetCount, mergeHist.GetChangesetTime);
			logger.DebugFormat("qm[clock time={0}]", t.Delta);
		}
		
		private void _thrAddRev(Revision rev)
		{
			if (_csMux.WaitOne())
				{
					_addRevision(rev);
				}
			_csMux.ReleaseMutex();
		}
		
		private void _qm_worker(object arg)
		{
			object[] data = arg as object[];
			AsyncQueue<QueryRec> queries = data[0] as AsyncQueue<QueryRec>;
			MergeHist mergeHist = data[1] as MergeHist;
			bool done = false;
			Changeset cs = null;
			
			while (!done)
				{
					bool signaled = queries.ItemsWaiting.WaitOne(500);
					
					if (signaled)
						{
							QueryRec rec = queries.pop();
							
							if (rec != null)
								{
									Revision rev = _cache.rev(MakeID(rec.id));
									if (rev != null)
										{
											_thrAddRev(rev);
										}
									else
										{
											cs = mergeHist.getCS(rec.id);
											
											/* run the query. */
											List<QueryRec> recs = mergeHist.queryMerge(cs, rec.item, rec.distance);
											
											/* dump the extra queries into the queue */
											foreach(var r in recs) { queries.push(r); }
										}
								}
							else { done = true; }
						}
					else { done = true; }
				}
		}
		
		private void _insertQueries(treelib.AVLTree<Changeset,ChangesetDescSorter> history,
																AsyncQueue<QueryRec> queries, 
																string branch, 
																TFSDBVisitor visitor)
		{
			treelib.AVLTree<Changeset,ChangesetDescSorter>.iterator it = history.begin();
			
			for(; it != history.end(); ++it)
				{
					/* grab the branches for this changeset. */
					List<string> branchParts = megahistory.Utils.FindChangesetBranches(it.item());
					
					logger.DebugFormat("saw[{0}]", it.item().ChangesetId);

					/* do a cache lookup first. */
					Revision rev = _cache.rev(MakeID(it.item().ChangesetId));
					
					if (rev != null)
						{
							/* store it in the in-memory cache */
							_addRevision(rev);
						}
					else
						{
							for(int bpi = 0; bpi < branchParts.Count; ++bpi)
								{
									Item itm = _vcs.GetItem(branchParts[bpi], 
																					new ChangesetVersionSpec(it.item().ChangesetId));
									
									logger.DebugFormat("qm[{0},{1}]", itm.ServerItem, it.item().ChangesetId);
									
									/* queue the visiting work. */
									QueryRec rec = new QueryRec
										{
											id = it.item().ChangesetId,
												item = itm,
												distance = RECURSIVE_QUERY_COUNT,
												};
									
									queries.push(rec);
								}
							
							if (branchParts.Count == 0)
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
