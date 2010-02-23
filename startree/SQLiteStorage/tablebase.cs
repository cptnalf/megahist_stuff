
using System.Data.SQLite;

namespace SQLiteStorage
{
	using Revision = StarTree.Plugin.Database.Revision;
	using IEnumerable = System.Collections.IEnumerable;
	using IEnumerator = System.Collections.IEnumerator;
	using DbType = System.Data.DbType;
	
	/* would store revisions, not parents though. 
	 * if an entry is in here, the revision has been fully queried for merge parts.
	 * 
	 * this would store all but the 'parents' part of the Revision object.
	 */
	public class TableBase
	{
		public virtual string ConnectionString { get; set; }
		
		protected SQLiteCommand _getCmd(string text)
		{
			SQLiteConnection conn = new SQLiteConnection(ConnectionString);
			
			conn.Open();
			SQLiteCommand cmd = conn.CreateCommand();
			cmd.CommandType = System.Data.CommandType.Text;
			cmd.CommandText = text;
			
			return cmd;
		}
	}	
}
