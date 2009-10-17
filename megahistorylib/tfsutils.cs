
using	Microsoft.TeamFoundation.VersionControl.Client;

namespace megahistory
{
	public static class Utils
	{
		/** difference a specific set o' stuffs
		 */
		public static void VisualDiff(Visitor.PatchInfo p1, Visitor.PatchInfo p2, pair<int,int> items)
		{
			VersionControlServer vcs = p1.cs.Changes[items.first].Item.VersionControlServer;
			IDiffItem left;
			IDiffItem right;
			
			left = new DiffItemVersionedFile(p1.cs.Changes[items.first].Item, 
																			 new ChangesetVersionSpec(p1.cs.ChangesetId));
			right = new DiffItemVersionedFile(p2.cs.Changes[items.second].Item, 
																				new ChangesetVersionSpec(p2.cs.ChangesetId));
			
			Difference.VisualDiffItems(vcs, left, right);
		}
		
		/** try to diff what's on the harddrive, then fall back to the latest version of the server items.
		 */
		public static void VisualDiff(string left, string right, VersionControlServer vcs)
		{
			Workspace[] workspaces = vcs.QueryWorkspaces(null, 
																									 System.Environment.UserName, 
																									 System.Environment.MachineName);
			
			if (workspaces != null)
				{
					WorkingFolder f = null;
					
					foreach(Workspace w in workspaces)
						{
							f = w.TryGetWorkingFolderForServerItem(left);
						}
				}
			
			Difference.VisualDiffFiles(vcs, left, null, right, null);
		}

		
		private static string 
	}
}
