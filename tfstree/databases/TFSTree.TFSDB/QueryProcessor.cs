
using System.Threading;
using Microsoft.TeamFoundation.VersionControl.Client;
using System.Collections.Generic;

namespace TFSTree.Databases.TFSDB
{
	using MergeHist = megahistory.MergeHist<Revision>;
	using QueryRec = megahistory.MergeHistQueryRec;

	internal class QueryProcessor
	{
		private AsyncQueue<QueryRec> _queries = new AsyncQueue<QueryRec>(int.MaxValue);
		private MergeHist _mergeHist;
		private VersionControlServer _vcs;
		private TFSDBVisitor _visitor;
		
		internal QueryProcessor(TFSDBVisitor visitor, VersionControlServer vcs)
		{
			_vcs = vcs;
			_visitor = visitor;
			
			_mergeHist = new MergeHist(_vcs, _visitor);
		}

		internal void runThreads()
		{
			Thread[] threads = new Thread[TFSDB.THREAD_COUNT];
			Timer t = new Timer();

			t.start();
			for (int i = 0; i < threads.Length; ++i)
				{
				threads[i] = new Thread(new ParameterizedThreadStart(_qm_worker));

				threads[i].Priority = ThreadPriority.Lowest;
				threads[i].Start(null);
				}

			for (int i = 0; i < threads.Length; ++i) { threads[i].Join(); }
			t.stop();

			/* sanity check. */
			if (_queries.Count > 0) { throw new System.Exception("fuck!"); }

			/* report some statistics. */
			TFSDB.logger.DebugFormat("qm[{0} queries took {1}]",
												 _mergeHist.QueryCount, _mergeHist.QueryTime);
			TFSDB.logger.DebugFormat("qm[{0} get items took {1}]",
												 _mergeHist.GetItemCount, _mergeHist.GetItemTime);
			TFSDB.logger.DebugFormat("qm[{0} get changesets took {1}]",
												 _mergeHist.GetChangesetCount, _mergeHist.GetChangesetTime);
			TFSDB.logger.DebugFormat("qm[clock time={0}]", t.Delta);
		}
		
		private void _qm_worker(object arg)
		{
			bool done = false;
			Changeset cs = null;
			
			while (!done)
				{
					bool signaled = _queries.ItemsWaiting.WaitOne(500);
					
					if (signaled)
						{
							QueryRec rec = _queries.pop();
							
							if (rec != null)
								{
									cs = _mergeHist.getCS(rec.id);
									
									/* run the query. */
									List<QueryRec> recs = _mergeHist.queryMerge(cs, rec.item, rec.distance);
									
									/* dump the extra queries into the queue */
									foreach(var r in recs) { _queries.push(r); }
								}
							else { done = true; }
						}
					else { done = true; }
				}
		}
	}
}
