
using Microsoft.TeamFoundation.VersionControl.Client;

namespace megahistorylib
{
	using RevisionCont = treelib.AVLDict<int,Revision>;
	using PrimaryIDCont = treelib.AVLTree<int, IntDescSorter>;
	
	/// <summary>
	/// default results id.
	/// </summary>
	public class MergeResults : IMergeResults
	{
		/// <summary>
		/// mutex for threaded inserts/gets
		/// </summary>
		protected System.Threading.Mutex _csMux = new System.Threading.Mutex();
		
		/// <summary>
		/// cache of revisions.
		/// </summary>
		protected RevisionCont _revisions = new RevisionCont();
		
		/// <summary>
		/// primary changeset ids history 
		/// </summary>
		protected PrimaryIDCont _primaryIDs = new PrimaryIDCont();
		
		/// <summary>
		/// the biggest ID in the primary search branch.
		/// </summary>
		protected int _firstID = -1;

		/// <summary>
		/// build a revision.
		/// </summary>
		/// <param name="branch"></param>
		/// <param name="cs"></param>
		/// <returns></returns>
		protected virtual Revision _construct(string branch, Changeset cs)
		{
			branch = tfsinterface.Utils.GetEGSBranch(branch);
			return new Revision(branch, cs); 
		}
		
		/// <summary>
		/// add the revision to the internal cache.
		/// </summary>
		/// <param name="rev"></param>
		protected void _add(Revision rev)
		{
			RevisionCont.iterator it = _revisions.find(rev.ID);
			if (it == _revisions.end())
				{ _revisions.insert(rev.ID, rev); }
		}
		
		/// <summary>
		/// constructor
		/// </summary>
		public MergeResults() { }
		
		/// <summary>
		///
		/// </summary>
		public virtual int firstID { get { return _firstID; } }
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual System.Collections.Generic.IEnumerable<int> getPrimaryIDs()
		{ return _primaryIDs; }
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual treelib.support.iterator_base<int> primaryIDStart()
		{ return _primaryIDs.begin(); }
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual treelib.support.iterator_base<int> primaryIDEnd()
		{ return _primaryIDs.end(); }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="csID"></param>
		/// <returns></returns>
		public virtual Revision getRevision(int csID)
		{
			Revision rev = null;
			RevisionCont.iterator it = _revisions.find(csID);
			if (it != _revisions.end()) { rev = it.value(); }
			
			return rev;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="branch"></param>
		/// <param name="cs"></param>
		/// <returns></returns>
		public virtual Revision construct(string branch, Changeset cs)
		{
			Revision rev = null;
			
			if (_csMux.WaitOne())
				{
					rev = getRevision(cs.ChangesetId);
					if (rev == null)
						{
							rev = _construct(branch, cs);
							
							_add(rev);
						}
				}
			_csMux.ReleaseMutex();
			
			return rev;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="csID"></param>
		/// <returns></returns>
		public virtual bool visited(int csID)
		{
			Revision rev = null;
			
			if (_csMux.WaitOne())
				{
					rev = getRevision(csID);
				}
			_csMux.ReleaseMutex();
			
			return rev != null;
		}
		
		/// <summary>
		/// </summary>
		/// <param name="csID"></param>
		public virtual void setFirstID(int csID) { _firstID = csID; }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="csID"></param>
		public virtual void addPrimaryID(int csID)
		{
			PrimaryIDCont.iterator it = _primaryIDs.find(csID);
			
			if (it == _primaryIDs.end()) { _primaryIDs.insert(csID); }
		}
	}
}
