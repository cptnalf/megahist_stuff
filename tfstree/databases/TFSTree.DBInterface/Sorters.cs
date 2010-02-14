
using System.Collections.Generic;

namespace TFSTree.Databases
{
	public class RevisionSorterDesc : IComparer<Revision>
	{
		public RevisionSorterDesc() { }
		
		/* legend for an ascending sorter:
		 * -1 => one > two
		 *  0 => one == two
		 *  1 => one < two
		 *
		 * non-null is always greater than null.
		 * decending returns the opposite numbers to force 
		 * the sort to go in the other direction.
		 */
		public int Compare(Revision one, Revision two)
		{
			int result = 1;
			object obj1 = one;
			object obj2 = two;
			
			if (obj1 == null) { result = (obj2 == null) ? 0 : -1; }
			else
				{
					if (obj2 == null) { result = (obj1 == null) ? 0 : 1; }
					else
						{
							/* so neither is null... */
							result = two.CompareTo(one);
						}
				}
			
			return result;
		}
	}
}
