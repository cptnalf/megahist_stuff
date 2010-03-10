
using System.Collections.Generic;

namespace megahistorylib
{
	using Changeset = Microsoft.TeamFoundation.VersionControl.Client.Changeset;
	using Change = Microsoft.TeamFoundation.VersionControl.Client.Change;
	
	/// <summary>
	/// a revision interface.
	/// </summary>
	public class Revision
	{
		private Changeset _cs;
		private string _branch;
		private int _cngsCnt = -1;
		
		/// <summary>
		/// parents.
		/// </summary>
		protected List<int> _parents = new List<int>();
		
		/// <summary>
		/// 
		/// </summary>
		public System.Collections.Generic.IEnumerable<int> Parents
		{ get { return _parents; } }
		
		/// <summary>the branch the changeset belongs to</summary>
		public string Branch { get { return _branch; } }
		
		/// <summary>the original changeset</summary>
		public Changeset cs { get { return _cs; } }
		
		/// <summary>the id</summary>
		public int ID { get { return _cs.ChangesetId; } }

		/// <summary>who checked in the changeset</summary>
		public string User { get { return _cs.Owner; } }
		
		/// <summary>when the changeset was created</summary>
		public System.DateTime CreationDate { get { return _cs.CreationDate; } }
		
		/// <summary></summary>
		public string Comment { get { return _cs.Comment; } }
		
		/// <summary>
		/// number of changes
		/// </summary>
		public int ChangesCount
		{
			get
				{
					if (_cngsCnt < 0) { _cngsCnt = _cs.Changes.Length; }
					return _cngsCnt;
				}
		}
		
		/// <summary>
		/// 
		/// </summary>
		public Change[] Changes { get { return _cs.Changes; } }
		
		/// <summary>
		/// </summary>
		public Revision(string branch, Changeset cs)
		{
			_cs = cs;
			_branch = branch;
		}
		
		/// <summary>
		/// add a parent of this changeset.
		/// </summary>
		public void addParent(int parentID)
		{
			bool found = false;
			foreach(int p in _parents)
				{
					found = p == parentID;
					if (found) { break; }
				}
			
			if (!found) { _parents.Add(parentID); }
		}
	}
}
