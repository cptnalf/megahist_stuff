
namespace StarTree.Plugin.Database
{
	using RevisionCont = treelib.AVLTree<Revision, RevisionSorterDesc>;
	using BranchContainer = treelib.AVLTree<string, treelib.StringSorterInsensitive>;
	using RevisionIdx = treelib.AVLDict<string, Revision>;
	using BranchChangesets =
		treelib.AVLDict<string, treelib.AVLDict<string,Revision>, treelib.StringSorterInsensitive>;
	
	/// <summary>
	/// base class for a repository
	/// this implements an in-mempry cache of the various import revision information bits
	///
	/// repository access is protected so that repo modification will work ok.
	/// preserve the state of 2 containers:
	///  _changesetIdx
	///  _branchChangesets
	///
	/// this assumes that you won't update the repo and retrieve sets for graphing
	/// at the same time.
	/// </summary>
	public class RevisionRepoBase
	{
		protected RevisionIdx _changesetIdx = new RevisionIdx();
		protected BranchContainer _branches = new BranchContainer();
		protected BranchChangesets _branchChangesets = new BranchChangesets();
		
		/// <summary>
		/// retrieves branches from the BranchContainer.
		/// </summary>
		public virtual System.Collections.Generic.IEnumerable<string> BranchNames
		{ get { return _branches; } }
		
		/// <summary>
		/// retrieves revisions from the RevisionIdx.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public virtual Revision rev(string id)
		{
			Revision r = null;
			RevisionIdx.iterator it = _changesetIdx.find(id);
			if (it != _changesetIdx.end()) { r = it.value(); }
			return r;
		}

		/// <summary>
		/// get revisions for a specific branch given the limit.
		/// </summary>
		/// <param name="branch"></param>
		/// <param name="limit"></param>
		/// <returns></returns>
		public virtual RevisionCont getBranch(string branch, ulong limit)
		{
			RevisionCont revisions = new RevisionCont();
			
			BranchChangesets.iterator it = _branchChangesets.find(branch);
			if (it != _branchChangesets.end())
				{
					ulong count = (it.value().size() > limit) ? limit : it.value().size();
					RevisionIdx.iterator csit = it.value().begin();

					for (ulong i = 0; i < count; ++i, ++csit)
						{
							if (csit == it.value().end()) { break; }
							revisions.insert(csit.value());
						}
				}
			
			return revisions;
		}
		
		/// <summary>
		/// add a revision.
		/// </summary>
		/// <param name="rev"></param>
		protected void _addRevision(Revision rev)
		{
			RevisionIdx.iterator csit = _changesetIdx.find(rev.ID);
			bool insertBranch = false;
					
			if (csit == _changesetIdx.end())
				{
					/* insert the changeset into the changeset index. */
					_changesetIdx.insert(rev.ID, rev);
					insertBranch = true;
				}
			else
				{
					/* so we found it... let's ensure it's the same. */
					if (csit.value().Branch != rev.Branch)
						{		insertBranch = true; }
				}

			if (insertBranch)
				{
					BranchChangesets.iterator bit = _branchChangesets.find(rev.Branch);

					/* now plop it into the branch-changesets index. */
					if (bit != _branchChangesets.end())
						{ bit.value().insert(rev.ID, rev); }
					else
						{
							RevisionIdx csidx = new RevisionIdx();
							csidx.insert(rev.ID, rev);
							_branchChangesets.insert(rev.Branch, csidx);
						}
				}
		}
	}
}
