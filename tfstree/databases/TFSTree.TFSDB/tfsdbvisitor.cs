
using Microsoft.TeamFoundation.VersionControl.Client;

namespace TFSTree.Databases.TFSDB
{
	internal class TFSDBVisitor : RevisionRepoBase, megahistory.IVisitor<Revision>
	{
		protected System.Threading.Mutex _csMux = new System.Threading.Mutex();
		
		internal TFSDBVisitor() { }
		
		internal treelib.AVLDict<string, treelib.AVLDict<string, Revision>, treelib.StringSorterInsensitive>
		  getBranchChangesets() { return _branchChangesets; }
		
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
	}
}

