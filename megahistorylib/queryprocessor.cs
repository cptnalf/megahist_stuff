
using System.Threading;
using Microsoft.TeamFoundation.VersionControl.Client;
using System.Collections.Generic;

namespace megahistorylib
{
	internal class QueryProcessor
	{
		private saastdlib.AsyncQueue<QueryRec> _queries = 
			new saastdlib.AsyncQueue<QueryRec>(int.MaxValue);
		private MegaHistory _megahist;
		private int _threadCount;
		
		internal QueryProcessor(MegaHistory megahist, int threadCount)
		{
			_threadCount = threadCount;
			_megahist = megahist;
		}

		/// <summary>
		/// add a new item to the queue
		/// </summary>
		internal void push(int id, Item itm, int distance)
		{
			QueryRec rec = new QueryRec
				{
					id = id,
					item = itm,
					distance = distance,
				};
			
			_queries.push(rec); 
		}
		
		/// <summary>
		/// run the threaded querymerges workers
		/// </summary>
		internal void runThreads()
		{
			Thread[] threads = new Thread[_threadCount];
			saastdlib.Timer t = new saastdlib.Timer();
			
			Logger.logger.DebugFormat("qp[using {0} threads]", _threadCount);
			
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
			if (_queries.Count > 0) 
				{ throw new System.Exception("queue not empty!"); }

			/* report some statistics. */
			Logger.logger.DebugFormat("qp<mh>[{0} queries took {1}]",
																_megahist.QueryCount, _megahist.QueryTime);
			Logger.logger.DebugFormat("qp<mh>[{0} get items took {1}]",
																_megahist.GetItemCount, _megahist.GetItemTime);
			Logger.logger.DebugFormat("qp<mh>[{0} get changesets took {1}]",
																_megahist.GetChangesetCount, _megahist.GetChangesetTime);
			Logger.logger.DebugFormat("qp<mh>[clock time={0}]", t.Delta);
		}
		
		private void _qm_worker(object arg)
		{
			bool done = false;
			Changeset cs = null;
			
			while (!done)
				{
					bool signaled = _queries.ItemsWaiting.WaitOne(100);
					
					if (signaled)
						{
							QueryRec rec = _queries.pop();
							
							if (rec != null)
								{
									cs = _megahist.getCS(rec.id);
									
									/* run the query. */
									IList<QueryRec> recs = _megahist.queryMerge(cs, rec.item, rec.distance);
									
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
