
using System.Windows.Forms;

namespace Monotree
{
	public class PluginInterface : TFSTree.Databases.IDBPlugin
	{
	#region IDBPlugin Members
		private OpenFileDialog ofd;

		public string internalName { get { return this.GetType().FullName; } }

		public string Name { get { return "Monotone"; } }

		public TFSTree.Databases.IRevisionRepo open()
		{
			Database db = null;
			
			if (ofd == null)
				{
					ofd = new OpenFileDialog();
					ofd.AutoUpgradeEnabled = true;
					ofd.CheckFileExists = true;
					ofd.Filter = "Monotone db (*.mtn)|*.mtn|All files (*.*)|*.*";
					ofd.Title = "Open a monotone database";
					ofd.SupportMultiDottedExtensions = true;
				}
			
			DialogResult result = ofd.ShowDialog();
			if (result == DialogResult.OK)
				{
					db = new Database(ofd.FileName);
				}
			
			return db;
		}

		public void init()
		{
		
		}

		public void close()
		{
		
		}

	#endregion
	}
}
