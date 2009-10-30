/*
 * chiefengineer@neghvar
 * 2009-04-13 18:57:49 -0700
 */

using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;

namespace megahistory
{
	/** pulls a chain of changesets out of merges
	 */
	public class MegaHistory
	{
		public delegate bool ChangeTypeToConsiderDelegate(Change cng);

		/** what ChangeType(s) do we want to consider when we query for decomposition.
		 *  this defaults to a function which only considers:
		 *   changes which are not just ChangeType.Merge
		 *   and changes which contain a ChangeType.Merge
		 */
		public static ChangeTypeToConsiderDelegate IsChangeToConsider = _isChangeToConsider;

		private static bool _isChangeToConsider(Change cng)
		{
			/* look at only merge and branch changes, 
			 * but ignore only 'Merge' changes (these are TFS's way of syncing it's internal merge state)
			 */
			return (cng.ChangeType != ChangeType.Merge) &&
				(
				 //((cng.ChangeType & ChangeType.Branch) == ChangeType.Branch) ||
				 ((cng.ChangeType & ChangeType.Merge) == ChangeType.Merge)
				 );
		}

		static internal log4net.ILog logger = log4net.LogManager.GetLogger("megahistory_logger");
	
		public class Options
		{
			private bool _noRecurse = false;          /**< do we want recursion? */
			/** always decompose changesets 
			 *  (even if they didn't result in any branches)
			 */
			private bool _forceDecomposition = false;
		
			/** allow recursive queries to revisit already seen branches 
			 *  aka full recursion (decompose all merge changesets...)
			 *
			 *  so, you might think this option is broken, but it really isn't, 
			 *  see an explaination of how this does its stuff first.
			 */
			private bool _allowBranchRevisiting = false;
		
			public Options() { }
			public bool NoRecurse { get { return _noRecurse; } set { _noRecurse = value; } }
			public bool ForceDecomposition 
			{ get { return _forceDecomposition; } set { _forceDecomposition = value; } }
			public bool AllowBranchRevisiting
			{ get { return _allowBranchRevisiting; } set { _allowBranchRevisiting = value; } }
		}
	
		private Options _options;
		private VersionControlServer _vcs;
		private Visitor _visitor;
		private uint _queries = 0;
		private Timer _queryTimer = new Timer();

		public TimeSpan QueryTime { get { return _queryTimer.Total; } }
		public uint Queries { get { return _queries; } }
	
		public MegaHistory(VersionControlServer vcs, Visitor visitor)
		: this(new Options(), vcs, visitor) { }
	
