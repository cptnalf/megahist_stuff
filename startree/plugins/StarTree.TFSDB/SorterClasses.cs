
using System.Collections.Generic;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace StarTree.Plugin.TFS
{
	internal class ChangesetDescSorter : IComparer<Changeset>
	{
		public ChangesetDescSorter() { }

		public int Compare(Changeset c1, Changeset c2)
		{
			int res = 0;

			if (c1 == null || c2 == null)
				{
					/* figure out which one... */
					if (c1 == null) { res = -1; }
					else { res = 1; }
				}
			else { res = c2.ChangesetId.CompareTo(c1.ChangesetId); }

			return res;
		}
	}
}
