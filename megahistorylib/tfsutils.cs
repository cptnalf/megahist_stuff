
using	Microsoft.TeamFoundation.VersionControl.Client;

namespace megahistory
{
	public static partial class Utils
	{
		/** get a tfs server. */
		public static VersionControlServer GetTFSServer(string serverName)
		{
			Microsoft.TeamFoundation.Client.TeamFoundationServer srvr;
			
			if (serverName != null && serverName != string.Empty)
				{
					srvr = Microsoft.TeamFoundation.Client.TeamFoundationServerFactory.GetServer(serverName);
				}
			else
				{
					/* hmm, they didn't specify one, so get the first in the list. */
					Microsoft.TeamFoundation.Client.TeamFoundationServer[] servers =
						Microsoft.TeamFoundation.Client.RegisteredServers.GetServers();
					
					srvr = servers[0];
				}
			
			return (srvr.GetService(typeof(VersionControlServer)) as VersionControlServer);
		}
		
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
		
		/** try to diff what's on the harddrive, 
		 *  then fall back to the latest version of the server items.
		 */
		public static void VisualDiff(string left, string right, VersionControlServer vcs)
		{
			Difference.VisualDiffFiles(vcs, left, null, right, null);
		}
		
		private static System.Text.RegularExpressions.Regex _egsRE = 
			new System.Text.RegularExpressions.Regex("(.+)/EGS/(.*)",
																 System.Text.RegularExpressions.RegexOptions.IgnoreCase |
																							 System.Text.RegularExpressions.RegexOptions.Compiled);
		
		/** retrieve the base tfs path of the branch.
		 */
		public static string GetPathPart(string path)
		{
			string path_part = string.Empty;
			System.Text.RegularExpressions.Match match = _egsRE.Match(path);
			
			if (match != null)
				{
					path_part = match.Groups[2].Value;
				}
			else
				{
					int idx = path.LastIndexOf("/EGS");
					if (idx >=0)
						{
							path_part = path.Substring(idx + 4);
						}
				}
			
			return path_part;
		}
		
		public static bool IsMergeChangeset(Changeset cs)
		{
			bool isMerge = false;
			int changesCount = cs.Changes.Length;
			
			for(int i=0; i < changesCount; ++i)
				{
					Change cng = cs.Changes[i];
					
					isMerge =
						((cng.ChangeType & ChangeType.Branch) == ChangeType.Branch
						 ||
						 (cng.ChangeType & ChangeType.Merge) == ChangeType.Merge);
					if (isMerge) { break; }
				}
			
			return isMerge;
		}
		
		/** retreive the branches of a particular path, at a particular version
		 *
		 *  @param vcs      version control server
		 *  @param srcPath  the path to find branches of
		 *  @param ver      version of the path
		 */
		public static treelib.TreapDict<int,Microsoft.TeamFoundation.VersionControl.Client.Item> GetBranches(VersionControlServer vcs, 
																																																					string srcPath, VersionSpec ver)
		{
			treelib.TreapDict<int,Microsoft.TeamFoundation.VersionControl.Client.Item> tree = new treelib.TreapDict<int,Microsoft.TeamFoundation.VersionControl.Client.Item>();
			
			ItemSpec itm = new ItemSpec(srcPath, RecursionType.None);
			BranchHistoryTreeItem[][] branches = 
				vcs.GetBranchHistory(new ItemSpec[] { itm }, ver);
			
			for(int i=0; i < branches.Length; ++i)
				{
					for(int j=0; j < branches[i].Length; ++j)
						{
							_walk_tree(branches[i][j], tree);
						}
				}
			
			return tree;
		}
		
		private static void _walk_tree(BranchHistoryTreeItem item, treelib.TreapDict<int,Microsoft.TeamFoundation.VersionControl.Client.Item> tree)
		{
			/* bail early. */
			if (item == null) { return; }
			
			if (item.Relative != null
					&& item.Relative.BranchToItem != null)
				{
					Item tmp = item.Relative.BranchToItem;
					if (tree.find(tmp.ItemId) == tree.end())
						{ tree.insert(tmp.ItemId, tmp); }
					
					foreach(BranchHistoryTreeItem ti in item.Children)
						{ _walk_tree(ti, tree); }
				}
		}

	}
}
