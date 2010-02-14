
/* going to beef-up tfs with some sqlite action.
 * i probably only need to store the branch query results, 
 * but i think i'm going to try to cache more.
 * 
 */
 
using System.Data.SQLite;

namespace TFSTree.Databases.SQLiteCache
{
	using IEnumerable = System.Collections.IEnumerable;
	using IEnumerator = System.Collections.IEnumerator;
	using DbType = System.Data.DbType;
	using BranchContainer = treelib.AVLTree<string, treelib.StringSorterInsensitive>;
	
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
	
	public class BranchRevisionResults : TableBase,
		System.Collections.Generic.IEnumerable<Revision>,
		System.Collections.Generic.IEnumerator<Revision>
	{
		private SQLiteCommand _cmd;
		private SQLiteDataReader _rdr;
		private Revision _revision;
		private RevisionTable _revTbl;
		private ParentsTable _parentTbl;
		private string _branch;
		private ulong _limit;
		
		public BranchRevisionResults(RevisionTable revTbl, ParentsTable parTbl, 
																 string branch, ulong limit)
		{
			_revTbl = revTbl;
			_parentTbl = parTbl;
			_branch = branch;
			_limit = limit;
		}
		
		public System.Collections.Generic.IEnumerator<Revision> GetEnumerator()
		{
			_cmd = _getCmd(
@"SELECT id 
  FROM revision 
  WHERE branch = @branch 
  ORDER BY id DESC 
  LIMIT @limit");
			_cmd.Parameters.Add("@branch", DbType.String);
			_cmd.Parameters.Add("@limit", DbType.Int64);
			
			_cmd.Parameters[0].Value = _branch;
			_cmd.Parameters[1].Value = (long)_limit;
			
			return this;
		}
		
		IEnumerator IEnumerable.GetEnumerator() { return this.GetEnumerator(); }
		
		public void Dispose()
		{
			if (_cmd != null)
				{
					_cmd.Dispose();
					_cmd = null;
					_rdr = null;
				}
		}
		
		public Revision Current { get { return _revision; } }
		
		public bool MoveNext()
		{
			bool haveNext = false;
			if (_rdr == null)
				{
					_rdr = _cmd.ExecuteReader();
					
					haveNext = _rdr.HasRows && _rdr.Read();
				}
			else { haveNext = _rdr.Read(); }
			
			if (haveNext)
				{
					_revision = _revTbl.getRev(_rdr.GetInt32(0));
					_parentTbl.load(ref _revision);
				}
			
			return haveNext;
		}

		object IEnumerator.Current { get { return this.Current; } }
		bool IEnumerator.MoveNext() { return this.MoveNext(); }
		void IEnumerator.Reset() { throw new System.NotSupportedException(); }
	}
	
	public class RevisionTable : TableBase
	{
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
@"INSERT INTO revision (id, branch, author, created, comment)
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
		
		public BranchContainer getBranches()
		{
			BranchContainer branches = new BranchContainer();
			
			using (SQLiteCommand cmd = _getCmd(
@"SELECT DISTINCT branch 
  FROM revision 
  ORDER BY branch"))
				{
					SQLiteDataReader rdr = cmd.ExecuteReader();
					if (rdr.HasRows) { while(rdr.Read()) { branches.insert(rdr.GetString(0)); } }
				}
			
			return branches;
		}
	}
	
	/// <summary>
	/// the parents of a revision.
	/// </summary>
	public class ParentsTable : TableBase
	{
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
				}
		}
	}
}
