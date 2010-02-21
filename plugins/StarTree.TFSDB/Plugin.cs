

using System.Linq;
using System.Windows.Forms;

namespace StarTree.Plugin.TFSDB
{
	using DisplayNames = StarTree.Plugin.Database.DisplayNames;
	using Revision = StarTree.Plugin.Database.Revision;
	using Snapshot = StarTree.Plugin.Database.Snapshot;
	
	[System.AddIn.AddIn("TFSDB", Description="plugin for TFS")]
	public class PluginInterface : StarTree.Plugin.Database.Plugin
	{
		private DisplayNames _dn;
		private TFSDB _db;
		
		public PluginInterface()
		{
			_dn = new DisplayNames
				{
					name = "tfs",
					id = "Changeset",
					author = "Committer",
					date = "CreationDate",
					log = "Comment",
					parent = "Parent",
				};
		}
				
		public override void open()
		{
			TFSServersSelectorForm form = new TFSServersSelectorForm();
			DialogResult result = form.ShowDialog();
			if (result == DialogResult.OK)
				{
					string serverName = form.getSelection();
					_db = new TFSDB();
					_db.load(serverName);
				}
		}

		public override void close()
		{
		
		}

		public override string currentName { get { return _db.Name; } }
		public override DisplayNames names { get { return _dn; } }

		public override string[] branches()
		{
			return _db.BranchNames.ToArray();
		}

		public override Snapshot getBranch(string branch, long limit)
		{
			return _db.getBranch(branch, (ulong)limit);
		}
	}
}
