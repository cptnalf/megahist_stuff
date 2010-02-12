
namespace TFSTree.Databases
{
	/*
	 * 
1) slurp in all of the changesets.
6) 
	 */
	using RevisionCont = treelib.AVLTree<Revision>;
	using BranchContainer = treelib.AVLTree<string, treelib.StringSorterInsensitive>;
	using ChangesetIdx = treelib.AVLDict<int,Revision,treelib.IntSorterDesc>;
	using BranchChangesets = 
		treelib.AVLDict<string,treelib.AVLDict<int,Revision,treelib.IntSorterDesc>,treelib.StringSorterInsensitive>;
	using ChangesetCont = treelib.AVLTree<ChangesetLoader.Changeset>;
	
	using FileStream = System.IO.FileStream;
	using Stream = System.IO.Stream;
	using FileMode = System.IO.FileMode;
	using FileAccess = System.IO.FileAccess;
	using MemoryStream = System.IO.MemoryStream;
	
	using XmlSerializer = System.Xml.Serialization.XmlSerializer;
	
	public class FakeTFS : IRevisionRepo
	{
		private treelib.AVLDict<string,string, treelib.StringSorterInsensitive> _branchMap;
		
		private ChangesetIdx _changesetIdx = new ChangesetIdx();
		private BranchContainer _branches = new BranchContainer();
		private BranchChangesets _branchChangesets = new BranchChangesets();
		private string _filename;
		
		public event System.EventHandler<ProgressArgs> OnProgress;
		
		public FakeTFS()
		{
			_branchMap = new treelib.AVLDict<string,string, treelib.StringSorterInsensitive>();
			
			_branchMap.insert("$/IGT_0803/development/dev_advantage/EGS/", "devadv");
			_branchMap.insert("$/IGT_0803/release/EGS8.1/SP2/RTM/EGS/", "81sp2rtm");
			_branchMap.insert("$/IGT_0803/release/EGS8.1/SP2/HF/EGS/", "81sp2hf");
			_branchMap.insert("$/IGT_0803/release/EGS8.1/SP1/HF/EGS/", "81sp1hf");
			_branchMap.insert("$/IGT_0803/release/EGS8.1/SP0/HF/EGS/", "81rtm");
			_branchMap.insert("$/IGT_0803/release/EGS8.1/dev_sp/EGS/", "81devsp");
			_branchMap.insert("$/IGT_0803/main/EGS/", "main");
			_branchMap.insert("$/IGT_0803/development/dev_uInstall/EGS/", "devuinstall");
			_branchMap.insert("$/IGT_0803/development/dev_tableManager/EGS/", "devtableManager");
			_branchMap.insert("$/IGT_0803/development/dev_tableID/EGS/", "dev_tableID");
			_branchMap.insert("$/IGT_0803/development/dev_mariposa/EGS/", "dev_mariposa");
			_branchMap.insert("$/IGT_0803/development/dev_MA/EGS/", "dev_ma");
			_branchMap.insert("$/IGT_0803/development/dev_eft/EGS/", "deveft");
			_branchMap.insert("$/IGT_0803/development/dev_build/EGS/", "dev_build");
			_branchMap.insert("$/IGT_0803/development/dev_AX/EGS/", "dev_ax");
			_branchMap.insert("$/IGT_0803/development/dev_AdvAsia/EGS/", "dev_advasias");
		}
		
		public Revision this[string id]
		{
			get
				{
					Revision rev = null;
					int csid = System.Int32.Parse(id);
					ChangesetIdx.iterator it = _changesetIdx.find(csid);
					if (it != _changesetIdx.end()) { rev = it.value(); }
					return rev;
				}
		}
		
		public Revision rev(string id) { return this[id]; }
		
		public string FileName { get { return _filename; } }
		
		public System.Collections.Generic.IEnumerable<string> BranchNames
		{ get { return _branches; } }
		
		public System.Collections.Generic.Dictionary<string,Revision> revs(string branch, ulong limit)
		{
			System.Collections.Generic.Dictionary<string,Revision> revisions = 
				new System.Collections.Generic.Dictionary<string,Revision>();
			
			BranchChangesets.iterator it = _branchChangesets.find(branch);
			if (it != _branchChangesets.end())
				{
					ulong count = (it.value().size() > limit) ? limit : it.value().size();
					ChangesetIdx.iterator csit = it.value().begin();
					
					for(ulong i=0; i < count; ++i, ++csit)
						{
							if (csit == it.value().end()) { break; }
							revisions.Add(csit.value().ID, csit.value());
						}
				}
			
			return revisions;
		}
		
