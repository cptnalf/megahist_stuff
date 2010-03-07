
namespace megahistorylib
{
	/// <summary>
	/// a revision interface.
	/// </summary>
	public interface IRevision
	{
		/// <summary>
		/// add a parent changeset id to this revision.
		/// </summary>
		/// <param name="id"></param>
		void addParent(int id);
	}
}
