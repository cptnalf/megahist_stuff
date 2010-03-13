using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tfs_fullhistory
{
	using VersionControlServer = Microsoft.TeamFoundation.VersionControl.Client.VersionControlServer;
	using VersionSpec = Microsoft.TeamFoundation.VersionControl.Client.VersionSpec;
	using ChangesetVersionSpec = Microsoft.TeamFoundation.VersionControl.Client.ChangesetVersionSpec;
	using BranchItemCont = treelib.TreapDict<int, Microsoft.TeamFoundation.VersionControl.Client.Item>;
	using ItemSet = Microsoft.TeamFoundation.VersionControl.Client.ItemSet;
	using RecursionType = Microsoft.TeamFoundation.VersionControl.Client.RecursionType;
	using DeletedState = Microsoft.TeamFoundation.VersionControl.Client.DeletedState;
	using ItemType = Microsoft.TeamFoundation.VersionControl.Client.ItemType;
	using Item = Microsoft.TeamFoundation.VersionControl.Client.Item;
	
	public partial class MainForm : Form
	{
		internal class BranchItem
		{
			private Item _itm;
			internal BranchItem(Item itm) { _itm = itm; }
			
			internal Item item { get { return _itm; } }
			public override string ToString() { return _itm.ServerItem; }
		}

		internal class fooNode : TreeNode { }

		private VersionControlServer _vcs = null;
		private ImageList _imgLst = new ImageList();
		
		public MainForm()
		{
			InitializeComponent();
			_imgLst.Images.Add(Properties.Resources.changeset);
			_imgLst.Images.Add(Properties.Resources.deleted);
			
			_tfsTree.ImageList = _imgLst;
			_tfsTree.BeforeExpand += treeView1_BeforeExpand;
			_tfsTree.PathSeparator = "/";
			_tfsTree.ContextMenuStrip = this.contextMenuStrip1;
			
			this.Icon = Properties.Resources.ico21;
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			_vcs = tfsinterface.SCMUtils.GetTFSServer(Properties.Settings.Default.TFSServer);
			
			VersionSpec ver = VersionSpec.Latest;
			List<BranchItem> items = new List<BranchItem>();
			
			BranchItemCont branches = tfsinterface.SCMUtils.GetBranches(_vcs, "$/IGT_0803/main/EGS/", ver);
			
			BranchItemCont.iterator it = branches.begin();
			for(; it != branches.end(); ++it)
				{
					items.Add( new BranchItem(it.value()));
				}
			_branchesCB.DataSource = items;
		}

		private void _goBtn_Click(object sender, EventArgs e)
		{
			_tfsTree.Nodes.Clear();
			
			if (_branchesCB.SelectedItem != null)
				{
					BranchItem branch = _branchesCB.SelectedItem as BranchItem;
					
					if (branch != null)
						{
							TreeNode node = _tfsTree.Nodes.Add(branch.item.ServerItem);
							node.Tag = branch.item;
							node.Nodes.Add(new fooNode());
						}
				}
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
			System.Text.RegularExpressions.Regex root_re = 
			  new System.Text.RegularExpressions.Regex("\\" + root.FullPath, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
			  
			foreach (Item item in items.Items)
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

							if (path != string.Empty)
								{
									TreeNode node = root.Nodes.Add(path);
									node.Tag = item;
									
									if (item.DeletionId > 0) { node.ImageIndex = 1; }
									else { node.ImageIndex = 0; }
									node.Nodes.Add(new fooNode());
								}
						}
				}
		}

		private void historyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_tfsTree.SelectedNode != null)
				{
					Item item = _tfsTree.SelectedNode.Tag as Item;
					
					if (item != null)
						{
							HistoryForm hf = new HistoryForm();
							hf.setPath(Properties.Settings.Default.TFSServer, item, (int)_limitN.Value);
							hf.Show();
						}
				}
		}

		private void megahistoryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_tfsTree.SelectedNode != null)
				{
					Item item = _tfsTree.SelectedNode.Tag as Item;
					
					if (item != null)
						{
							HistoryForm hf = new HistoryForm();
							hf.setPath(Properties.Settings.Default.TFSServer, item, (int)_limitN.Value, (int)_distN.Value);
							
							hf.Show();
						}
				}
		}
	}
}
