
using System.Windows.Forms;
using System.Linq;

namespace Monotree
{
	using Revision = StarTree.Plugin.Database.Revision;
	using Snapshot = StarTree.Plugin.Database.Snapshot;
	using DisplayNames = StarTree.Plugin.Database.DisplayNames;
	
	[System.AddIn.AddIn("monotone", 
	  Description="monotone database plugin, reads 0.38 databases", 
	  Version="1.0.0.0")]
	public class PluginInterface : StarTree.Plugin.Database.Plugin
	{
		private OpenFileDialog ofd;
		private DisplayNames _dn;
		private Database _db;
		
		public PluginInterface()
		{
			ofd = new OpenFileDialog();
			ofd.Filter = "Monotone db (*.mtn)|*.mtn|All files (*.*)|*.*";
			ofd.Title = "Open a monotone database";
			ofd.AutoUpgradeEnabled = true;
			ofd.CheckFileExists = true;
			ofd.SupportMultiDottedExtensions = true;

			_dn = new DisplayNames();
			_dn.name = "Monotone";
			_dn.author = "Author";
			_dn.date = "Date";
			_dn.id = "Revision";
			_dn.log = "ChangeLog";
			_dn.parent = "Ancester";
		}
		
		public override string currentName
		{
			get
				{
					if (_db != null ) { return _db.Name; }
					
					return null;
				}
		}
		
		public override DisplayNames names { get { return _dn; } }
		
		public override void open()
		{
			DialogResult result = ofd.ShowDialog();
			if (result == DialogResult.OK)
				{
					_db = new Database(ofd.FileName);
				}
		}

		public override void close()
		{
		
		}

		public override string[] branches()
		{
			return _db.BranchNames.ToArray();
		}

		public override Snapshot getBranch(string branch, long limit)
		{
			return _db.getBranch(branch, (ulong)limit);
		}

		public override Revision getRevision(string id) { return _db.rev(id); }
	}
}
