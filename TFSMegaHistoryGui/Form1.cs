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
	public partial class Form1 : Form
	{
		static VersionControlServer _get_tfs_server(string serverName)
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
			
			return (srvr.GetService(typeof(VersionControlServer)) as VersionControlServer);
		}
		
		private BackgroundWorker _worker = new BackgroundWorker();
		private BackgroundWorker _wiWorker = new BackgroundWorker();
		private BackgroundJobs.GetWorkItemData _getWIData = null;
		
		public Form1()
		{
			InitializeComponent();
			
			_worker.WorkerReportsProgress = true;
			_worker.DoWork += new DoWorkEventHandler(_worker_DoWork);
			_worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_worker_RunWorkerCompleted);
			_worker.ProgressChanged += _worker_ProgressChanged;
			
			_tfsPath.AutoCompleteCustomSource = new AutoCompleteStringCollection();
			
			/* maybe replace this with something a little better, like a tfs query, or 
			 * implement the '...' button so you can browse for your file/directory.
			 */
			string[] dirs =	new string[] {
				"$/IGT_0803/main/EGS", 
				"$/IGT_0803/development/dev_advantage/EGS", 
				"$/IGT_0803/development/dev_ABS/EGS", 
				"$/IGT_0803/development/dev_AdvAsia/EGS",
				"$/IGT_0803/development/dev_AX/EGS", 
				"$/IGT_0803/development/dev_build/EGS", 
				"$/IGT_0803/development/dev_eft/EGS",
				"$/IGT_0803/development/dev_eft_83/EGS", 
				"$/IGT_0803/development/dev_MA/EGS", 
				"$/IGT_0803/development/dev_Mar/EGS",
				"$/IGT_0803/development/dev_mariposa/EGS",
				"$/IGT_0803/development/dev_tableID/EGS",
				"$/IGT_0803/development/dev_tableManager/EGS",
				"$/IGT_0803/development/dev_uInstall/EGS",
				"$/IGT_0803/release/EGS8.1/dev_sp/EGS",
				"$/IGT_0803/release/EGS8.1/SP0/RTM/EGS",
				"$/IGT_0803/release/EGS8.1/SP1/RTM/EGS",
				"$/IGT_0803/release/EGS8.1/SP2/RTM/EGS",
				"$/IGT_0803/release/EGS8.1/SP3/RTM/EGS",
				"$/IGT_0803/release/EGS8.2/SP0/RTM/EGS",
				"$/IGT_0803/release/EGS8.2/SP1/RTM/EGS",
				"$/IGT_0803/release/EGS8.2/dev_sp/EGS",
				"$/IGT_0803/release/EGS8.2/rel_devb/EGS",
				"$/IGT_0803/release/EGS8.2/SP2_hf/RTM/EGS"
			};
			
			foreach(string part in dirs) { _tfsPath.AutoCompleteCustomSource.Add(part); }
		}

		void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (treeView1.InvokeRequired)
				{ treeView1.BeginInvoke(new RunWorkerCompletedEventHandler(_worker_RunWorkerCompleted)); }
			else
				{
					if (e.Error != null)
						{
						}
					else
						{
						}
					
					long delta = (long)e.Result;
					button1.Enabled = true;
					progressBar1.Style = ProgressBarStyle.Blocks;
					TimeSpan t = new TimeSpan(delta);
					_time.Text = t.ToString();
				}
		}
		
		void _worker_ProgressChanged(object sender, ProgressChangedEventArgs args)
		{
			if (treeView1.InvokeRequired)
				{ treeView1.BeginInvoke(new ProgressChangedEventHandler(_worker_ProgressChanged)); }
			else
				{
					TreeNode node = args.UserState as TreeNode;
					
					if (node != null)
						{
							treeView1.Nodes.Add(node);
						}
				}
		}
		
		void _worker_DoWork(object sender, DoWorkEventArgs e)
		{
			long start_ticks = DateTime.Now.Ticks;
			string tfspath;
			string changeset;
			int maxchanges = 10;
			bool branchesToo = false;
			VersionControlServer vcs = _get_tfs_server("rnoengtfs");
			object[] args = e.Argument as object[];
			MegaHistory.Options options = new MegaHistory.Options();
			
			tfspath = args[0] as string;
			changeset =args[1] as string;
			maxchanges = (int)args[2];
			branchesToo = (bool)args[6];
			
			options.NoRecurse = (bool)args[3];
			options.AllowBranchRevisiting = (bool)args[4];
			options.ForceDecomposition = (bool)args[5];
			if (branchesToo)
				{
					MegaHistory.IsChangeToConsider = 
						delegate(Change cng)
						{
						return (
								(cng.ChangeType != ChangeType.Merge)  /* ignore only merges. */
								&&
								(
								 /* look for branched items, or merged items */
								 ((cng.ChangeType & ChangeType.Branch) == ChangeType.Branch) ||
								 ((cng.ChangeType & ChangeType.Merge) == ChangeType.Merge)
								 )
								);
						};
				}
			
			if (changeset == string.Empty)
				{
					/* list all of the history, then query ones which are merges. */
					System.Collections.IEnumerable foo = 
						vcs.QueryHistory(tfspath, VersionSpec.Latest,
														 0, RecursionType.Full, null, null, null, maxchanges, true, false, false);
					
					foreach (object o in foo)
						{
							Changeset cs = o as Changeset;
							bool isMerge = false;
							TreeNode node = null;
							
							foreach(Change cng in cs.Changes)
								{
									isMerge =
										((cng.ChangeType & ChangeType.Branch) == ChangeType.Branch
										 ||
										 (cng.ChangeType & ChangeType.Merge) == ChangeType.Merge);
									if (isMerge) { break; }
								}
							
							if (isMerge)
								{
									ChangesetVersionSpec ver = new ChangesetVersionSpec(cs.ChangesetId);
									
									node = _run_recursive_query(vcs, tfspath, ver, options);
									if (node == null) { node = new TreeNode(cs.ChangesetId.ToString()); }
								}
							else
								{
									node = new TreeNode(cs.ChangesetId.ToString());
									node.Tag = new Visitor.PatchInfo(0, cs, null);
								}
							
							if (node != null)
								{
									_worker.ReportProgress(99, node);
								}
						}
				}
			else
				{
					ChangesetVersionSpec ver = new ChangesetVersionSpec(changeset);
					TreeNode node = _run_recursive_query(vcs, tfspath, ver, options);
					
					if (node == null) { node = new TreeNode("no changesets found."); }
					_worker.ReportProgress(10, node);
				}
			
			{
				long end_ticks = DateTime.Now.Ticks;
				e.Result = end_ticks - start_ticks;
			}
		}
		
		private TreeNode _run_recursive_query(VersionControlServer vcs, string tfspath, ChangesetVersionSpec ver, MegaHistory.Options options)
		{
			HistoryCollector visitor = new HistoryCollector();
			MegaHistory history = new MegaHistory(options, vcs, visitor);
			bool result = history.visit(0, tfspath, ver, ver, ver);
			
			return (result ? visitor.Root : null);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			/* alrighty... 
			 * take args and push them to the megahistory connectors.
			 */
			
			progressBar1.Style = ProgressBarStyle.Marquee;
			progressBar1.MarqueeAnimationSpeed = 10;
			treeView1.Nodes.Clear();
			
			button1.Enabled = false;
			int maxChanges = (int)_maxChanges.Value;
			object[] args = new object[] 
			{ _tfsPath.Text, _changeset.Text, maxChanges, _noRecurse.Checked, _allowBranchRevisiting.Checked,
			  _forceDecomposition.Checked, _branchesToo.Checked };
			_worker.RunWorkerAsync(args);
		}
		
		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			Visitor.PatchInfo p = e.Node.Tag as Visitor.PatchInfo;
			
			changesetCtrl1.reset();
			changesetCtrl1.ID = p.cs.ChangesetId;
			changesetCtrl1.Owner = p.cs.Owner;
			changesetCtrl1.Comment = p.cs.Comment;
			changesetCtrl1.CreationDate = p.cs.CreationDate;
			
			if (p.cs.Changes.Length < 100)
				{
					/* do the whole thing here. */
					tables.Changes cngs = new tfs_fullhistory.tables.Changes();
					
					foreach(Change cng in p.cs.Changes) { cngs.add(cng); }
					changesetCtrl1.Changes = cngs;
				}
			else
				{
					/* background it. */
					tables.Changes cngs = new tfs_fullhistory.tables.Changes();
					cngs.add(string.Format("found {0} items, not listing them.", p.cs.Changes.Length), 
					         string.Empty, string.Empty);
					changesetCtrl1.Changes = cngs;
				}
			/* provide a way to view some of the changes. */
			
			/* just background this.
			 * i'm thinking this is the culprit in the long pauses, 
			 * (especially for short change lists)
			 */
			if (_getWIData == null)
			{
				_getWIData = new tfs_fullhistory.BackgroundJobs.GetWorkItemData(changesetCtrl1, p.cs);
				_getWIData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_getWIData_RunWorkerCompleted);
				_getWIData.RunWorkerAsync();
			}
		}
		
		private void  _getWIData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
 			_getWIData = null;
		}
		 void button2_Click(object sender, EventArgs e)
		{
			PathSelector ps = new PathSelector();
			VersionControlServer vcs = _get_tfs_server("rnoengtfs");
			ps.setVCS(vcs);
			ps.ShowDialog();
			_tfsPath.Text = ps.getSelection();
		}

		private void _allowBranchRevisiting_CheckStateChanged(object sender, EventArgs e)
		{
			if (_allowBranchRevisiting.CheckState == CheckState.Checked)
			  {
				/* tell the user they may have choosen poorly. */
				DialogResult res = MessageBox.Show("Are you really sure you want to do this?", 
				"Verifying really really bad idea", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
				if (res != DialogResult.Yes) { _allowBranchRevisiting.CheckState = CheckState.Unchecked; }
			  }
		}

	}
}