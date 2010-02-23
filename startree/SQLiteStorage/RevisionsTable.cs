
using System.Data.SQLite;

namespace SQLiteStorage
{
	using Revision = StarTree.Plugin.Database.Revision;
	using DbType = System.Data.DbType;
	using BranchContainer = treelib.AVLTree<string, treelib.StringSorterInsensitive>;
	
	public class RevisionsTable : TableBase
	{
		public void create()
		{
			try{
				using (SQLiteCommand cmd = _getCmd(
@"CREATE TABLE IF NOT EXISTS revisions 
  (id INTEGER, branch TEXT, author TEXT, created DATETIME, comment TEXT)"))
					{
						cmd.ExecuteNonQuery();
					}
				
				using (SQLiteCommand cmd = _getCmd(
@"CREATE INDEX IF NOT EXISTS revisions_id ON revisions (id)"))
					{
						cmd.ExecuteNonQuery();
					}
				using (SQLiteCommand cmd = _getCmd(
@"CREATE INDEX IF NOT EXISTS revisions_branch ON revisions (branch)"))
					{
						cmd.ExecuteNonQuery();
					}
			}
			catch(System.Exception e)
				{
				}
		}
		
		public Revision getRev(int id)
		{
			Revision rev = null;
			using(SQLiteCommand cmd = 
						_getCmd(
@"SELECT id, branch, author, created, comment 
  FROM revisions 
  WHERE id = @id"))
				{
					cmd.Parameters.Add("@id", DbType.Int32);
					cmd.Parameters[0].Value = id;
					SQLiteDataReader rdr = cmd.ExecuteReader();
					
					if (rdr.HasRows && rdr.Read())
						{
							rev = new Revision();
							rev.ID = rdr.GetInt32(0).ToString();
							rev.Branch = rdr.GetString(1);
							rev.Author = rdr.GetString(2);
							rev.Date = rdr.GetDateTime(3);
							rev.Log = rdr.GetString(4);
						}
					rdr.Dispose();
					rdr = null;
					cmd.Connection.Close();
					cmd.Connection = null;
				}
			
			return rev;
		}
		
		/// <summary>
		/// save a revision to the database cache.
		/// </summary>
		/// <param name="rev"></param>
		public void save(Revision rev)
		{
			using (SQLiteCommand cmd = 
						 _getCmd(
@"INSERT INTO revisions (id, branch, author, created, comment)
  VALUES (@id, @branch, @author, @created, @comment)"))
				{
					cmd.Parameters.Add("@id", DbType.Int32);
					cmd.Parameters[0].Value = int.Parse(rev.ID);
					
					cmd.Parameters.Add("@branch", DbType.String);
					cmd.Parameters[1].Value = rev.Branch;
					
					cmd.Parameters.Add("@author", DbType.String);
					cmd.Parameters[2].Value = rev.Author;
					
					cmd.Parameters.Add("@created", DbType.DateTime);
					cmd.Parameters[3].Value = rev.Date;
			
					cmd.Parameters.Add("@comment", DbType.String);
					cmd.Parameters[4].Value = rev.Log;
					
					cmd.ExecuteNonQuery();
				}
		}
		
		public void del(Revision rev)
		{
			using (SQLiteCommand cmd = _getCmd(
@"DELETE FROM revisions
WHERE id = @id AND branch = @branch AND author = @author AND created = @created"))
				{
					cmd.Parameters.Add("@id", DbType.Int32);
					cmd.Parameters[0].Value = int.Parse(rev.ID);
					
					cmd.Parameters.Add("@branch", DbType.String);
					cmd.Parameters[1].Value = rev.Branch;
					
					cmd.Parameters.Add("@author", DbType.String);
					cmd.Parameters[2].Value = rev.Author;
					
					cmd.Parameters.Add("@created", DbType.DateTime);
					cmd.Parameters[3].Value = rev.Date;
					
					cmd.ExecuteNonQuery();
					
					System.Threading.Thread.Sleep(2);
					cmd.Connection.Close();
					cmd.Connection = null;
				}
		}
		
		public BranchContainer getBranches()
		{
			BranchContainer branches = new BranchContainer();
			
			using (SQLiteCommand cmd = _getCmd(
@"SELECT DISTINCT branch 
  FROM revisions
  ORDER BY branch"))
				{
					SQLiteDataReader rdr = cmd.ExecuteReader();
					if (rdr.HasRows) { while(rdr.Read()) { branches.insert(rdr.GetString(0)); } }
				}
			
			return branches;
		}
	}
	
}
