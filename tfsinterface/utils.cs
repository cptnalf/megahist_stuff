
using	Microsoft.TeamFoundation.VersionControl.Client;

namespace tfsinterface
{
	/// <summary>
	/// utils
	/// </summary>
	public static partial class Utils
	{
		/// <summary>
		/// difference a specific set o' stuffs
		/// </summary>
		/// <param name="item1"></param>
		/// <param name="csID1"></param>
		/// <param name="item2"></param>
		/// <param name="csID2"></param>
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
		
		/// <summary>
		/// try to diff what's on the harddrive, 
		/// then fall back to the latest version of the server items.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <param name="vcs"></param>
		public static void VisualDiff(string left, string right, VersionControlServer vcs)
		{
			Difference.VisualDiffFiles(vcs, left, null, right, null);
		}
		
		private static System.Text.RegularExpressions.Regex _egsRE = 
			new System.Text.RegularExpressions.Regex("(.+)/EGS/(.*)",
																 System.Text.RegularExpressions.RegexOptions.IgnoreCase |
																							 System.Text.RegularExpressions.RegexOptions.Compiled);
		
		/// <summary>
		/// get the branch part of the given path.
		/// </summary>
		/// <remarks>
		/// $/IGT_0803/development/dev_adv_cr/EGS/shared/lib/win32/PinUtil.dll
		/// 
		/// $/IGT_0803/development/dev_adv_cr/EGS/
		/// </remarks>
		/// <param name="fullPath"></param>
		/// <returns></returns>
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
		
		/// <summary>
		/// retrieve the base tfs path of the branch.
		/// </summary>
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
		
		/// <summary>
		/// is the changeset passed in a merge changeset?
		/// it looks at the changes contained in the changeset to make that determiniation
		/// </summary>
		/// <param name="cs"></param>
		/// <returns></returns>
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
