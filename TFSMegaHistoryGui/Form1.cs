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
		private BackgroundWorker _worker = new BackgroundWorker();
		private BackgroundWorker _wiWorker = new BackgroundWorker();
		private BackgroundJobs.GetWorkItemData _getWIData = null;
		private string _tfsServerName = "rnoengtfs";
		
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
			
			_setupTreeListView();
		}
		
		private void _setupTreeListView()
		{
			treeListView1.HotItemStyle = new BrightIdeasSoftware.HotItemStyle();
			treeListView1.HotItemStyle.BackColor = Color.CadetBlue;
			treeListView1.HotItemStyle.ForeColor = Color.Black;
			treeListView1.HotItemStyle.FontStyle = FontStyle.Underline;
			//treeListView1.HotTracking = true;
			
			treeListView1.ShowItemToolTips = true;
			treeListView1.CellToolTip.InitialDelay = 2000;
			treeListView1.CellToolTip.ReshowDelay = 4000;
			treeListView1.CellToolTip.IsBalloon = false;
			treeListView1.CellToolTip.BackColor = System.Drawing.SystemColors.Info;
			treeListView1.CellToolTip.BackColor = System.Drawing.SystemColors.InfoText;
			
			{
				treeListView1.EmptyListMsg = "No Changesets Found.";
				BrightIdeasSoftware.TextOverlay textOverlay =
					treeListView1.EmptyListMsgOverlay as BrightIdeasSoftware.TextOverlay;
				textOverlay.TextColor = Color.Firebrick;
				textOverlay.BackColor = Color.MistyRose;
				textOverlay.BorderColor = Color.DarkRed;
				textOverlay.BorderWidth = 4.0f;
				textOverlay.Font = new Font("Tahoma", 26);
				textOverlay.Rotation = -5;
			}
			
			_branch.AspectGetter = _aspectGetter;

			BrightIdeasSoftware.TypedObjectListView<megahistory.Visitor.PatchInfo> patches = new BrightIdeasSoftware.TypedObjectListView<megahistory.Visitor.PatchInfo>(treeListView1);
			patches.GenerateAspectGetters();
			
			treeListView1.CanExpandGetter = _canExpandGetter;
			treeListView1.ChildrenGetter = _childrenGetter;
		}

		/** *****************************************
		 *  treeview handlers.
		 */
		private string _aspectGetter(object model)
		{
			megahistory.Visitor.PatchInfo p = model as megahistory.Visitor.PatchInfo;
			string str = string.Empty;
			
			if (p != null)
				{
					if (p.treeBranches != null && p.treeBranches.Count > 0)
						{
							str = p.treeBranches[0];
						}
				}
			return str;
		}
		
		private bool _canExpandGetter(object o)
		{
			bool result = false;
			megahistory.Visitor.PatchInfo patch = o as megahistory.Visitor.PatchInfo;
			if (patch != null) { result = patch.partCount > 0; }
			return result;
		}
		
		private System.Collections.IEnumerable _childrenGetter(object model)
		{
			List<megahistory.Visitor.PatchInfo> patches = null;
			megahistory.Visitor.PatchInfo patch = model as megahistory.Visitor.PatchInfo;
			if (patch != null) { patches = patch.parts; }
			else { patches = new List<megahistory.Visitor.PatchInfo>(); }
			
			return patches;
		}
		/* ***************************************** */
		
		
		/** cleanup after the megahistory querier is done doing it's thing.
		 */
		private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (treeListView1.InvokeRequired)
				{ treeListView1.BeginInvoke(new RunWorkerCompletedEventHandler(_worker_RunWorkerCompleted)); }
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
		
		/** fixup the progressbar.
		 */
		private void _worker_ProgressChanged(object sender, ProgressChangedEventArgs args)
		{
			if (treeListView1.InvokeRequired)
				{ treeListView1.BeginInvoke(new ProgressChangedEventHandler(_worker_ProgressChanged)); }
			else
				{
					megahistory.Visitor.PatchInfo commit = args.UserState as megahistory.Visitor.PatchInfo;
					
					if (commit != null)
						{
							treeListView1.AddObject(commit);
							treeListView1.RefreshObject(commit);
						}
				}
		}
		
		/** start the megahistory querier.
		 */
		private void _worker_DoWork(object sender, DoWorkEventArgs e)
		{
			long start_ticks = DateTime.Now.Ticks;
			string tfspath;
			string changeset;
			int maxchanges = 10;
			bool branchesToo = false;
			VersionControlServer vcs = megahistory.Utils.GetTFSServer(_tfsServerName);
			object[] args = e.Argument as object[];
			megahistory.MegaHistory.Options options = new megahistory.MegaHistory.Options();
			HistoryCollector visitor = new HistoryCollector();
			visitor.Worker = _worker;
			
			tfspath = args[0] as string;
			changeset =args[1] as string;
			maxchanges = (int)args[2];
			branchesToo = (bool)args[6];
			
			options.NoRecurse = (bool)args[3];
			options.AllowBranchRevisiting = (bool)args[4];
			options.ForceDecomposition = (bool)args[5];
			if (branchesToo)
				{
					megahistory.MegaHistory.IsChangeToConsider = 
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
			
			VersionSpec ver = null;
			
			if (changeset == string.Empty)
				{
					ver = VersionSpec.Latest;
				}
			else
				{
					ver = new ChangesetVersionSpec(changeset);
				}
			
			megahistory.MegaHistory mh = new megahistory.MegaHistory(options, vcs, visitor);
			
			mh.visit(tfspath, ver, maxchanges);
			
			{
				long end_ticks = DateTime.Now.Ticks;
				e.Result = end_ticks - start_ticks;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			/* alrighty... 
			 * take args and push them to the megahistory connectors.
			 */
			
			progressBar1.Style = ProgressBarStyle.Marquee;
			progressBar1.MarqueeAnimationSpeed = 10;
			treeListView1.ClearObjects();
			
			button1.Enabled = false;
			int maxChanges = (int)_maxChanges.Value;
			object[] args = 
				new object[] 
				{ 
					_tfsPath.Text, 
					_changeset.Text, 
					maxChanges, 
					_noRecurse.Checked,
					_allowBranchRevisiting.Checked,
					_forceDecomposition.Checked,
					_branchesToo.Checked
				};
			
			_worker.RunWorkerAsync(args);
		}
		
		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{

		}
		
		private void  _getWIData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
 			_getWIData = null;
		}
		
		private void button2_Click(object sender, EventArgs e)
		{
			PathSelector ps = new PathSelector();
			VersionControlServer vcs = megahistory.Utils.GetTFSServer(_tfsServerName);
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
																						 "Verifying really really bad idea", 
																						 MessageBoxButtons.YesNoCancel, 
																						 MessageBoxIcon.Warning);
					if (res != DialogResult.Yes) { _allowBranchRevisiting.CheckState = CheckState.Unchecked; }
			  }
		}
		
		private void treeListView1_CellToolTipShowing(object sender, 
																									BrightIdeasSoftware.ToolTipShowingEventArgs e)
		{
			megahistory.Visitor.PatchInfo patch = e.Model as megahistory.Visitor.PatchInfo;
			
			if (patch != null)
				{
					e.Title = string.Format("Changeset {0}", patch.id);
					e.Text = string.Format("Parent {0}\r\nBranch {1}\r\nCreated {2}\r\nCreated By {3}\r\n{4}",
					                       patch.parent, 
																 string.Empty, patch.user, patch.creationDate, patch.comment);
				}
		}

		private void toolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if (treeListView1.SelectedIndices.Count > 1)
				{
					megahistory.Visitor.PatchInfo p1 = treeListView1.GetModelObject(treeListView1.SelectedIndices[0]) as megahistory.Visitor.PatchInfo;
					megahistory.Visitor.PatchInfo p2 = treeListView1.GetModelObject(treeListView1.SelectedIndices[1]) as megahistory.Visitor.PatchInfo;
					
					SortedDictionary<string, pair<int,int>> paths = 
						new SortedDictionary<string, pair<int,int>>();
					System.Text.RegularExpressions.Regex egs_re = 
						new System.Text.RegularExpressions.Regex("(.+)/EGS/(.*)", 
											System.Text.RegularExpressions.RegexOptions.IgnoreCase | 
											System.Text.RegularExpressions.RegexOptions.Compiled);
					
					Change[] changes = p1.cs.Changes;
					int cngCount = changes.Length;
					for(int i =0; i < cngCount; ++i)
						{
							string path;
							System.Text.RegularExpressions.Match match = egs_re.Match(changes[i].Item.ServerItem);
							
							if (match != null)
								{
									path = match.Groups[2].Value;
									paths.Add(path, new pair<int,int>(i, -1));
								}
						}
					
					changes = p2.cs.Changes;
					cngCount = changes.Length;
					for(int i=0; i < changes.Length; ++i)
						{
							string path;
							System.Text.RegularExpressions.Match match = egs_re.Match(changes[i].Item.ServerItem);
							
							if (match != null)
								{
									path = match.Groups[2].Value;
									if (paths.ContainsKey(path)) { paths[path].second = i; }
								}
						}
					/* well, crap. now i need to remove all of the paths which have a -1 for the second item. */
					List<string> removes = new List<string>();
					
					foreach(KeyValuePair<string,pair<int,int>> item in paths)
						{ if (item.Value.second == -1) { removes.Add(item.Key); } }
					
					foreach(string key in removes) { paths.Remove(key); }
					
					if (paths.Count > 0)
						{
							FileSelectionForm fsf = new FileSelectionForm();
							fsf.Paths = paths;
							if (fsf.ShowDialog() == DialogResult.OK)
								{
									string key = fsf.SelectedPath;
									pair<int,int> items = paths[key];
									
									megahistory.Utils.VisualDiff(p1, p2, items);
								}
						}
					else { MessageBox.Show("No files in common between the selected changesets."); }
				}
		}

		private void treeListView1_SelectionChanged(object sender, EventArgs e)
		{
			changesetCtrl1.reset();
			
			if (treeListView1.SelectedIndices.Count > 0 && treeListView1.SelectedIndices.Count < 2)
				{
					/* only one item is selected... */
					megahistory.Visitor.PatchInfo p = treeListView1.GetModelObject(treeListView1.SelectedIndex) as megahistory.Visitor.PatchInfo;

					changesetCtrl1.ID = p.cs.ChangesetId;
					changesetCtrl1.Owner = p.cs.Owner;
					changesetCtrl1.Comment = p.cs.Comment;
					changesetCtrl1.CreationDate = p.cs.CreationDate;
					
					if (p.cs.Changes.Length < _changeCntNUD.Value)
						{
							/* do the whole thing here. */
							tables.Changes cngs = new tfs_fullhistory.tables.Changes();

							foreach (Change cng in p.cs.Changes) { cngs.add(cng); }
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
		}
	}
}