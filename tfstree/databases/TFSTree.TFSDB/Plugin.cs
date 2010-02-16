﻿

using System.Windows.Forms;

namespace TFSTree.Databases.TFSDB
{
	public class PluginInterface : IDBPlugin
	{
	#region IDBPlugin Members

		public string internalName { get { return this.GetType().FullName; } }

		public string Name { get { return "TFS"; } }
		
		public string IDName { get { return "Changeset"; } }
		public string ParentName { get { return "Parent"; } }
		public string AuthorName { get { return "Committer"; } }
		public string LogName    { get { return "Comment"; } }
		
		public IRevisionRepo open()
		{
			TFSDB db = null;
			
			TFSServersSelectorForm form = new TFSServersSelectorForm();
			DialogResult result = form.ShowDialog();
			if (result == DialogResult.OK)
				{
					string serverName = form.getSelection();
					db = new TFSDB();
					db.load(serverName);
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
