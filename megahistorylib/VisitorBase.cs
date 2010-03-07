
using Microsoft.TeamFoundation.VersionControl.Client;

namespace megahistorylib
{
	using PrimaryIDsCont = treelib.AVLTree<int, IntDescSorter>;
	using ChangesetCont = treelib.AVLDict<int, CSWrapper>;
	
	/// <summary>
	/// 
	/// </summary>
	public class VisitorBase : IVisitor
	{
		/// <summary>
		/// the list of changeset ids which were asked about.
		/// </summary>
		protected PrimaryIDsCont _primaryIDs = new PrimaryIDsCont();
		
		/// <summary>
		/// container with all visited changesets.
		/// </summary>
		protected ChangesetCont _cache = new ChangesetCont();
		
		/// <summary>
		/// 
		/// </summary>
		public VisitorBase() { }
		
		/// <summary>
		/// find a changeset
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public virtual CSWrapper find(int id)
		{
			CSWrapper w = null;
			ChangesetCont.iterator it = _cache.find(id);
			if (it != _cache.end()) { w = it.value(); }
			
			return w;
		}
		
		/// <summary>
		/// create a changeset or visit one.
		/// </summary>
		/// <param name="branch"></param>
		/// <param name="cs"></param>
		/// <returns></returns>
		public virtual IRevision construct(string branch, Changeset cs)
		{
			CSWrapper w;
			ChangesetCont.iterator it = _cache.find(cs.ChangesetId);
			if (it != _cache.end()) { w = it.value(); }
			else { w = new CSWrapper(cs, branch); }
			
			return w;
		}
		
		/// <summary>
		/// add a parent to the given changeset.
		/// </summary>
		/// <param name="csID"></param>
		/// <param name="parentID"></param>
		public virtual void addParent(int csID, int parentID)
		{
			CSWrapper w = this.find(csID);
			w.addParent(parentID);
		}
		
		/// <summary>
		/// have we visited this changeset already?
		/// </summary>
		/// <param name="branch"></param>
		/// <param name="csID"></param>
		/// <returns></returns>
		public virtual bool visited(string branch, int csID)
		{
			CSWrapper w = this.find(csID);
			
			return w != null;
		}
		
		/// <summary>
		/// add the changeset id to the list of primary ids.
		/// </summary>
		/// <param name="csID"></param>
		public virtual void addPrimaryID(int csID)
		{
			PrimaryIDsCont.iterator it = _primaryIDs.find(csID);
			
			if (it == _primaryIDs.end()) { _primaryIDs.insert(csID); }
		}
	}
}
