
using System.Data.SQLite;

namespace SQLiteStorage
{
	using Revision = StarTree.Plugin.Database.Revision;
	using DbType = System.Data.DbType;
	
	/// <summary>
	/// the parents of a revision.
	/// </summary>
	public class ParentsTable : TableBase
	{
		public void create()
		{
			string createTable = @"CREATE TABLE IF NOT EXISTS parents
(child INTEGER, parent INTEGER)";
			string createIdx = @"CREATE INDEX IF NOT EXISTS parents_child
(child)";
			SQLiteCommand cmd = null;
			try
				{
					cmd = _getCmd(createTable);
					cmd.ExecuteNonQuery();
					cmd.Dispose();
					cmd = null;
					
					cmd = _getCmd(createIdx);
					cmd.ExecuteNonQuery();
					cmd.Dispose();
					cmd = null;
				}
			catch(System.Exception) { }
			
			if (cmd != null) 
				{
					cmd.Dispose();
					cmd = null;
				}
		}
		
		public void load(ref Revision rev)
		{
			using (SQLiteCommand cmd =
						 _getCmd(
@"SELECT child, parent 
  FROM parents
  WHERE child = @child
  ORDER BY parent"))
				{
					cmd.Parameters.Add("@child", DbType.Int32);
					cmd.Parameters[0].Value = int.Parse(rev.ID);
					SQLiteDataReader rdr = cmd.ExecuteReader();
					
					if (rdr.HasRows)
						{
							while(rdr.Read())
								{
									int id = rdr.GetInt32(1);
									rev.addParent(id.ToString());
								}
						}
					
					rdr.Dispose();
					rdr = null;
					cmd.Connection.Close();
					cmd.Connection = null;
				}
		}
		
		/// <summary>
		/// save parents of a revision.
		/// </summary>
		/// <param name="rev"></param>
		public void save(Revision rev)
		{
			using (SQLiteCommand cmd = _getCmd(
@"INSERT INTO parents (child, parent) 
 VALUES(@child, @parent)"))
				{
					cmd.Parameters.Add("@child", DbType.Int32);
					cmd.Parameters.Add("@parent", DbType.Int32);
					int child = int.Parse(rev.ID);
					
					cmd.Parameters[0].Value = child;
					
					foreach(string parent in rev.Parents)
						{
							int parentID = int.Parse(parent);
							cmd.Parameters[1].Value = parentID;
							
							cmd.ExecuteNonQuery();
						}
					
					cmd.Connection.Close();
					cmd.Connection = null;
				}
		}
		
		public void del(Revision rev)
		{
			using (SQLiteCommand cmd = _getCmd(
@"DELETE FROM parents 
WHERE child = @child AND parent = @parent"))
				{
					cmd.Parameters.Add("@child", DbType.Int32);
					cmd.Parameters.Add("@parent", DbType.Int32);
					int child = int.Parse(rev.ID);
					
					cmd.Parameters[0].Value = child;
					
					foreach(string parent in rev.Parents)
						{
							int parentID = int.Parse(parent);
							cmd.Parameters[1].Value = parentID;
							
							cmd.ExecuteNonQuery();
						}
					
					System.Threading.Thread.Sleep(2);
					cmd.Connection.Close();
					cmd.Connection = null;
				}
		}
	}
}