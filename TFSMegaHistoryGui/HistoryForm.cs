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
	public partial class HistoryForm : Form
	{
		private BackgroundWorker _worker;
		private string _tfsPath;
		private int _maxChanges;
		private bool _branchesToo;
		private megahistory.MegaHistory.Options _options;
		private VersionControlServer _vcs;
		
		public HistoryForm()
		{
			InitializeComponent();
			_setupTreeListView();
		}
		
		public void setPath(VersionControlServer vcs,
												string tfsPath, int maxChanges, bool branchesToo, 
												megahistory.MegaHistory.Options options)
		{
			_vcs = vcs;
			_tfsPath = tfsPath;
			_maxChanges = maxChanges;
			_branchesToo = branchesToo;
			_options = options;
		}
		
		private void _setupTreeListView()
		{
			treeListView1.SmallImageList = new ImageList();
			treeListView1.SmallImageList.Images.Add(Properties.Resources.changeset);
			treeListView1.SmallImageList.ImageSize = new Size(16, 16);
 
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

		private void HistoryForm_Load(object sender, EventArgs e)
		{
			_worker = new BackgroundWorker();
			/* alrighty... 
			 * take args and push them to the megahistory connectors.
			 */
			
			// progressBar1.Style = ProgressBarStyle.Marquee;
			// progressBar1.MarqueeAnimationSpeed = 10;
			treeListView1.ClearObjects();
			
			_worker.RunWorkerAsync();
		}


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
					// button1.Enabled = true;
					// progressBar1.Style = ProgressBarStyle.Blocks;
					TimeSpan t = new TimeSpan(delta);
					// _time.Text = t.ToString();
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
			HistoryCollector visitor = new HistoryCollector();
			visitor.Worker = _worker;
						
			if (_branchesToo)
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
			
			/* @TODO if/when i add deleted-item support, 
			 * i'll need the changeset on the item id?
			 */
			VersionSpec ver = VersionSpec.Latest;
			Item item = _vcs.GetItem(_tfsPath, ver);
			
			if (item.ItemType == ItemType.File)
				{
					megahistorylib.ItemHistory ih =
						new megahistorylib.ItemHistory(_vcs, visitor);
					
					ih.visit(_tfsPath, ver, _maxChanges);
				}
			else
				{
					megahistory.MegaHistory mh = new megahistory.MegaHistory(_options, _vcs, visitor);

					mh.visit(_tfsPath, ver, _maxChanges);
				}
			
			{
				long end_ticks = DateTime.Now.Ticks;
				e.Result = end_ticks - start_ticks;
			}
		}

	}
}