		public MegaHistory(Options options, VersionControlServer vcs, Visitor visitor)
		{ 
			_options = options;
			_vcs=vcs;
			_visitor = visitor;
			
			if (! logger.Logger.Repository.Configured)
			{
				System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
				System.IO.FileStream fs;
				
				try{
					fs = new System.IO.FileStream(asm.Location+".config", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
					log4net.Config.XmlConfigurator.Configure(fs);
				} catch(Exception e)
				{
					int q = 1;
					q -= 12;
				}
				
				int j =0;
				j -= 45;
			}
		}
		
		public virtual bool visit(string srcPath, VersionSpec srcVersion, int maxChanges)
		{
			bool result = true;
			System.Collections.IEnumerable foo =
				_vcs.QueryHistory(srcPath, srcVersion, 0, RecursionType.Full,
													null, null, null, /* user, from ver, to ver */
													maxChanges, 
													true, false, false); 
			
			/* inc changes, slot mode, inc download info. */
			foreach (object o in foo)
				{
					Changeset cs = o as Changeset;
					bool isMerge = false;
					megahistory.Visitor.PatchInfo commit = null;
					
					result &= true;
					isMerge = Utils.IsMergeChangeset(cs);
					
					if (isMerge)
						{
							ChangesetVersionSpec ver = new ChangesetVersionSpec(cs.ChangesetId);
							
							visit(0, srcPath, ver, ver, ver);
						}
					else
						{
							_visitor.visit(0, cs);
						}
					
					_visitor.reset();
				}
			
			/* dump some stats out to the log file. */
			logger.DebugFormat("{0} queries took {1}", _queries, _queryTimer.Total);
			logger.DebugFormat("{0} findchangesetbranchcalls for {1} changesets.", 
												 Utils.FindChangesetBranchesCalls, _visitor.visitedCount());
			
			return result;
		}
		
		/** so, what this does is this:
		 *  uses VersionControlServer.QueryMerges to get a list of target and source changeset.
		 *  a private method trees that list into:
		 *   target changeset [ list o' source changests ]
		 *  we then march through that list
		 *   target(s) (here is where we visit the target)
		 *     source(s)
		 *  
		 *  if the source contains any TFS paths we want to query,
		 *   then we do a recursive query on that changeset (it is now the target)
		 *  otherwise we'll just visit the changeset
		 */
		public virtual bool visit(int parentID,
															string targetPath, 
															VersionSpec targetVer,
															VersionSpec fromVer,
															VersionSpec toVer)
		{ 
			bool result = _visit(parentID, null, 
													 null, null, 
													 targetPath, targetVer, 
													 fromVer, toVer, RecursionType.Full); 
		
			/* dump some stats out to the log file. */
			logger.DebugFormat("{0} queries took {1}", _queries, _queryTimer.Total);
			logger.DebugFormat("{0} findchangesetbranchcalls for {1} changesets.", 
												 Utils.FindChangesetBranchesCalls, _visitor.visitedCount());
		
			return result;
		}
	
		public virtual bool visit(string srcPath, VersionSpec srcVer, 
															string target, VersionSpec targetVer,
															VersionSpec fromVer, VersionSpec toVer,
															RecursionType recursionType)
		{
			bool result = _visit(0, null, 
													 srcPath, srcVer, 
													 target, targetVer, 
													 fromVer, toVer, recursionType);
		
			/* dump some stats out to the log file. */
			logger.DebugFormat("{0} queries took {1}", _queries, _queryTimer.Total);
			logger.DebugFormat("{0} findchangesetbranchcalls for {1} changesets.", 
												 Utils.FindChangesetBranchesCalls, _visitor.visitedCount());
			return result;
		}
	
		/** visit an explicit list of changesets. 
		 */
		private bool _visit(int parentID,
												List<string> targetBranches,
												string srcPath, VersionSpec srcVer, 
												string target, VersionSpec targetVer,
												VersionSpec fromVer, VersionSpec toVer,
												RecursionType recursionType)
		{
			logger.DebugFormat("{{_visit: parent={0}",parentID);
		
			/* so, here we might have a few top-level merge changesets. 
			 * the red-black binary tree sorts the changesets in decending order
			 */
			RBDictTree<int,SortedDictionary<int,ChangesetMerge>> merges = 
				query_merges(_vcs, srcPath, srcVer, target, targetVer, fromVer, toVer, recursionType);
		
			RBDictTree<int,SortedDictionary<int,ChangesetMerge>>.iterator it = merges.begin();
		
			/* walk through the merge changesets
			 * - this should return only one merge changeset for the recursive calls.
			 */
			for(; it != merges.end(); ++it)
				{
					int csID = it.value().first;
					try
						{
							/* visit the 'target' merge changeset here. 
							 * it's parent is the one passed in.
							 */
							Changeset cs = _vcs.GetChangeset(csID);
							string path_part = Utils.GetPathPart(target);
							ChangesetVersionSpec cstargetVer = targetVer as ChangesetVersionSpec;
						
							/* pass in the known set of branches in this changeset, or let it figure that out. */
							if (cstargetVer.ChangesetId == csID) { _visitor.visit(parentID, cs, targetBranches); }
							else { _visitor.visit(parentID, cs); }
						
							{
								Visitor.PatchInfo p = _visitor[csID];
								/* if the user chose to heed our warnings, 
								 *   and there are no branches to visit for this changeset?
								 *   move on to the next result, ignoring all the 'composites' of this 'fake' changeset.
								 */
								if (! _options.ForceDecomposition &&
										(p != null && (p.treeBranches == null || p.treeBranches.Count ==0)))
									{ continue; }
							}
						
							foreach(KeyValuePair<int,ChangesetMerge> cng in it.value().second)
								{
									ChangesetMerge csm = cng.Value;
									/* now visit each of the children.
									 * we've already expanded cs.ChangesetId (hopefully...)
									 */
									try
										{
											Changeset child = _vcs.GetChangeset(csm.SourceVersion);
											List<string> branches = null;
										
											{
												/* speed-up. if we already have the branches, don't do it again. 
												 * looking through 40K+ records takes some time...
												 */
												Visitor.PatchInfo p = _visitor[child.ChangesetId];
												if (p != null) { branches = p.treeBranches; }
												else { branches = Utils.FindChangesetBranches(child); }
											}
										
											/* - this is for the recursive query -
											 * you have to have specific branches here.
											 * a query of the entire project will probably not return.
											 *
											 * eg: 
											 * query target $/IGT_0803/main/EGS,78029 = 41s
											 * query target $/IGT_0803,78029 = DNF (waited 6+m)
											 */
										
											if (_options.NoRecurse)
												{
													/* they just want the top-level query. */
													_visitor.visit(cs.ChangesetId, child, branches);
												}
											else
												{
													if (!_visitor.visited(child.ChangesetId))
														{
															/* we just wanted to see the initial list, not a full tree of changes. */
															ChangesetVersionSpec tv = new ChangesetVersionSpec(child.ChangesetId);
															bool results = false;
														
															/* this is going to execute a number of queries to get
															 * all of the source changesets for this merge.
															 * since using a branch is an(several hundred) order(s) of magnitude faster, 
															 * we're doing this by branch(path)
															 */
															for(int i=0; i < branches.Count; ++i)
																{
																	if (_options.AllowBranchRevisiting || !_visitor.visited(branches[i]))
																		{
																			logger.DebugFormat("visiting ({0}) {1}{2}",
																												 child.ChangesetId, branches[i], path_part);
																		
																			/* this recurisve call needs to then 
																			 * handle visiting the results of this query. 
																			 */
																			bool branchResult = _visit(cs.ChangesetId, branches, 
																																 null, null,
																																 branches[i]+path_part, tv, 
																																 tv, tv, RecursionType.Full);
																		
																			if (branchResult) { results = true; }
																		}
																}
														
															if (!results)
																{
																	/* we got no results from our query, so display the changeset
																	 * (it won't be displayed otherwise)
																	 */
																	_visitor.visit(cs.ChangesetId, child, branches); //, branches);
																}
														}
													else
														{
															/* do we want to see it again? */
															_visitor.visit(cs.ChangesetId, child, branches);//, branches);
														}
												}
										}
									catch(Exception e)
									{
										string f = string.Format("{0}=>{1}", cs.ChangesetId, csm.SourceVersion);
										logger.Fatal(f, e);
										_visitor.visit(cs.ChangesetId, csm.SourceVersion, e);
									}
								}
						}
					catch(Exception e)
					{
						string f = string.Format("{0}=>{1}", parentID, csID);
						logger.Fatal(f, e);						
						_visitor.visit(parentID, csID, e);
					}
				}
		
			logger.DebugFormat("}}_visit:{0}", parentID);
		
			return false == merges.empty();
		}
		
		private RBDictTree<int,SortedDictionary<int,ChangesetMerge>> 
			query_merges(VersionControlServer vcs,
									 string srcPath, VersionSpec srcVer,
									 string targetPath, VersionSpec targetVer,
									 VersionSpec fromVer, VersionSpec toVer,
									 RecursionType recurType)
		{
			RBDictTree<int,SortedDictionary<int,ChangesetMerge>> merges = 
				new RBDictTree<int,SortedDictionary<int,ChangesetMerge>>();

			++_queries;
			logger.DebugFormat("query_merges {0}, {1}, {2}, {3}, {4}, {5}",
												 ( srcPath == null ? "(null)": srcPath), 
												 (srcVer == null ? "(null)" : srcVer.DisplayString),
												 targetPath, targetVer.DisplayString, 
												 (fromVer == null ? "(null)" : fromVer.DisplayString),
												 (toVer == null ? "(null)" : toVer.DisplayString));
		
			_queryTimer.start();
			try
				{
					ChangesetMerge[] mergesrc = vcs.QueryMerges(srcPath, srcVer, targetPath, targetVer,
																											fromVer, toVer, recurType);
					/* group by merged changesets. */
					for(int i=0; i < mergesrc.Length; ++i)
						{
							RBDictTree<int,SortedDictionary<int,ChangesetMerge>>.iterator it = 
								merges.find(mergesrc[i].TargetVersion);
						
							if (merges.end() == it)
								{
									/* create the list... */
									SortedDictionary<int,ChangesetMerge> group = new SortedDictionary<int,ChangesetMerge>();
									group.Add(mergesrc[i].SourceVersion, mergesrc[i]);
									merges.insert(mergesrc[i].TargetVersion, group);
								}
							else
								{ it.value().second.Add(mergesrc[i].SourceVersion, mergesrc[i]); }
						}
				}
			catch(Exception e)
				{
					logger.Fatal("querying failed", e);						
					
					//Console.Error.WriteLine("Error querying: {0},{1}", targetPath, targetVer);
					//Console.Error.WriteLine(e.ToString());
				}
			_queryTimer.stop();
		
			return merges;
		}
	
	}
}
