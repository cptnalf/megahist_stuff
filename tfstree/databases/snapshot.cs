
namespace TFSTree.Databases
{
	using RevisionCont = treelib.AVLTree<Revision, RevisionSorterDesc>;
	using BranchContainer = treelib.AVLTree<string, treelib.StringSorterInsensitive>;
	using RevisionIdx = treelib.AVLDict<string, Revision>;
	using BranchChangesets =
		treelib.AVLDict<string, treelib.AVLDict<string, Revision>, treelib.StringSorterInsensitive>;

	using FileStream = System.IO.FileStream;
	using Stream = System.IO.Stream;
	using FileMode = System.IO.FileMode;
	using FileAccess = System.IO.FileAccess;
	using MemoryStream = System.IO.MemoryStream;
	
	using XmlSerializer = System.Xml.Serialization.XmlSerializer;
	
	public class Snapshot : RevisionRepoBase, IRevisionRepo
	{
		private static readonly byte[] MAGIC_BYTES = new byte[] { 0,0};
		
		private string _filename;
		
		public RevisionIdx Revisions { get { return this._changesetIdx; } }
		public BranchChangesets BranchChangesets { get { return this._branchChangesets; } }
		
		public event System.EventHandler<ProgressArgs> OnProgress;
		
		public Snapshot() { }
		
		public string Name { get { return _filename; } }
		
		public void close() { }
		
		public void save(string filename, string branch, Microsoft.Glee.Drawing.Graph graph)
		{
			/* create the snapshot */
			
			Stream w = new FileStream(filename, FileMode.Create, FileAccess.Write);
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(branch);
			
			w.Write(bytes, 0, bytes.Length);
			w.Write(MAGIC_BYTES, 0, MAGIC_BYTES.Length);
			
			RevisionCont revisions = _getRevisions(graph);
			XmlSerializer s = new XmlSerializer(typeof(Revision));
			
			RevisionCont.iterator it = revisions.begin();
			for(; it != revisions.end(); ++it)
				{
					s.Serialize(w, it.item());
					w.Write(MAGIC_BYTES, 0, MAGIC_BYTES.Length);
				}
			
			w.Flush();
			w.Close();
		}
		
		public void loadfolder(string filename) { throw new System.NotImplementedException(); }
		
		public void load(string filename)
		{
			_filename = filename;
			
			using (Stream r = new FileStream(filename, FileMode.Open, FileAccess.Read))
				{
					XmlSerializer s = new XmlSerializer(typeof(Revision));
					Revision rev = null;
					string branch = null;
					MemoryStream strm;
					
					strm = _readBytes(r);
					if (strm != null)
						{
							branch = System.Text.Encoding.UTF8.GetString(strm.ToArray());
							strm = null;
						}
					else { throw new System.Exception("branch name not found!"); }
					
					_branches.insert(branch);
					
					do {
						strm = _readBytes(r);
						if (strm != null)
							{
								rev = s.Deserialize(strm) as Revision;
					 
								if (rev != null) { _addRevision(rev); }
							}
					}	while( strm != null);
					
					r.Close();
				}
		}
		
		
		public void fixhistory(string branch)
		{
			/* find the branch, then add parents for the revision history.
			 * 
			 * this just assumes you have the complete history for the branch.
			 * you may not have the complete history, in which case this might look weird.
			 */
			
			BranchChangesets.iterator bit = _branchChangesets.find(branch);
			if (bit != _branchChangesets.end())
				{
					string prev = null;
					
					for(RevisionIdx.iterator it = bit.value().begin();
							it != bit.value().end();
							++it)
						{
							if (prev != null) { it.value().addParent(prev); }
							prev = it.value().ID;
						}
				}
		}
		
		private MemoryStream _readBytes(Stream r)
		{
			/* i would need to chunk this up. */
			MemoryStream strm = new MemoryStream();
			byte[] b = new byte[1];
			bool seen = false;
			byte[] zero = new byte[] {0};
			
			while(0 < r.Read(b, 0, 1))
				{
					if (seen)
						{
							if (b[0] == 0) { break; }
							else
								{
									/* write the zero byte we found back out to the stream
									 * before we process the byte we just got.
									 */
									strm.Write(zero, 0, zero.Length);
								}
						}
					
					if (b[0] == 0) {	seen = true; }
					else           { strm.Write(b, 0, 1); }
				}
			
			strm.Flush();
			
			if (strm.Length > 0) { strm.Seek(0, System.IO.SeekOrigin.Begin); }
			else
				{
					/* destroy the stream, return null. */
					strm.Close(); 
					strm = null;
				}
			
			return strm;
		}
		
		private RevisionCont _getRevisions(Microsoft.Glee.Drawing.Graph graph)
		{
			RevisionCont revisions = new RevisionCont();
			foreach(object val in graph.NodeMap.Values)
				{
					Microsoft.Glee.Drawing.DrawingObject obj = val as Microsoft.Glee.Drawing.DrawingObject;
					if (obj != null)
						{
							Revision r = obj.UserData as Revision;
							if (r != null) { revisions.insert(r); }
						}
				}
			return revisions;
		}
		
	}
}
