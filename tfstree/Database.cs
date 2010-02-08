using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace TFSTree
{
	/// <summary>Database connection to monotone's database.</summary>
	class Database
	{
		/// <summary>Filename of the database.</summary>
		string filename;
		
		/// <summary>raw list of all revisions. </summary>
		SortedList<string,Revision> _revisions = new SortedList<string,Revision>();
		
		/// <summary>branch to revisions map</summary>
		Dictionary<string,SortedList<string,Revision>> _branches = 
			new Dictionary<string,SortedList<string,Revision>>();
		
		/// <summary>Creates a database connection.</summary>
		/// <param name="filename">Filename of the database to connect to.</param>
		public Database(string filename)
		{
			this.filename = filename;
			Init();
		}

		/// <summary>Gets the filename of the database.</summary>
		/// <value>Gets the filename of the database.</value>
		public string FileName
		{
			get { return filename; }
		}

		/// <summary>Initializes a database connection.</summary>
		private void Init()
		{
			/* this table contains the directed-graph definition of 'parents' and 'children'.
			 * so:
			 * 1 -> 2 -> 4
			 *      3 --/
			 * so the parents of 4 are 2, 3.
			 * parents of 2 are 1
			 * parents of 3 are null.
			 */			
			
			/* temp tables to generate the necessary data. */
			DataTable raw = new DataTable("tmp");
			raw.Columns.Add("cs", typeof(string));
			raw.Columns.Add("parent", typeof(string));
			raw.Columns.Add("branch", typeof(string));
			raw.Columns.Add("author", typeof(string));
			raw.Columns.Add("date", typeof(string));
			
			/* raw import. */
			using (System.IO.StreamReader rdr = new System.IO.StreamReader(filename))
				{
					for(string line = rdr.ReadLine(); line != null; line = rdr.ReadLine())
						{
							string[] parts = line.Split(','); //'\0');
							bool add_row = false;
							
							if (_revisions.ContainsKey(parts[0]))
								{
									Revision rev = _revisions[parts[0]];
									
									if (parts[1] != "0")
										{
											rev.Parents.Add(parts[1]);
										}
								}
							else
								{
									Revision r = new Revision(parts[0], parts[2], parts[3], parts[4], string.Empty);
									
									if (parts[1] != "0") { r.Parents.Add(parts[1]); }
									
									if (!_revisions.ContainsKey(r.ID))
										{
											_revisions.Add(r.ID, r);
										}
									
									if (!_branches.ContainsKey(r.Branch)) 
										{
											SortedList<string,Revision> l = new SortedList<string,Revision>();
											l.Add(r.ID, r);
											_branches.Add(r.Branch, l);
										}
									
									if (! _branches[r.Branch].ContainsKey(r.ID))
										{ _branches[r.Branch].Add(r.ID, r); }
								}
						}
				}							
							
// 							if (parts[1] == "0")
// 								{
// 									DataRow[] rows = raw.Select(string.Format("[cs] = {0}", parts[0]));
// 									add_row = (rows != null && rows.Length !=0);
// 								}
// 							else { add_row = true; }
							
// 							if (add_row)
// 								{ raw.Rows.Add(parts[0], parts[1], parts[2], parts[3], parts[4]); }
// 						}
// 				}
			
// 			/** add in the list o changesets, add in the branch lists. */
// 			foreach(DataRow row in raw.Rows)
// 				{
// 					Revision r = new Revision((string)row["cs"], (string)row["branch"], 
// 																		(string)row["author"], (string)row["date"],
// 																		string.Empty);
					
// 					if (!_revisions.ContainsKey(r.ID))
// 						{
// 							_revisions.Add(r.ID, r);
// 							if (_branches.ContainsKey(r.Branch)) { _branches[r.Branch].Add(r.ID,r); }
// 							else
// 								{
// 									SortedList<string,Revision> l = new SortedList<string,Revision>();
// 									l.Add(r.ID,r);
// 									_branches.Add(r.Branch, l);
// 								}
// 						}
// 				}
			
// 			foreach(string key in _revisions.Keys)
// 				{
// 					/* get the list of merge parents. */
// 					DataRow[] recs = raw.Select(string.Format("[parent] = '{0}'", key));
// 					foreach(DataRow row in recs)
// 						{
// 							string k = (string)row["cs"];
// 							int idx = _revisions[key].Parents.FindIndex(delegate(string f) { return f == k; });
							
// 							if (idx < 0)
// 								{
// 									_revisions[key].Parents.Add((string)row["cs"]);
// 								}
// 						}
					
// 					/* now get the parent in this branch. */
// 					SortedList<string,Revision> branch_hist;
// 					if (_branches.TryGetValue(_revisions[key].Branch, out branch_hist))
// 						{
// 							int idx = branch_hist.IndexOfKey(key);
// 							if (idx -1 > 0)
// 								{
// 									/* now add the previous changeset in this branch, since we found one */
// 									_revisions[key].Parents.Add(branch_hist.Values[idx-1].ID);
// 								}
// 						}
// 				}
			
// 			using (System.IO.StreamWriter wr = new System.IO.StreamWriter("flarg.branches.txt"))
// 				{
// 					foreach(string branch in _branches.Keys)
// 						{
// 							string[] values = new string[_branches[branch].Count];
// 							_branches[branch].Keys.CopyTo(values, 0);
// 							wr.WriteLine("{0}={1}", branch, string.Join(",",values));
// 						}
// 				}
			
// 			using (System.IO.StreamWriter wr = new System.IO.StreamWriter("flarg.txt"))
// 				{
// 					foreach(string key in _revisions.Keys)
// 						{
// 							string[] values = new string[_revisions[key].Parents.Count];
// 							_revisions[key].Parents.CopyTo(values);
// 							wr.WriteLine("{0}\0{1}\0{2}\0{3}\0{4}",
// 													 _revisions[key].ID, _revisions[key].Branch, _revisions[key].Author,
// 													 _revisions[key].Date, string.Join(",", values));
// 						}
// 				}
		}

		/// <summary>Gets branch names.</summary>
		/// <returns>Branch names.</returns>
		public List<string> GetBranchNames()
		{
			List<string> branches = new List<string>();
			branches.AddRange(_branches.Keys);
			
			return branches;
		}

		/// <summary>Gets the latest revisions for a branch.</summary>
		/// <param name="branch">Branch to get revisions from.</param>
		/// <param name="limit">Maximum number of revisions to get.</param>
		/// <returns>Revisions.</returns>
		public Dictionary<string, Revision> GetLatestRevisionIDs(string branch, int limit)
		{
			Dictionary<string,Revision> revisions = new Dictionary<string,Revision>();
			
			if (_branches.ContainsKey(branch))
				{
					SortedList<string,Revision> revs = _branches[branch];
					limit = (limit > revs.Count ? revs.Count : limit);
					int max = revs.Values.Count-1;
					
					for(int i=0; i < limit; ++i)
						{
							Revision r = revs.Values[max -i];
							revisions.Add(r.ID, r);
						}
				}
			return revisions;
		}
		
		/// <summary>Gets a revision.</summary>
		/// <param name="id">Unique identifier.</param>
		/// <returns>Revision or null if the unique identifier is invalid.</returns>
		public Revision GetRevision(string id)
		{
			Revision rev;
			_revisions.TryGetValue(id, out rev);
			return rev;
		}

		/// <summary>Compresses a database.</summary>
		/// <remarks>When a database is compressed all data is deleted which is not used by TFSTree.</remarks>
		public void Compress()
		{
			
		}
	}
}
