using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BranchDifferGui
{
	public partial class Form1 : Form
	{
		static Microsoft.TeamFoundation.VersionControl.Client.VersionControlServer _get_tfs_server(string serverName)
		{
			Microsoft.TeamFoundation.Client.TeamFoundationServer srvr;

			if (serverName != null && serverName != string.Empty)
			{
				srvr = Microsoft.TeamFoundation.Client.TeamFoundationServerFactory.GetServer(serverName);
			}
			else
			{
				/* hmm, they didn't specify one, so get the first in the list. */
				Microsoft.TeamFoundation.Client.TeamFoundationServer[] servers =
					Microsoft.TeamFoundation.Client.RegisteredServers.GetServers();

				srvr = servers[0];
			}

			return (srvr.GetService(typeof(Microsoft.TeamFoundation.VersionControl.Client.VersionControlServer)) as Microsoft.TeamFoundation.VersionControl.Client.VersionControlServer);
		}
		
		public Form1()
		{
			InitializeComponent();
			treeListView1.CanExpandGetter = branch_order.TreeItem.HasChildren;
			treeListView1.ChildrenGetter = branch_order.TreeItem.ChildrenGetter;
		}

		private void _pathSel_Click(object sender, EventArgs e)
		{
			tfs_fullhistory.PathSelector sel = new tfs_fullhistory.PathSelector();
			sel.setVCS(_get_tfs_server("rnoengtfs"));
			if (sel.ShowDialog() == DialogResult.OK)
			{
				textBox1.Text = sel.getSelection();
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			branch_order bo = new branch_order();
			branch_order.TreeItem root = null;
			branch_order.Options options;
			options.src_path = textBox1.Text;
			options.src_id = 0;
			options.server = _get_tfs_server("rnoengtfs");
			
			root = bo.get_branches(options);
			
			treeListView1.ClearObjects();
			treeListView1.AddObject(root);
			treeListView1.ExpandAll();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (treeListView1.SelectedIndices.Count > 1)
				{
					branch_order.TreeItem t1 = treeListView1.GetModelObject(treeListView1.SelectedIndices[0]) as branch_order.TreeItem;
					branch_order.TreeItem t2 = treeListView1.GetModelObject(treeListView1.SelectedIndices[1]) as branch_order.TreeItem;
										
					Microsoft.TeamFoundation.VersionControl.Client.VersionControlServer vcs = _get_tfs_server("rnoengtfs");
					
					megahistory.Utils.VisualDiff(t1.FullPath, t2.FullPath, vcs);
				}
		}

		private void historyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (treeListView1.SelectedIndices.Count > 0)
			{
				Microsoft.TeamFoundation.VersionControl.Client.VersionControlServer vcs = _get_tfs_server("rnoengtfs");
				
				foreach(int idx in treeListView1.SelectedIndices)
				{
					HistoryViewerForm hist = new HistoryViewerForm();
					branch_order.TreeItem item = treeListView1.GetModelObject(idx) as branch_order.TreeItem;
					
					hist.setInfos(vcs, item.FullPath, (int)numericUpDown1.Value);
					hist.Show();
				}
			}
		}
	}
}
