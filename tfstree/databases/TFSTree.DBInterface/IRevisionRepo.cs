
namespace TFSTree.Databases
{
	using RevisionCont = treelib.AVLTree<Revision,RevisionSorterDesc>;
	/// <summary>
	/// progress arg class
	/// </summary>
	public class ProgressArgs : System.EventArgs
	{
		/// <summary>
		/// change in your progress.
		/// </summary>
		public int Delta { get; set; }
		
		/// <summary>
		/// data to go along with your progress
		/// </summary>
		public object Data { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="delta"></param>
		/// <param name="obj"></param>
		public ProgressArgs(int delta, object obj)
		{
			this.Delta = delta;
			this.Data = obj;
		}
	}
	
	/// <summary>
	/// database interface
	/// </summary>
	public interface IRevisionRepo
	{
		/// <summary>
		/// name
		/// </summary>
		string Name { get; }
		
		/// <summary>
		/// the branches this database has access to.
		/// </summary>
		System.Collections.Generic.IEnumerable<string> BranchNames { get; }
		
		/// <summary>
		/// retrieve one revision.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Revision rev(string id);
		
		/// <summary>
		/// retrieve a branch's revisions.
		/// </summary>
		/// <param name="branch"></param>
		/// <param name="limit"></param>
		/// <returns></returns>
		RevisionCont getBranch(string branch, ulong limit);
		
		/// <summary>
		/// what to do to report some progress
		/// when doing something which is taking a long time.
		/// </summary>
		event System.EventHandler<ProgressArgs> OnProgress;
	}
}
