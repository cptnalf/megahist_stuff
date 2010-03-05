
using	Microsoft.TeamFoundation.VersionControl.Client;

namespace megahistorylib
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
		public static void VisualDiff(Item item1, int csID1, Item item2, int csID2)
		{
			VersionControlServer vcs = item1.VersionControlServer;
			IDiffItem left;
			IDiffItem right;
			
			left = new DiffItemVersionedFile(item1, 
																			 new ChangesetVersionSpec(csID1));
			right = new DiffItemVersionedFile(item2, 
																				new ChangesetVersionSpec(csID2));
			
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
		
		public static string GetEGSBranch(string fullPath)
		{
			string path = string.Empty;
			System.Text.RegularExpressions.Match match = _egsRE.Match(fullPath);
			
			if (match != null)
				{
					if (! string.IsNullOrEmpty(match.Groups[1].Value))
						{ path = match.Groups[1].Value; }
					else
						{
							
						}
				}
			
			return path;
		}
		
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
	}
}
