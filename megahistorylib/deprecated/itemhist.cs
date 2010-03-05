

/* folder tracking should probably work excellently the way it is.
 * 
 * file-tracking probably sucks the way it currently is.
 * especially for deleted items, maybe renamed items, ...
 */

using System;
using Microsoft.TeamFoundation.VersionControl.Client;
using ItemTree = treelib.TreapDict<int,Microsoft.TeamFoundation.VersionControl.Client.Item>;
using MH = megahistory.deprecated.MegaHistory;

namespace megahistory.deprecated
{
	public class ItemHistory
	{
		private long _queries;
		private megahistorylib.Timer _queryTimer = new megahistorylib.Timer();
		private VersionControlServer _vcs;
		private ItemTree _tree;
		private Visitor _visitor;
		
		public ItemHistory(VersionControlServer vcs, Visitor visitor) 
		{
			_vcs = vcs;
			_visitor = visitor;
		}
		
		/**
		 *  @param srcPath    item path
		 *  @param srcVersion version
		 *  @param maxChanges maximum number of changes to query for.
		 */
		public virtual bool visit(string srcPath, VersionSpec srcVersion, int maxChanges)
		{
			bool result = true;
			
			_tree = megahistory.SCMUtils.GetBranches(_vcs, srcPath, srcVersion);
			
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
					
					result &= true;
					isMerge = megahistorylib.Utils.IsMergeChangeset(cs);
					
					if (isMerge)
						{
							visit(0, srcPath, cs.ChangesetId);
						}
					else
						{
							_visitor.visit(0, cs);
						}
					
					_visitor.reset();
				}
			
			/* dump some stats out to the log file. */
			MH.logger.DebugFormat("{0} queries took {1}", _queries, _queryTimer.Total);
			MH.logger.DebugFormat("{0} findchangesetbranchcalls for {1} changesets.", 
													 megahistorylib.Utils.FindChangesetBranchesCalls, _visitor.visitedCount());
			
			return result;
		}
		
		public bool visit(int parentID, string targetPath, int changesetID)
		{ return visit(parentID, targetPath, changesetID, 0); }
		
		/** visit the specified path.
		 */
		public bool visit(int parentID, string targetPath, int changesetID, int targetDelID)
		{
			bool result = false;
			string srcPath = null;
			VersionSpec srcVer = null;
			int srcDelID = 0;
			VersionSpec targetVer = new ChangesetVersionSpec(changesetID);
			VersionSpec fromVer = targetVer;
			VersionSpec toVer = targetVer;
			RecursionType recurType = RecursionType.None; /* assume these are all files */
			ChangesetMergeDetails mergedetails;
			
			if (_tree == null) { _tree = megahistory.SCMUtils.GetBranches(_vcs, targetPath, targetVer); }
			
			MH.logger.DebugFormat("visit {0} {1} {2} {3}",
														parentID, targetPath, targetVer.DisplayString);
			
			try {
				Changeset cs = _vcs.GetChangeset(changesetID);
				_visitor.visit(parentID, cs);
			}
			catch(System.Exception except) { MH.logger.ErrorFormat("visit {0}", except.ToString()); }
			
			mergedetails = 
				_vcs.QueryMergesWithDetails(srcPath, srcVer, srcDelID,
																		targetPath, targetVer, targetDelID, 
																		fromVer, toVer, recurType);
			++ _queries;
			
			
			result = (mergedetails != null) && (mergedetails.MergedItems.Length > 0);
			
			/* this should always be 1 when the targetpath is a file. */
			foreach(ItemMerge m in mergedetails.MergedItems)
				{
					MH.logger.DebugFormat("{0} {1} {2}", 
																m.SourceServerItem, m.SourceVersionFrom, m.TargetVersionFrom);
					
					Changeset cngs = _vcs.GetChangeset(m.SourceVersionFrom);
					int cl = cngs.Changes.Length;
				
					for(int i =0; i < cl; ++i)
						{
							if (((cngs.Changes[i].ChangeType & ChangeType.Merge) == ChangeType.Merge)
									|| (cngs.Changes[i].ChangeType & ChangeType.Branch) == ChangeType.Branch)
								{
									/* arg...
									 * probably want to do a branch history,
									 * then see if the item changed in this changeset is the item we're looking for.
									 */
									if (_tree.find(cngs.Changes[i].Item.ItemId) != _tree.end())
										{
											visit(m.TargetVersionFrom, 
														cngs.Changes[i].Item.ServerItem, 
														cngs.Changes[i].Item.ChangesetId,
														cngs.Changes[i].Item.DeletionId);
											break;
										}
								}
						}
				}
			
			return result;
		}
	}
}
