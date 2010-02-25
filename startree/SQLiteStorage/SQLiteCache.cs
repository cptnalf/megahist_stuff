
namespace SQLiteStorage
{
	using Revision = StarTree.Plugin.Database.Revision;
	using Snapshot = StarTree.Plugin.Database.Snapshot;
	using BranchContainer = treelib.AVLTree<string, treelib.StringSorterInsensitive>;
	using RevisionCont = treelib.AVLTree<StarTree.Plugin.Database.Revision, StarTree.Plugin.Database.RevisionSorterDesc>;
	
	public class SQLiteCache
	{
		private string _connStr;
		private RevisionsTable _revsTbl = new RevisionsTable();
		private ParentsTable _parentsTbl = new ParentsTable();
		
		public SQLiteCache() { }

		public virtual string Name { get { return _connStr; } }
		
		/// <summary>
		/// load the branches from the database.
		/// </summary>
		public virtual System.Collections.Generic.IEnumerable<string> BranchNames
		{ get { return _revsTbl.getBranches(); } }
		
		/// <summary>
		/// load revisions directly from the database.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public virtual Revision rev(string id)
		{
			int revID = int.Parse(id);
			return rev(revID);
		}
		
		public virtual Revision rev(int id)
		{
			Revision revision = _revsTbl.getRev(id);
			if (revision != null) { _parentsTbl.load(ref revision); }

			return revision;			
		}

		public virtual Snapshot getBranch(string branch, ulong limit)
		{
			Snapshot sn = new Snapshot();
			BranchRevisionResults query = new BranchRevisionResults(_revsTbl, _parentsTbl,
																															branch, limit);
			query.ConnectionString = _connStr;
			
			foreach(Revision rev in query)
				{
					Revision fuckyouCS = rev;
					_parentsTbl.load(ref fuckyouCS);
					sn.add(fuckyouCS);
				}
			
			return sn;
		}
		
		public virtual Snapshot queryMerges(Revision rev)
		{
			Snapshot sn = new Snapshot();
			
			sn.add(rev);
			foreach(string parent in rev.Parents)
				{
					int id = int.Parse(parent);
					Revision p = _revsTbl.getRev(id);
					
					if (p != null)
						{
							_parentsTbl.load(ref p);
							
							sn.add(p);
						}
				}
			
			return sn;
		}
		
		/// <summary>
		/// prime the database
		/// </summary>
		/// <param name="filename"></param>
		public virtual void load(string filename)
		{
			_connStr = string.Format("data source={0}", filename);
			_revsTbl.ConnectionString = _connStr;
			_parentsTbl.ConnectionString = _connStr;
			
			_revsTbl.create();
			_parentsTbl.create();
		}
		
		public virtual void close() { }

		public object start()
		{
			System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection(_connStr);
			conn.Open();
			
			return conn;
		}
		
		public void end(object data)
		{
			System.Data.SQLite.SQLiteConnection conn = data as System.Data.SQLite.SQLiteConnection;
			conn.Close();
			conn = null;
		}
		
		public void save(object data, Revision rev)
		{
			System.Data.SQLite.SQLiteConnection conn = data as System.Data.SQLite.SQLiteConnection;
			_revsTbl.save(conn, rev);
			_parentsTbl.save(conn, rev);
		}
		
		public void del(Revision rev)
		{
			_revsTbl.del(rev);
			_parentsTbl.del(rev);
		}
	}
}
