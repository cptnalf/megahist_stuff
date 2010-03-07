
using Microsoft.TeamFoundation.VersionControl.Client;

namespace megahistorylib
{
	/// <summary>
	/// a visitor item.
	/// </summary>
	public interface IVisitor
	{
		/// <summary>
		/// visit a changeset,
		/// create an object from the data.
		/// </summary>
		/// <param name="branch"></param>
		/// <param name="cs"></param>
		/// <returns></returns>
		IRevision construct(string branch, Changeset cs);
		
		/// <summary>
		/// add a parent to the provided changeset
		/// </summary>
		/// <param name="csID"></param>
		/// <param name="parentID"></param>
		void addParent(int csID, int parentID);
		
		/// <summary>
		/// have we already seen this changeset?
		/// </summary>
		/// <param name="branch"></param>
		/// <param name="csID"></param>
		/// <returns></returns>
		bool visited(string branch, int csID);
		
		/// <summary>
		/// add the first order changesets
		/// </summary>
		/// <param name="csID"></param>
		void addPrimaryID(int csID);
	}
}
