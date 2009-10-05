using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace tfs_fullhistory
{
	public partial class PathSelector : Form
	{
		class fooNode : TreeNode
		{
		};
		
		VersionControlServer _vcs;
	
		public void setVCS(VersionControlServer vcs) { _vcs=vcs; }
		public string getSelection() { return treeView1.SelectedNode.FullPath; }
		public PathSelector()
		{
			InitializeComponent();
		}
		
		private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			if ((e.Action & TreeViewAction.Expand) == TreeViewAction.Expand)
				{
					if (e.Node.Nodes.Count > 0)
						{
							bool needsExpand = false;
							
							needsExpand = e.Node.Nodes[0] is fooNode;
							
							if (needsExpand)
							{
								e.Node.Nodes.Clear();
								
								_process_items(e.Node);
							}
						}
				}
		}
		
		private void _process_items(TreeNode root)
		{
			ItemSet items = _vcs.GetItems(root.FullPath, VersionSpec.Latest, RecursionType.OneLevel);
			foreach(Item item in items.Items)
				{
					string path = item.ServerItem.Replace(root.FullPath, string.Empty);
					if (path.Length > 0)
						{
							if (path[0] == '/')
								{
									path = path.Substring(1);
								}
							
							if (path != string.Empty)
								{
									TreeNode node = root.Nodes.Add(path);
									node.Tag = item.ServerItem;
									node.Nodes.Add(new fooNode());
								}
						}
				}
		}

		private void PathSelector_Load(object sender, EventArgs e)
		{
			ItemSet items = _vcs.GetItems("$/IGT_0803", VersionSpec.Latest, RecursionType.OneLevel);
			
			TreeNode root = treeView1.Nodes.Add("$");
			root = root.Nodes.Add("IGT_0803");
			
			_process_items(root);
		}
	}
}
