
namespace megahistorylib
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="F"></typeparam>
	/// <typeparam name="S"></typeparam>
	public class pair<F,S> : System.IComparable<pair<F,S>> where F : System.IComparable<F>
	{
		/// <summary>
		/// 
		/// </summary>
		public F first;
		/// <summary>
		/// 
		/// </summary>
		public S second;
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="f"></param>
		/// <param name="s"></param>
		public pair(F f, S s) { first = f; second = s; }
		
		private int _cmp(pair<F,S> two)
		{
			int result = -1;
			object o = (object)two;
			
			if (o != null) { result = first.CompareTo(two.first); }
			
			return result;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public int CompareTo(pair<F,S> obj) { return _cmp(obj); }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		int System.IComparable<pair<F,S>>.CompareTo(pair<F,S> obj) { return _cmp(obj); }
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode() { return ((object)first).GetHashCode(); }
	}
}
