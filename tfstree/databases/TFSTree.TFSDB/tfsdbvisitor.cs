
using Microsoft.TeamFoundation.VersionControl.Client;

namespace TFSTree.Databases.TFSDB
{
	using BranchContainer = treelib.AVLTree<string, treelib.StringSorterInsensitive>;
	using BranchChangesets =
		treelib.AVLDict<string, treelib.AVLDict<string, Revision>, treelib.StringSorterInsensitive>;
	using RevisionIdx = treelib.AVLDict<string,Revision>;
	
	internal class TFSDBVisitor : RevisionRepoBase, megahistory.IVisitor<Revision>
	{
		protected System.Threading.Mutex _csMux = new System.Threading.Mutex();
		
		internal TFSDBVisitor() { }
		
		internal treelib.AVLDict<string, treelib.AVLDict<string, Revision>, treelib.StringSorterInsensitive>
		  getBranchChangesets() { return _branchChangesets; }
		
		internal string cleanseBranch(string branch)
		{
			lock(_branches)
				{
					BranchContainer.iterator it = _branches.find(branch);
					
					if (it != _branches.end())
						{
							/* if they're not equal, return the one in the list.
							 * otherwise, return the one they gave us.
							 */
							if (branch != it.item()) { branch = it.item(); }
						}
					else
						{
							/* i don't already know about this branch,
							 * so record it.
							 */
							_branches.insert(branch);
						}
				}
					
			return branch;
		}
		
		internal void primeBranches(BranchContainer.iterator beg, BranchContainer.iterator end)
		{ for(; beg != end; ++beg) { _branches.insert(beg.item()); } }
		
		public void addRevision(Revision rev)
		{
			_addRevision(rev);
		}
		
		public Revision visit(string branch, Changeset cs)
		{
			Revision rev = null;
			
			if (_csMux.WaitOne())
				{
					string id = TFSDB.MakeID(cs.ChangesetId);
					
					rev = base.rev(id);
					
					if (rev == null)
						{
							System.DateTime t;
							
							if (cs.CreationDate.Kind == System.DateTimeKind.Unspecified)
								{
									t = System.DateTime.SpecifyKind(cs.CreationDate, System.DateTimeKind.Local);
								}
							else { t = cs.CreationDate; }
							
							rev = new Revision(id, branch,
																 cs.Owner, cs.CreationDate,
																 cs.Comment);
							
							rev.Branch = megahistory.Utils.GetEGSBranch(branch) + "/EGS/";
							rev.Branch = cleanseBranch(rev.Branch);
							//logger.DebugFormat("{0}=>{1}", rev.Branch, cs.ChangesetId);
							
							_addRevision(rev);
						}
				}
			_csMux.ReleaseMutex();
			
			return rev;
		}
		
		public void addParent(Revision rev, int parentID)
		{
			rev.addParent(TFSDB.MakeID(parentID));
		}
		
		public bool visited(string branch, int csID)
		{
			Revision rev = null;
			
			if (_csMux.WaitOne())
				{
					string id = TFSDB.MakeID(csID);
					rev = this.rev(id);
				}
			_csMux.ReleaseMutex();
			
			return rev != null;
		}
		
		internal void save(TFSDB db)
		{
			for(BranchChangesets.iterator bit = _branchChangesets.begin();
					bit != _branchChangesets.end();
					++bit)
				{
					for(RevisionIdx.iterator rit = bit.value().begin();
							rit != bit.value().end();
							++rit)
						{
							TFSDB.logger.DebugFormat("s[{0}]", rit.value().ID);
							
							/* database. */
							db.save(rit.value());
						}
				}
		}
	}
}