		public void loadfolder(string filename)
		{
 			_filename = filename;
			
// 			if (System.IO.File.Exists(filename))
// 				{
// 					/* its just a file. */
// 					_loadFile(filename);
// 				}
// 			else
// 				{
					/* could be a path, or it could be that we don't have access. */
					if (System.IO.Directory.Exists(filename))
						{
							foreach(string file in System.IO.Directory.GetFiles(filename, "*.xml"))
								{
									_loadFile(file);
								}
						}
// 				}
		}
		
		public void load(string filename)
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
			System.Data.DataTable raw = new System.Data.DataTable("tmp");
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
							int csID = System.Int32.Parse(parts[0]);
							
							ChangesetIdx.iterator it = _changesetIdx.find(csID);
							if (it != _changesetIdx.end())
								{
									Revision rev = it.value();
									
									if (parts[1] != "0")
										{
											rev.addParent(parts[1]);
										}
								}
							else
								{
									Revision r = new Revision(parts[0], parts[2], parts[3], parts[4], string.Empty);
									
									if (parts[1] != "0") { r.addParent(parts[1]); }
									
									_changesetIdx.insert(csID, r);
									
									BranchChangesets.iterator bit = _branchChangesets.find(r.Branch);
									
									if (bit != _branchChangesets.end())
										{
											/* well, we've already checked the changeset list,
											 * unless we've screwed up somewhere, this should be ok.
											 */
											bit.value().insert(csID, r);
										}
									else
										{
											ChangesetIdx cngsidx = new ChangesetIdx();
											cngsidx.insert(csID, r);
											_branchChangesets.insert(r.Branch, cngsidx);
											
											_branches.insert(r.Branch);
										}
								}
						}
				}
		}
		
		private void _loadFile(string filename)
		{
			ChangesetCont changesets = new ChangesetCont();
			
			using (Stream r =new FileStream(filename, FileMode.Open, FileAccess.Read))
				{
					XmlSerializer s = new XmlSerializer(typeof(ChangesetLoader.Changeset));
					ChangesetLoader.Changeset cs = null;
					
					do {
						cs = _processChunk(r, s);
						if (cs != null)
							{
								changesets.insert(cs);
							}
					}	while( cs != null);
						
					r.Close();
				}
			
			_load(changesets);
		}
		
		private ChangesetLoader.Changeset _processChunk(Stream r, XmlSerializer s)
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
			ChangesetLoader.Changeset c = null;
			
			if (strm.Length > 0)
				{
					strm.Seek(0, System.IO.SeekOrigin.Begin);
					c = s.Deserialize(strm) as ChangesetLoader.Changeset;
				}
			
			return c;
		}
		
		private void _load(ChangesetCont changesets)
		{
			/* walk each changeset. */
			for(ChangesetCont.iterator it = changesets.begin();
					it != changesets.end();
					++it) 
				{
					/* get the list of branches for this changeset. */
					it.item().buildBranches();
					
					/* add branches to the master list of branches. */
					foreach(string b1 in it.item().Branches)
						{
							string branch = b1;
							treelib.AVLDict<string,string, treelib.StringSorterInsensitive>.iterator bmit =
								_branchMap.find(branch);
							
							if (bmit != _branchMap.end()) { branch = bmit.value(); }
							
							/* add the changeset to the proper branch. */
							BranchChangesets.iterator bit = _branchChangesets.find(branch);
							Revision rev = 
								new Revision(it.item().ID,
														 branch,
														 it.item().Author,
														 it.item().Date,
														 it.item().Comment);
							
							if (_changesetIdx.end() == _changesetIdx.find(it.item().ID))
								{
									/* add the changeset to the changeset index. */
									_changesetIdx.insert(it.item().ID, rev);
								}
							
							if (bit != _branchChangesets.end())
								{
									if (bit.value().find(it.item().ID) == bit.value().end())
										{ bit.value().insert(it.item().ID, rev); }
								}
							else
								{
									ChangesetIdx branchIdx = new ChangesetIdx();
									
									branchIdx.insert(it.item().ID, rev);
									_branchChangesets.insert(branch, branchIdx);
								}
							
							/* add the branch to the list of available branches. */
							if (_branches.find(branch) == _branches.end()) { _branches.insert(branch); } 
						}
				}
			
			/* now we need to populate the parents. */
			for(BranchChangesets.iterator bcit = _branchChangesets.begin();
					bcit != _branchChangesets.end();
					++bcit)
				{
					Revision prev = null;
					
					for(ChangesetIdx.iterator csit = bcit.value().begin();
							csit != bcit.value().end();
							++csit)
						{
							/* this should list everything from largest to smallest
							 * 
							 * so largest has a parent of the next item in the list.
							 */
							if (prev != null) 
								{
									prev.addParent(csit.value().ID);
								}
							
							prev = csit.value();
						}
				}
		}
	}
}
