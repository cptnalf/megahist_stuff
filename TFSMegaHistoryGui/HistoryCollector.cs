using System;
using System.Collections.Generic;

namespace tfs_fullhistory
{
	using Changeset = Microsoft.TeamFoundation.VersionControl.Client.Changeset;
	using PrimaryIDsCont = treelib.AVLTree<int, megahistorylib.IntDescSorter>;
	using RevisionCont = treelib.AVLDict<int, Revision>;
	
	/* so, change the way this shit works...
	 * 
	 * 
	 */
	class HistoryCollector : megahistorylib.IMergeResults
	{
		private PrimaryIDsCont _primaryIDs = new PrimaryIDsCont();
		private RevisionCont _revisions = new RevisionCont();
		
		private System.ComponentModel.BackgroundWorker _worker = null;
		
		public System.ComponentModel.BackgroundWorker Worker
		{ get { return _worker; } set { _worker = value; } }
		
		public HistoryCollector() { }

		internal IEnumerable<Revision> primaryRevs()
		{
			for(PrimaryIDsCont.iterator it = _primaryIDs.begin();
			    it != _primaryIDs.end();
			    ++it)
				{
					RevisionCont.iterator rit = _revisions.find(it.item());
					if (rit != _revisions.end())
						{
							Revision rev = rit.value();
							yield return rev;
						}
				}
		}

#region IMergeResults Members

		public IEnumerable<int> getPrimaryID() { return _primaryIDs; }

		public treelib.support.iterator_base<int> primaryIDStart() { return _primaryIDs.begin(); }

		public treelib.support.iterator_base<int> primaryIDEnd() { return _primaryIDs.end(); }

		public megahistorylib.Revision getRevision(int csID)
		{
			Revision rev = null;
			RevisionCont.iterator it = _revisions.find(csID);
			if (it != _revisions.end()) { rev = it.value(); }
			
			return rev;
		}

		public megahistorylib.Revision construct(string branch, Changeset cs)
		{
			Revision rev = new Revision(cs, branch);
			return rev;
		}

		public bool visited(int csID)
		{
			RevisionCont.iterator it = _revisions.find(csID);
			return it != _revisions.end();
		}

		public void addPrimaryID(int csID)
		{
			PrimaryIDsCont.iterator it = _primaryIDs.find(csID);
			if (it != _primaryIDs.end()) { _primaryIDs.insert(csID); }
		}

#endregion
	}
}
