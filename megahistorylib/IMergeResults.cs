
using Microsoft.TeamFoundation.VersionControl.Client;

namespace megahistorylib
{
	/// <summary>
	/// reuslts.
	/// </summary>
	public interface IMergeResults
	{
		/// <summary>
		/// the biggest ID in the primary search branch.
		/// this is where consumers should start their history traversal.
		/// </summary>
		int firstID { get; }

		/// <summary>
		/// an enumeration of ids.
		/// </summary>
		/// <returns></returns>
		System.Collections.Generic.IEnumerable<int> getPrimaryIDs();
		
		/// <summary>
		/// the start of the primary history.
		/// </summary>
		/// <returns></returns>
		treelib.support.iterator_base<int> primaryIDStart();
		
		/// <summary>
		/// the end of the primary history
		/// </summary>
		/// <returns></returns>
		treelib.support.iterator_base<int> primaryIDEnd();
		
		/// <summary>
		/// get a specific revision from the resultset.
		/// </summary>
		/// <param name="csID"></param>
		/// <returns></returns>
		Revision getRevision(int csID);
		
		/// <summary>
		/// construct a revision from the given branch and changeset.
		/// </summary>
		/// <param name="branch"></param>
		/// <param name="cs"></param>
		/// <returns></returns>
		Revision construct(string branch, Changeset cs);
		
		//void addParent(Revision revision, int parentID);
		
		/// <summary>
		/// have we already seen this changeset id already?
		/// </summary>
		/// <param name="csID"></param>
		/// <returns></returns>
		bool visited(int csID);
		
		/// <summary>
		/// add a primary history changeset id.
		/// </summary>
		/// <param name="csID"></param>
		void addPrimaryID(int csID);
		
		/// <summary>
		/// set the first changeset id.
		/// </summary>
		void setFirstID(int csID);
	}
}
