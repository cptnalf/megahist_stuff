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
		private bool _branchesToo;
		private int _maxChanges;
		private megahistory.MegaHistory.Options _options;
		
		public bool branchesToo { get { return _branchesToo; } set { _branchesToo = value; } }
		public int maxChanges { get { return _maxChanges; } set { _maxChanges = value; } }
		public megahistory.MegaHistory.Options options
		{ get { return _options; } set { _options = value; } }
	
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
			ItemSet items = _vcs.GetItems(root.FullPath, VersionSpec.Latest, RecursionType.OneLevel, DeletedState.Any, ItemType.Any);
			System.Text.RegularExpressions.Regex root_re = new System.Text.RegularExpressions.Regex("\\"+root.FullPath, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
			foreach(Item item in items.Items)
				{
					/* need to do a case-insentive replace */
					//string path = item.ServerItem.Replace(root.FullPath, string.Empty);
					string path = root_re.Replace(item.ServerItem, string.Empty);
					if (path.Length > 0)
						{
							if (path[0] == '/')
								{
									path = path.Substring(1);
								}
							
							if (path != string.Empty && item.DeletionId < 1)
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

		private void historyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			TreeNode node = treeView1.SelectedNode;
			
			if (node != null)
				{
					HistoryForm hf = new HistoryForm();
					hf.setPath(_vcs, node.FullPath, _maxChanges, _branchesToo, _options);
					hf.Show();
				}
		}
	}
}
