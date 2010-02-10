
namespace TFSTree.Databases
{
	using RevisionDict = System.Collections.Generic.Dictionary<string,Revision>;
	using BranchContainer = treelib.AVLTree<string, treelib.StringSorterInsensitive>;
	using RevisionIdx = treelib.AVLDict<int, Revision, treelib.IntSorterDesc>;
	using BranchChangesets =
		treelib.AVLDict<string, treelib.AVLDict<int, Revision, treelib.IntSorterDesc>, treelib.StringSorterInsensitive>;

	public class RevisionRepoBase
	{
		protected RevisionIdx _changesetIdx = new RevisionIdx();
		protected BranchContainer _branches = new BranchContainer();
		protected BranchChangesets _branchChangesets = new BranchChangesets();
		
		public System.Collections.Generic.IEnumerable<string> BranchNames
		{ get { return _branches; } }
		
		public Revision rev(string id)
		{
			Revision r = null;
			int csid = System.Int32.Parse(id);
			RevisionIdx.iterator it = _changesetIdx.find(csid);
			if (it != _changesetIdx.end()) { r = it.value(); }
			return r;
		}
		
		protected void _addRevision(string branch, int csID, Revision rev)
		{
			RevisionIdx.iterator csit = _changesetIdx.find(csID);
			
			if (csit == _changesetIdx.end())
				{
					BranchChangesets.iterator bit = _branchChangesets.find(rev.Branch);
					
					/* insert the changeset into the changeset index. */
					_changesetIdx.insert(csID, rev);
					
					/* now plop it into the branch-changesets index. */
					if (bit != _branchChangesets.end())
						{
							bit.value().insert(csID, rev);
						}
					else
						{
							RevisionIdx csidx = new RevisionIdx();
							csidx.insert(csID, rev);
							_branchChangesets.insert(rev.Branch, csidx);
						}
				}
		}
		protected RevisionDict _getRevisions(string branch, ulong limit)
		{
			RevisionDict revisions = new RevisionDict();
			
			BranchChangesets.iterator it = _branchChangesets.find(branch);
			if (it != _branchChangesets.end())
				{
					ulong count = (it.value().size() > limit) ? limit : it.value().size();
					RevisionIdx.iterator csit = it.value().begin();
					
					for(ulong i=0; i < count; ++i, ++csit)
						{
							if (csit == it.value().end()) { break; }
							revisions.Add(csit.value().ID, csit.value());
						}
				}
			
			return revisions;
		}
	}
}
