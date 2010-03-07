
using	Microsoft.TeamFoundation.VersionControl.Client;

namespace tfsinterface
{
	using ItemDict = treelib.TreapDict<int,Item>;
	
	/// <summary>
	/// 
	/// </summary>
	public static class SCMUtils
	{
		/// <summary>
		/// get the tfs server given by the name.
		/// </summary>
		/// <param name="serverName"></param>
		/// <returns></returns>
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
		
		/** retreive the branches of a particular path, at a particular version
		 *
		 *  @param vcs      version control server
		 *  @param srcPath  the path to find branches of
		 *  @param ver      version of the path
		 */
		public static ItemDict GetBranches(VersionControlServer vcs, 
																			 string srcPath, VersionSpec ver)
		{
			ItemDict tree = new ItemDict();
			
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
		
		private static void _walk_tree(BranchHistoryTreeItem item, ItemDict tree)
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
