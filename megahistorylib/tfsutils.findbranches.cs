
using System.Collections.Generic;
using	Microsoft.TeamFoundation.VersionControl.Client;
using System;

namespace megahistory
{
	public static partial class Utils
	{
		public static uint FindChangesetBranchesCalls = 0;
		
		/** walk the changes in the changeset and find all unique 'EGS' trees.
		 *  eg:
		 *  $/IGT_0803/main/EGS/shared/project.inc
		 *  $/IGT_0803/development/dev_advantage/EGS/advantage/advantage.sln
		 *  $/IGT_0803/main/EGS/advantage/advantageapps.sln
		 *
		 *  would yield just 2:
		 *  $/IGT_0803/main/EGS/
		 *  $/IGT_0803/development/dev_advantage/EGS/
		 *
		 */
		public static List<string> FindChangesetBranches(Changeset cs)
		{
			Timer timer = new Timer();
			List<string> itemBranches = new List<string>();
			
			++FindChangesetBranchesCalls;
			
			MegaHistory.LoadLogger();
				
			timer.start();
			if (cs.Changes.Length > 1000)
				{
					itemBranches = _get_egs_branches_threaded(cs);
				}
			else
				{
					itemBranches = _get_egs_branches_nonthreaded(cs);
				}
			timer.stop();
			
			MegaHistory.logger.DebugFormat("branches for {0} took: {1}", cs.ChangesetId, timer.Delta);
		
			return itemBranches;
		}
		
		private static List<string> _get_egs_branches_nonthreaded(Changeset cs)
		{
			List<string> itemBranches = new List<string>();
			int changesLen = cs.Changes.Length;
			
			for(int i=0; i < changesLen; ++i)
				{
					
					if (MegaHistory.IsChangeToConsider(cs.Changes[i])	)
						{
							string itemPath = cs.Changes[i].Item.ServerItem;
							bool found = false;
							int idx = 0;
							
							for(int j=0; j < itemBranches.Count; ++j)
								{
									/* the stupid branches are not case sensitive. */
									idx = itemPath.IndexOf(itemBranches[j], StringComparison.InvariantCultureIgnoreCase);
									if (idx == 0) { found = true; break; }
								}
							
							if (!found)
								{
									/* yeah steve, '/EGS8.2' sucks now doesn't it... */
									string str = "/EGS/";
									
									/* the stupid branches are not case sensitive. */
									idx = itemPath.IndexOf(str, StringComparison.InvariantCultureIgnoreCase);
									
									if (idx > 0)
										{
											itemPath = itemPath.Substring(0,idx+str.Length);
#if DEBUG
											if (itemPath.IndexOf("$/IGT_0803/") == 0)
												{
#endif
													MegaHistory.logger.DebugFormat("branch={0}", itemPath);
													itemBranches.Add(itemPath);
#if DEBUG
												}
											else
												{
													Console.Error.WriteLine("'{0}' turned into '{1}'!", 
																									cs.Changes[i].Item.ServerItem,
																									itemPath);
												}
#endif
										}
								}
						}
				}
			
			return itemBranches;
		}
		
		/** this class is args for the threads. */
		private class _args
		{
			internal int changesLen;
			internal Change[] changes;
			internal int ptr;
			internal System.Threading.ReaderWriterLock rwlock;
			internal List<string> itemBranches;
		}
		
		/** this starts the worker threads and then waits for their results.
		 */
		private static List<string> _get_egs_branches_threaded(Changeset cs)
		{
			System.Threading.Thread[] threads = new System.Threading.Thread[8];
			_args args = new _args();
		
			args.itemBranches = new List<string>();
			args.rwlock = new System.Threading.ReaderWriterLock();
			args.changesLen = cs.Changes.Length;
			args.changes = cs.Changes;
		
			for(int i=0; i < threads.Length; ++i)
				{
					threads[i] = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(_egsbranches_worker));
					threads[i].Priority = System.Threading.ThreadPriority.Lowest;
					threads[i].Start(args);
				}
		
			for(int i=0; i < threads.Length; ++i) { threads[i].Join(); }
		
			return args.itemBranches;
		}
		
		private static void _egsbranches_worker(object o)
		{
			_args args = o as _args;
			bool done = false;
			
			int res = System.Threading.Interlocked.Increment(ref args.ptr);
			
			done = res >= args.changesLen;
			
			while(!done)
				{
					string itemPath = args.changes[res].Item.ServerItem;
					bool found = false;
					int idx = 0;
					int itemCount = 0;
				
					/* skip all non-merge changesets. */
					if (MegaHistory.IsChangeToConsider(args.changes[res]))
						{
							try {
								try {
									args.rwlock.AcquireReaderLock(10 * 1000); /* 10 second timeout. */
									itemCount = args.itemBranches.Count;
									for(int j=0; j < args.itemBranches.Count; ++j)
										{
											/* the stupid branches are not case sensitive. */
											idx = itemPath.IndexOf(args.itemBranches[j], 
																						 StringComparison.InvariantCultureIgnoreCase);
											if (idx == 0) { found = true; break; }
										}
								}
								finally
									{
										args.rwlock.ReleaseReaderLock();
									}
							} catch(ApplicationException) { /* we lost the lock. */ }
						}
				
					if (!found)
						{
							/* yeah steve, '/EGS8.2' sucks now doesn't it... */
							string str = "/EGS/";
						
							/* the stupid branches are not case sensitive. */
							idx = itemPath.IndexOf(str, StringComparison.InvariantCultureIgnoreCase);
						
							if (idx > 0)
								{
									itemPath = itemPath.Substring(0,idx+str.Length);
#if DEBUG
									if (itemPath.IndexOf("$/IGT_0803/") == 0)
										{
#endif
											try {
												try {
													bool reallyFound = false;
													args.rwlock.AcquireWriterLock(60 * 1000); /* 1 minute timeout. */
												
													if (itemCount != args.itemBranches.Count)
														{
															/* look again. */
															for(int j=0; j < args.itemBranches.Count; ++j)
																{
																	idx = itemPath.IndexOf(args.itemBranches[j], 
																												 StringComparison.InvariantCultureIgnoreCase);
																	if (idx == 0) { reallyFound = true; break; }
																}
														}
													
													if (! reallyFound) 
														{
															MegaHistory.logger.DebugFormat("branch={0}", itemPath);
															args.itemBranches.Add(itemPath); 
														}
												}
												finally { args.rwlock.ReleaseWriterLock(); }
											} catch(ApplicationException) { /* we lost the lock. */ }
#if DEBUG
										}
									else
										{
											Console.Error.WriteLine("'{0}' turned into '{1}'!", 
																							args.changes[res].Item.ServerItem,
																							itemPath);
										}
#endif
								}
						}
				
					/* setup for the next loop. */
					res = System.Threading.Interlocked.Increment(ref args.ptr);
					done = res >= args.changesLen;
				}
		}
	}
}
