
using System.Windows.Forms;

namespace Git.Database
{
	public class PluginInterface : TFSTree.Databases.IDBPlugin
	{
		private FolderBrowserDialog fbd = new FolderBrowserDialog();
		
		public PluginInterface()
		{
			fbd.Description = "Folder containing the git repository, or container for the repository";
			fbd.ShowNewFolderButton = false;
		}
	#region IDBPlugin Members
		
		public string internalName { get { return this.GetType().FullName; } }
		
		public string Name { get { return "git"; } }

		public TFSTree.Databases.IRevisionRepo open()
		{
			Database db = null;
			
			DialogResult result = fbd.ShowDialog();
			if (result == DialogResult.OK)
				{
					db = new Database(@"c:\programs\git\bin\git.exe", fbd.SelectedPath);
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
