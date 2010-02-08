
/*
 
 */

using System.Collections.Generic;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace TFSTree
{
	using IEnumerable = System.Collections.IEnumerable;
	
	internal class TFSTreeDB : IRevisionDB
	{
		private VersionControlServer _vcs;
		private treelib.
		
		internal TFSTreeDB()
		{
			
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
					isMerge = megahistory.Utils.IsMergeChangeset(cs);
					
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
		}
		
		public List<string> GetBranches()
		{
		}
		
		public Dictionary<string, Revision> GetLatestRevisionIDs(string branch, int limit)
		{
			
		}
		
		public Revision GetRevision(string id)
		{
		}

	}
	
}