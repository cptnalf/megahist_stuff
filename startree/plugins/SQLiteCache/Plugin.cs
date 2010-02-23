
using System.Windows.Forms;
using System.Linq;

namespace StarTree.Plugin.SQLiteCache
{
	using Snapshot = StarTree.Plugin.Database.Snapshot;
	using Revision = StarTree.Plugin.Database.Revision;
	using DisplayNames = StarTree.Plugin.Database.DisplayNames;
	
	[System.AddIn.AddIn("SQLiteCache", Description="sqlite 3 database support")]
	public class PluginInterface : StarTree.Plugin.Database.Plugin
	{
		private SQLiteStorage.SQLiteCache _db;
		private DisplayNames _dn;
		
		public PluginInterface()
		{
			_dn = new DisplayNames
				{
					name = "SQLiteCache",
					id = "id",
					parent = "parent",
					author = "User",
					log = "Comment",
					date = "date",
				};
		}
		
		public override void open()
		{
			OpenFileDialog ofd = new OpenFileDialog();
			
			ofd.Title = "Open a sqlite revision database";
			ofd.Filter = "SQLite DBs (*.db)|*.db|All files (*.*)|*.*";
			ofd.Multiselect = false;
			ofd.CheckFileExists = false;
			ofd.AutoUpgradeEnabled = true;
			
			DialogResult result = ofd.ShowDialog();
			if (result == DialogResult.OK)
				{
					_db = new SQLiteStorage.SQLiteCache();
					_db.load(ofd.FileName);
				}
		}

		public override void close()
		{
		
		}
		
		public override string  currentName { get { return _db.Name; } }
		public override DisplayNames  names { get { return _dn; } }
		
		public override string[]  branches()
		{
			return _db.BranchNames.ToArray();
		}
		
		public override Snapshot getBranch(string branch, long limit)
		{
			return _db.getBranch(branch, (ulong)limit);
		}

		public override Revision getRevision(string id)
		{
			return _db.rev(id);
		}
	}
}
