﻿using System;
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
		private Microsoft.TeamFoundation.VersionControl.Client.VersionControlServer _vcs;
		private megahistory.TFSWorkspaces _workspaces;
		
		public Form1()
		{
			InitializeComponent();
			treeListView1.CanExpandGetter = branch_order.TreeItem.HasChildren;
			treeListView1.ChildrenGetter = branch_order.TreeItem.ChildrenGetter;
			
			_vcs = megahistory.Utils.GetTFSServer("rnoengtfs");
			_workspaces = new megahistory.TFSWorkspaces(_vcs);
			
			List<string> workspace_names = new List<string>();
			foreach(Microsoft.TeamFoundation.VersionControl.Client.Workspace w in _workspaces.Workspaces)
			{ workspace_names.Add(w.DisplayName); }
			
			_workspacesCB.DataSource = workspace_names;
		}

		private void _pathSel_Click(object sender, EventArgs e)
		{
			tfs_fullhistory.PathSelector sel = new tfs_fullhistory.PathSelector();
			sel.setVCS(megahistory.Utils.GetTFSServer("rnoengtfs"));
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
			options.server = megahistory.Utils.GetTFSServer("rnoengtfs");
			
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
					
					/* @todo i need to fix the redundant nature of 'treeitem' and patchinfo ... */
					string left, right;
					
					left = t1.FullPath;
					right = t2.FullPath;
					
					if (_useLocals.Checked)
					{
						left = _workspaces.getLocalPath((string)_workspacesCB.SelectedItem, t1.FullPath);
						right = _workspaces.getLocalPath((string)_workspacesCB.SelectedItem, t2.FullPath);
					}
					
					megahistory.Utils.VisualDiff(left, right, _vcs);
				}
		}

		private void historyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (treeListView1.SelectedIndices.Count > 0)
			{
				Microsoft.TeamFoundation.VersionControl.Client.VersionControlServer vcs = megahistory.Utils.GetTFSServer("rnoengtfs");
				
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
