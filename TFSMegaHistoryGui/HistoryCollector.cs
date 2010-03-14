using System;
using System.Collections.Generic;

namespace tfs_fullhistory
{
	using Changeset = Microsoft.TeamFoundation.VersionControl.Client.Changeset;
	using PrimaryIDsCont = treelib.AVLTree<int, treelib.sorters.IntDesc>;
	using BranchCont = treelib.AVLTree<string, treelib.sorters.StringInsensitive>;
	using RevisionCont = treelib.AVLDict<int, Revision>;
	
 	class HistoryCollector : megahistorylib.MergeResults
	{
		private BranchCont _branches = new BranchCont();
		private System.ComponentModel.BackgroundWorker _worker = null;
		
		private IEnumerable<Revision> _primaryRevs(int csID)
		{
			Queue<Revision> revs = new Queue<Revision>();
			Revision rev = this.getRevision(csID) as Revision;
			
			while(rev != null)
				{
					yield return rev;
					
					/* now walk through the parents of each revision in this branch
					 * and dump them all.
					 */
					foreach(Revision pr in rev.getParents())
						{ if (rev.Branch == pr.Branch) { revs.Enqueue(pr); } }
					
					if (revs.Count > 0) { rev = revs.Dequeue(); }
					else { rev = null; }
				}
		}
		
		/// <summary>
		/// this copies over the parent revisions we have to the 
		/// collection of 'Revision' objects. 
		/// this way that work isn't done on the fly.
		/// </summary>
		private void _fixRevs(Revision r)
		{
			if (r != null)
				{
					foreach(int pID in r.Parents)
						{
							Revision pr = this.getRevision(pID) as Revision;
							if (pr != null)
								{
									_fixRevs(pr);
									r.addParent(pr);
								}
						}
				}
		}
		
		protected override megahistorylib.Revision _construct(string branch, Changeset cs)
		{
			Revision rev = new Revision(branch, cs);
			
			/* want to have a single list of branches, this ensures that. */
			lock(_branches)
				{
					branch = tfsinterface.Utils.GetEGSBranch(branch);
					BranchCont.iterator it = _branches.find(branch);
					
					if (it != _branches.end())
						{
							/* see if the branches are really exactly the same */
							if (it.item() != branch) { branch = it.item(); }
						}
					else { _branches.insert(branch); }
				}
			
			return rev;
		}
		
		public System.ComponentModel.BackgroundWorker Worker
		{ get { return _worker; } set { _worker = value; } }
		
		public HistoryCollector() { }
				
		public override megahistorylib.Revision construct(string branch, Changeset cs)
		{
			return base.construct(branch, cs);
		}

		internal IEnumerable<Revision> primaryRevs()
		{ foreach(Revision r in _primaryRevs(this.firstID)) { yield return r; } }
		
		internal void fixRevs()
		{
			Revision r = this.getRevision(this.firstID) as Revision;
			if (r != null)
				{
					_fixRevs(r);
				}
		}
	}
}
