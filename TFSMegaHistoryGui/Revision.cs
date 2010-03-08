
namespace tfs_fullhistory
{
	using ParentsCont = System.Collections.Generic.List<Revision>;
	using Changeset = Microsoft.TeamFoundation.VersionControl.Client.Changeset;
	
	internal class Revision : megahistorylib.Revision, System.Collections.Generic.IComparer<Revision>
	{
		private ParentsCont _parents = new ParentsCont();

		internal Revision(Changeset cs, string branch)
			: base(cs, branch)
		{
		}
		
		internal System.Collections.Generic.IEnumerable<Revision> Parents
		{ get { return _parents; } }
		
		internal bool hasParents() { return _parents.Count > 0; }
		
		internal void addParent(Revision rev)
		{
			bool found = false;
			
			foreach(Revision r in _parents)
				{
					found = r.ID == rev.ID;
					if (! found) break;
				}
			if (!found) { _parents.Add(rev); }
		}

#region IComparer<Revision> Members

		public int Compare(Revision x, Revision y)
		{
			return x.ID.CompareTo(y.ID);
		}

#endregion
	}
}
