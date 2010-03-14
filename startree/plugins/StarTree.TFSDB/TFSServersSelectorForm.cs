using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StarTree.Plugin.TFS
{
	public partial class TFSServersSelectorForm : Form
	{
		public string getSelection()
		{
			string item = listBox1.SelectedItem as string;
			return item;
		}
		
		public TFSServersSelectorForm()
		{
			InitializeComponent();
		}

		private void TFSServersSelectorForm_Load(object sender, EventArgs e)
		{
			var servers = Microsoft.TeamFoundation.Client.RegisteredServers.GetServers();
			foreach(var server in servers)
				{
					listBox1.Items.Add(server.Name);
				}
		}
	}
}
