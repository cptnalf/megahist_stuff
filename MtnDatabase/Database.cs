using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

using TFSTree.Databases;

namespace Monotree
{
	using RevisionCont = treelib.AVLTree<Revision,RevisionSorterDesc>;
	
	/// <summary>Database connection to monotone's database.</summary>
	class Database : IRevisionRepo
	{
		/// <summary>Filename of the database.</summary>
		string _filename;
		
		/// <summary>Database connection.</summary>
		DbConnection _con;
		
		public event EventHandler<ProgressArgs> OnProgress;
		
		/// <summary>Creates a database connection.</summary>
		/// <param name="filename">Filename of the database to connect to.</param>
		internal Database(string filename)
		{
			_filename = filename;
			Init();
		}
		
		/// <summary>Gets the filename of the database.</summary>
		/// <value>Gets the filename of the database.</value>
		public string Name { get { return _filename; } }

		/// <summary>Initializes a database connection.</summary>
		private void Init()
		{
			//DbProviderFactory dataFactory = 
			//  DbProviderFactories.GetFactory("System.Data.Odbc");
			//_con = dataFactory.CreateConnection();
			_con = new System.Data.SQLite.SQLiteConnection();
			_con.ConnectionString = "data source=" + _filename + ";Read Only=True;FailIfMissing=True";
			_con.Open();
		}

		/// <summary>Decodes UTF8-encoded values.</summary>
		/// <param name="byteArray">UTF8-encoded value.</param>
		/// <returns>Decoded value.</returns>
		private string UTF8Decode(byte[] byteArray)
		{
			UTF8Encoding encoder = new UTF8Encoding();
			return encoder.GetString(byteArray).TrimEnd('\0');
		}

		/// <summary>Escapes a string to be used in a SQL statement.</summary>
		/// <param name="s">String to escape.</param>
		/// <returns>Escaped string.</returns>
		private string SQLEscape(string s)
		{
			return s.Replace("'", "\\'");
		}

		/// <summary>Gets branch names.</summary>
		/// <returns>Branch names.</returns>
		public IEnumerable<string> BranchNames
		{
			get
				{
					DbCommand cmd = _con.CreateCommand();
					cmd.CommandText = "select distinct value from revision_certs where name = 'branch' order by value";
					cmd.CommandType = CommandType.Text;
					
					DbDataReader r = cmd.ExecuteReader();
					List<string> names = new List<string>();
					while (r.Read())
						{
							byte[] buffer = new byte[256];
							r.GetBytes(0, 0, buffer, 0, buffer.Length);
							names.Add(UTF8Decode(buffer));
						}
					return names;
				}
		}

