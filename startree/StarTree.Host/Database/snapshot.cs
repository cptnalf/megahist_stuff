
namespace StarTree.Host.Database
{
	using RevisionCont = treelib.AVLTree<Revision, RevisionSorterDesc>;
	using BranchContainer = treelib.AVLTree<string, treelib.sorters.StringInsensitive>;
	using RevisionIdx = treelib.AVLDict<string, Revision>;
	using BranchChangesets =
		treelib.AVLDict<string, treelib.AVLDict<string, Revision>, treelib.sorters.StringInsensitive>;
	
	using Stream = System.IO.Stream;
	using MemoryStream = System.IO.MemoryStream;
	using XmlSerializer = System.Xml.Serialization.XmlSerializer;
	
	public class Snapshot : RevisionRepoBase
	{
		private static readonly byte[] MAGIC_BYTES = new byte[] { 0,0};
		
		private string _filename;
		private Plugin _plugin;
		
		//public RevisionIdx Revisions { get { return this._changesetIdx; } }
		//public BranchChangesets BranchChangesets { get { return this._branchChangesets; } }
		
		public Snapshot(Plugin p) { _plugin = p; }
		
		public ulong size() { return _changesetIdx.size(); }
		
		public RevisionIdx.iterator find(string branch)
		{
			RevisionIdx.iterator rit = RevisionIdx.End();
			BranchChangesets.iterator bit = _branchChangesets.find(branch);
			
			if (bit != _branchChangesets.end()) { rit = bit.value().begin(); }			
			return rit;
		}
		
		public override Revision rev(string id)
		{
			Revision rev = null;
			RevisionIdx.iterator it = this._changesetIdx.find(id);
			if (it != _changesetIdx.end()) { rev = it.value(); }
			else
				{
					if (_plugin != null)
						{
							rev = _plugin.getRevision(id);
							/* only add if we actually found something */
							if (rev != null) { add(rev); }
						}
				}
			
			return rev;
		}
		
		public void add(Revision rev) { _addRevision(rev); }
		
		/// <summary>
		/// save the current status of this snapshot out to a stream (file?)
		/// </summary>
		/// <param name="w"></param>
		public void save(System.IO.Stream w)
		{
			/* create the snapshot */
			XmlSerializer s = new XmlSerializer(typeof(Revision));
			
			RevisionIdx.iterator it = this._changesetIdx.begin();
			for (; it != this._changesetIdx.end(); ++it)
				{
					s.Serialize(w, it.value());
					w.Write(MAGIC_BYTES, 0, MAGIC_BYTES.Length);
				}
			
			w.Flush();
			w.Close();
		}
		
		/// <summary>
		/// load a byte stream into this snapshot.
		/// </summary>
		/// <param name="bytes"></param>
		public void load(byte[] bytes)
		{
			MemoryStream mr = new MemoryStream(bytes);
			load(mr);
		}
		
		/// <summary>
		/// load a stream (maybe from a file) into this snapshot.
		/// </summary>
		/// <param name="r"></param>
		public void load(System.IO.Stream r)
		{
			XmlSerializer s = new XmlSerializer(typeof(Revision));
			Revision rev = null;
			string branch = null;
			MemoryStream strm;

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
	}
}
