
namespace TFSTree.Databases.SQLiteCache
{
	using BranchContainer = treelib.AVLTree<string, treelib.StringSorterInsensitive>;
	using RevisionCont = treelib.AVLTree<Revision,RevisionSorterDesc>;
	
	public class SQLiteCache : IRevisionRepo
	{
		private string _connStr;
		private RevisionsTable _revsTbl = new RevisionsTable();
		private ParentsTable _parentsTbl = new ParentsTable();
		
		public SQLiteCache() { }

#region IRevisionRepo Members

		public string Name { get { return _connStr; } }
		
		/// <summary>
		/// load the branches from the database.
		/// </summary>
		public System.Collections.Generic.IEnumerable<string> BranchNames
		{ get { return _revsTbl.getBranches(); } }
		
		/// <summary>
		/// load revisions directly from the database.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Revision rev(string id)
		{
			int revID = int.Parse(id);
			Revision revision = _revsTbl.getRev(revID);
			if (revision != null) { _parentsTbl.load(ref revision); }
			
			return revision;
		}

		public RevisionCont getBranch(string branch, ulong limit)
		{
			RevisionCont revisions = new RevisionCont();
			BranchRevisionResults query = new BranchRevisionResults(_revsTbl, _parentsTbl,
																															branch, limit);
			query.ConnectionString = _connStr;
			
			foreach(Revision rev in query)
				{
					Revision fuckyouCS = rev;
					_parentsTbl.load(ref fuckyouCS);
					revisions.insert(fuckyouCS);
					if (OnProgress != null) { OnProgress(this, new ProgressArgs(1, null)); }
				}
			
			return revisions;
		}
		
		/// <summary>
		/// prime the database
		/// </summary>
		/// <param name="filename"></param>
		public void load(string filename)
		{
			_connStr = string.Format("data source={0}", filename);
			_revsTbl.ConnectionString = _connStr;
			_parentsTbl.ConnectionString = _connStr;
			
			_revsTbl.create();
			_parentsTbl.create();
		}
		
		public void close() { }
		
		public event System.EventHandler<ProgressArgs> OnProgress;

#endregion
		
		public void save(Revision rev)
		{
			_revsTbl.save(rev);
			_parentsTbl.save(rev);
		}
		
		public void del(Revision rev)
		{
			_revsTbl.del(rev);
			_parentsTbl.del(rev);
		}
	}
}
