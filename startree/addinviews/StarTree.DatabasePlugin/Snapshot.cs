using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarTree.Plugin.Database
{
	using RevisionCont = treelib.AVLTree<Revision, RevisionSorterDesc>;
	using BranchContainer = treelib.AVLTree<string, treelib.StringSorterInsensitive>;
	using RevisionIdx = treelib.AVLDict<string, Revision>;
	using BranchChangesets =
	treelib.AVLDict<string, treelib.AVLDict<string, Revision>, treelib.StringSorterInsensitive>;

	using Stream = System.IO.Stream;
	using MemoryStream = System.IO.MemoryStream;

	using XmlSerializer = System.Xml.Serialization.XmlSerializer;
	
	public class Snapshot : RevisionRepoBase
	{
		private static readonly byte[] MAGIC_BYTES = new byte[] { 0, 0 };
		
		private string _filename;
		
		public RevisionIdx Revisions { get { return this._changesetIdx; } }
		public BranchChangesets BranchChangesets { get { return this._branchChangesets; } }
		
		public Snapshot() { }
		
		public void add(Revision rev)
		{
			this._addRevision(rev);
		}
		
		public byte[] serialize()
		{
			MemoryStream w = new MemoryStream();
			XmlSerializer s = new XmlSerializer(typeof(Revision));

			RevisionIdx.iterator it = _changesetIdx.begin();
			for (; it != _changesetIdx.end(); ++it)
				{
					s.Serialize(w, it.value());
					w.Write(MAGIC_BYTES, 0, MAGIC_BYTES.Length);
				}
			
			w.Flush();
			w.Close();			
			byte[] result = w.ToArray();
			return result;
		}
		
		public void load(byte[] snapshot)
		{
			using (MemoryStream r = new MemoryStream(snapshot, 0, snapshot.Length, false, false))
				{
					XmlSerializer s = new XmlSerializer(typeof(Revision));
					Revision rev = null;
					MemoryStream strm;
					
					do
						{
							strm = _readBytes(r);
							if (strm != null)
								{
									rev = s.Deserialize(strm) as Revision;

									if (rev != null) { _addRevision(rev); }
								}
						} while (strm != null);

					r.Close();
				}
		}

		private MemoryStream _readBytes(Stream r)
		{
			/* i would need to chunk this up. */
			MemoryStream strm = new MemoryStream();
			byte[] b = new byte[1];
			bool seen = false;
			byte[] zero = new byte[] { 0 };

			while (0 < r.Read(b, 0, 1))
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

					if (b[0] == 0) { seen = true; }
					else { strm.Write(b, 0, 1); }
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
