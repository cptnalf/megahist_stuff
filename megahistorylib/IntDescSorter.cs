
namespace megahistorylib
{
	/// <summary>
	/// 
	/// </summary>
	public class IntDescSorter : System.Collections.Generic.IComparer<int>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public int Compare(int x, int y) { return y.CompareTo(x); }
	}
}