		/// <summary>Gets the latest revisions for a branch.</summary>
		/// <param name="branch">Branch to get revisions from.</param>
		/// <param name="limit">Maximum number of revisions to get.</param>
		/// <returns>Revisions.</returns>
		public RevisionCont getBranch(string branch, ulong limit)
		{
			DbCommand cmd = _con.CreateCommand();
			cmd.CommandText = "select distinct r.id from revisions as r inner join revision_certs as rc on (r.id = rc.id) where rc.name = 'branch' and rc.value like '" + SQLEscape(branch) + "' order by r.ROWID desc limit " + limit;
			cmd.CommandType = CommandType.Text;

			DbDataReader r = cmd.ExecuteReader();
			Dictionary<string, Revision> revs = new Dictionary<string, Revision>((int)limit);
			while (r.Read())
				{
					//byte[] buffer1 = new byte[256];
					//r.GetBytes(0, 0, buffer1, 0, buffer1.Length);
					//string id = UTF8Decode(buffer1);
					string id = r.GetString(0);

					revs.Add(id, new Revision(id, branch));
				}

			foreach (KeyValuePair<string, Revision> rev in revs)
				{
					cmd = _con.CreateCommand();
					cmd.CommandText = "select distinct rc1.value, rc2.value, rc3.value from revisions as r inner join revision_certs as rc1 on (r.id = rc1.id) inner join revision_certs as rc2 on (r.id = rc2.id) inner join revision_certs as rc3 on (r.id = rc3.id) where r.id = '" + SQLEscape(rev.Key) + "' and rc1.name = 'author' and rc2.name = 'date' and rc3.name = 'changelog'";
					cmd.CommandType = CommandType.Text;

					r = cmd.ExecuteReader();
					r.Read();
					
					byte[] buffer1 = new byte[256];
					r.GetBytes(0, 0, buffer1, 0, buffer1.Length);
					string author = UTF8Decode(buffer1);

					byte[] buffer2 = new byte[256];
					r.GetBytes(1, 0, buffer2, 0, buffer2.Length);
					string date = UTF8Decode(buffer2);

					byte[] buffer3 = new byte[256];
					r.GetBytes(2, 0, buffer3, 0, buffer3.Length);
					string log = UTF8Decode(buffer3);

					rev.Value.Author = author.Trim();
					rev.Value.Date = DateTime.Parse(date);
					rev.Value.Log = log.Trim();

					cmd = _con.CreateCommand();
					cmd.CommandText = "select distinct parent from revision_ancestry where child = '" + SQLEscape(rev.Key) + "'";
					cmd.CommandType = CommandType.Text;

					r = cmd.ExecuteReader();
					while (r.Read())
						{
							//buffer1 = new byte[256];
							//r.GetBytes(0, 0, buffer1, 0, buffer1.Length);
							//string id = UTF8Decode(buffer1);
							string id = r.GetString(0);

							if (id != "")
								rev.Value.Parents.Add(id);
						}
				}
			
			RevisionCont revisions = new RevisionCont();
			foreach(KeyValuePair<string,Revision> pair in revs)
				{
					if (! string.IsNullOrEmpty(pair.Value.ID))
						{ revisions.insert(pair.Value); }
				}

			return revisions;
		}

		/// <summary>Gets a revision.</summary>
		/// <param name="id">Unique identifier.</param>
		/// <returns>Revision or null if the unique identifier is invalid.</returns>
		public Revision rev(string id)
		{
			DbCommand cmd = _con.CreateCommand();
			cmd.CommandText = "select distinct rc1.value, rc2.value, rc3.value, rc4.value from revisions as r inner join revision_certs as rc1 on (r.id = rc1.id) inner join revision_certs as rc2 on (r.id = rc2.id) inner join revision_certs as rc3 on (r.id = rc3.id) inner join revision_certs as rc4 on (r.id = rc4.id) where r.id = '" + SQLEscape(id) + "' and rc1.name = 'author' and rc2.name = 'date' and rc3.name = 'changelog' and rc4.name = 'branch'";
			cmd.CommandType = CommandType.Text;

			DbDataReader r = cmd.ExecuteReader();
			if (r.Read())
				{
					byte[] buffer1 = new byte[256];
					r.GetBytes(0, 0, buffer1, 0, buffer1.Length);
					string author = UTF8Decode(buffer1);

					byte[] buffer2 = new byte[256];
					r.GetBytes(1, 0, buffer2, 0, buffer2.Length);
					string date = UTF8Decode(buffer2);

					byte[] buffer3 = new byte[256];
					r.GetBytes(2, 0, buffer3, 0, buffer3.Length);
					string log = UTF8Decode(buffer3);

					byte[] buffer4 = new byte[256];
					r.GetBytes(3, 0, buffer4, 0, buffer4.Length);
					string branch = UTF8Decode(buffer4);

					return new Revision(id, branch.Trim(), author.Trim(), date, log.Trim());
				}

			return null;
		}

		/// <summary>Compresses a database.</summary>
		/// <remarks>When a database is compressed all data is deleted which is not used by Monotree.</remarks>
		public void Compress()
		{
			string[] tables = new string[] { "heights", "rosters", "roster_deltas", "public_keys", "manifest_certs", "manifest_deltas", "manifests", "files", "file_deltas" };

			foreach (string table in tables)
				{
					DbCommand cmd = _con.CreateCommand();
					cmd.CommandText = "drop table if exists " + table;
					cmd.CommandType = CommandType.Text;
					cmd.ExecuteNonQuery();
				}

			DbCommand cmd2 = _con.CreateCommand();
			cmd2.CommandText = "vacuum";
			cmd2.CommandType = CommandType.Text;
			cmd2.ExecuteNonQuery();
		}
	}
}
