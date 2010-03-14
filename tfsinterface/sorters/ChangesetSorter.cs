
using Microsoft.TeamFoundation.VersionControl.Client;

namespace tfsinterface.sorters
{
	/// <summary>
	/// sort a changeset in ascending order.
	/// </summary>
	public class ChangesetSorter : System.Collections.Generic.IComparer<Changeset>
	{
#region IComparer<Changeset> Members
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public int Compare(Changeset x, Changeset y)
		{
			return x.ChangesetId.CompareTo(y.ChangesetId);
		}
#endregion
	}
	
	/// <summary>
	/// sort changesets in descending order
	/// </summary>
	public class ChangesetDescSorter : System.Collections.Generic.IComparer<Changeset>
	{
#region IComparer<Changeset> Members
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public int Compare(Changeset x, Changeset y)
		{
			return y.ChangesetId.CompareTo(x.ChangesetId);
		}
#endregion
	}
}
