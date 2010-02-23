
using System.Data.SQLite;

namespace SQLiteStorage
{
	using Revision = StarTree.Plugin.Database.Revision;
	using IEnumerable = System.Collections.IEnumerable;
	using IEnumerator = System.Collections.IEnumerator;
	using DbType = System.Data.DbType;
	using Revision_able = System.Collections.Generic.IEnumerable<StarTree.Plugin.Database.Revision>;
	using Revision_ator = System.Collections.Generic.IEnumerator<StarTree.Plugin.Database.Revision>;
	
	public class BranchRevisionResults : TableBase, Revision_able, Revision_ator
	{
		private SQLiteCommand _cmd;
		private SQLiteDataReader _rdr;
		private Revision _revision;
		private RevisionsTable _revTbl;
		private ParentsTable _parentTbl;
		private string _branch;
		private ulong _limit;
		
		public BranchRevisionResults(RevisionsTable revTbl, ParentsTable parTbl, 
																 string branch, ulong limit)
		{
			_revTbl = revTbl;
			_parentTbl = parTbl;
			_branch = branch;
			_limit = limit;
		}
		
		public Revision_ator GetEnumerator()
		{
			_cmd = _getCmd(
@"SELECT id 
  FROM revisions 
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
}