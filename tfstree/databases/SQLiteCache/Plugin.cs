
using System.Windows.Forms;

namespace TFSTree.Databases.SQLiteCache
{
	public class PluginInterface : IDBPlugin
	{
	#region IDBPlugin Members

		public string internalName { get { return this.GetType().FullName; } }

		public string Name { get { return "SQLite DB"; } }
		public string IDName { get { return "id"; } }
		public string ParentName { get { return "parent"; } }
		public string AuthorName { get { return "User"; } }
		public string LogName    { get { return "Comment"; } }
		
		public IRevisionRepo open()
		{
			IRevisionRepo database = null;
			
			OpenFileDialog ofd = new OpenFileDialog();
			
			ofd.Title = "Open a sqlite revision database";
			ofd.Filter = "SQLite DBs (*.db)|*.db|All files (*.*)|*.*";
			ofd.Multiselect = false;
			ofd.CheckFileExists = false;
			ofd.AutoUpgradeEnabled = true;
			
			DialogResult result = ofd.ShowDialog();
			if (result == DialogResult.OK)
				{
					SQLiteCache db = new SQLiteCache();
					db.load(ofd.FileName);
					database = db;
				}
			
			return database;
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
