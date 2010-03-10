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
		private enum Mode
		{
			normal,
			mega,
		}
		
		private BackgroundWorker _worker;
		private int _maxChanges;
		private string _tfsServer;
		private int _distance;
		private Item _item;
		private Mode _mode;
		
		public HistoryForm()
		{
			InitializeComponent();
			_setupTreeListView();
		}
		
		public void setPath(string tfsServer,
												Item item, int maxChanges,
												int distance)
		{
			_tfsServer = tfsServer;
			_maxChanges = maxChanges;
			_distance = distance;
			_item = item;
			_mode = Mode.mega;
			
			this.Text = item.ServerItem;
		}
		
		public void setPath(string tfsServer, Item item, int maxChanges)
		{
			_mode = Mode.normal;
			_maxChanges = maxChanges;
			_item = item;
			this.Text = item.ServerItem;
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

			BrightIdeasSoftware.TypedObjectListView<Revision> patches = new BrightIdeasSoftware.TypedObjectListView<Revision>(treeListView1);
			patches.GenerateAspectGetters();

			treeListView1.CanExpandGetter = _canExpandGetter;
			treeListView1.ChildrenGetter = _childrenGetter;
		}

		/** *****************************************
		 *  treeview handlers.
		 */

		private bool _canExpandGetter(object o)
		{
			bool result = false;
			Revision patch = o as Revision;
			if (patch != null) { result = patch.hasParents(); }
			return result;
		}

		private System.Collections.IEnumerable _childrenGetter(object model)
		{
			Revision rev = model as Revision;
			return rev.getParents();
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
			
			
			if (_mode == Mode.mega)
				{
					_worker.DoWork += _worker_DoWork;
					_worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
				}
			else
				{
					_worker.DoWork += _worker_Normal;
					//_worker.RunWorkerCompleted += _worker_NormalCompleted;
					_worker.ProgressChanged += _worker_NormalProgChanged;
					_worker.WorkerReportsProgress = true;
				}
			
			_worker.RunWorkerAsync();
		}
		
		private void _worker_Normal(object sender, DoWorkEventArgs args)
		{
			VersionControlServer vcs =tfsinterface.SCMUtils.GetTFSServer(_tfsServer);
			VersionSpec ver = null;
			
			if (_item.DeletionId > 0) { ver = new ChangesetVersionSpec(_item.ChangesetId); }
			else { ver = VersionSpec.Latest; }
			
			System.Collections.IEnumerable foo = vcs.QueryHistory(_item.ServerItem, ver, _item.DeletionId, 
			  RecursionType.Full, null, null, null, _maxChanges, true, true, false);
			  
			foreach(object o in foo)
				{
					Changeset cs = o as Changeset;
					
					if (cs != null)
						{
							List<string> branches = tfsinterface.Utils.FindChangesetBranches(cs, (cng) => true);
							
							Revision rev = new Revision(branches[0], cs);
							_worker.ReportProgress(1, rev);
						}
				}
		}
		
		private void _worker_NormalProgChanged(object sender, ProgressChangedEventArgs args)
		{
			if (treeListView1.InvokeRequired)
				{ treeListView1.BeginInvoke(new ProgressChangedEventHandler(_worker_ProgressChanged)); }
			else
				{
					Revision rev = args.UserState as Revision;
			
					if (rev != null)
						{
							treeListView1.AddObject(rev);
							treeListView1.RefreshObject(rev);
						}
				}
		}
		
		/** cleanup after the megahistory querier is done doing it's thing.
		 */
		private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (treeListView1.InvokeRequired)
				{ treeListView1.BeginInvoke(new RunWorkerCompletedEventHandler(_worker_RunWorkerCompleted)); }
			else
				{
					object[] results = e.Result as object[];
					
					if (e.Error != null)
						{
						}
					else
						{
							HistoryCollector visitor = results[1] as HistoryCollector;
							
							foreach(Revision r in visitor.primaryRevs())
								{
									treeListView1.AddObject(r);
									treeListView1.RefreshObject(r);
								}
						}
					
					long delta = (long)results[0];
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
					Revision commit = args.UserState as Revision;

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
						
			/* @TODO if/when i add deleted-item support, 
			 * i'll need the changeset on the item id?
			 */
			VersionSpec ver = VersionSpec.Latest;
			
			megahistorylib.MegaHistory mh = new megahistorylib.MegaHistory(_tfsServer, _distance);
			
			mh.Results = visitor;
			mh.query(_item, _maxChanges, null, null, null);
			
			visitor.fixRevs();
			{
				long end_ticks = DateTime.Now.Ticks;
				e.Result = new object[] { end_ticks - start_ticks, visitor };
			}
		}

	}
}
