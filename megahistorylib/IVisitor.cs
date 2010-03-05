
using Microsoft.TeamFoundation.VersionControl.Client;

namespace megahistorylib
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IVisitor<T>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="branch"></param>
		/// <param name="cs"></param>
		/// <returns></returns>
		T visit(string branch, Changeset cs);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <param name="parentID"></param>
		void addParent(T data, int parentID);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="branch"></param>
		/// <param name="csID"></param>
		/// <returns></returns>
		bool visited(string branch, int csID);
	}
}
