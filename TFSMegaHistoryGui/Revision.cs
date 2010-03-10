
namespace tfs_fullhistory
{
	using ParentsCont = System.Collections.Generic.List<Revision>;
	using Changeset = Microsoft.TeamFoundation.VersionControl.Client.Changeset;
	
	internal class Revision : megahistorylib.Revision, System.Collections.Generic.IComparer<Revision>
	{
		private ParentsCont _parRevs = new ParentsCont();

		internal Revision(string branch, Changeset cs)
			: base(branch, cs)
		{
		}
		
		internal System.Collections.Generic.IEnumerable<Revision> getParents() { return _parRevs; }
		
		internal bool hasParents() { return _parents.Count > 0; }
		
		internal void addParent(Revision rev)
		{
			bool found = false;
			
			foreach(Revision r in _parRevs)
				{
					found = r.ID == rev.ID;
					if (! found) break;
				}
			if (!found) { _parRevs.Add(rev); }
		}

#region IComparer<Revision> Members

		public int Compare(Revision x, Revision y)
		{
			return x.ID.CompareTo(y.ID);
		}

#endregion
	}
}
