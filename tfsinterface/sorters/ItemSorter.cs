
using Microsoft.TeamFoundation.VersionControl.Client;

namespace tfsinterface.sorters
{
	/// <summary>
	/// standard sorter for Item's.
	/// </summary>
	public class ItemSorter : System.Collections.Generic.IComparer<Item>
	{
		/// <summary>
		/// 
		/// </summary>
		public ItemSorter() { }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="one"></param>
		/// <param name="two"></param>
		/// <returns></returns>
		public int Compare(Item one, Item two) { return one.ItemId.CompareTo(two.ItemId); }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="one"></param>
		/// <param name="two"></param>
		/// <returns></returns>
		public int Compare(object one, object two) { return this.Compare((Item) one, (Item)two); }
	}
}
