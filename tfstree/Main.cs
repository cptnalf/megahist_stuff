using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using Microsoft.Glee.Drawing;
using Microsoft.Glee.GraphViewerGdi;

namespace TFSTree
{
	/// <summary>Main window of TFSTree.</summary>
	public partial class Main : Form
	{
		/// <summary>Database connection.</summary>
		IRevisionRepo database;
		
		/// <summary>Active revisions.</summary>
		Dictionary<string, Revision> revs;
		
		
		/// <summary>Active revision.</summary>
		/// <remarks>A revision is active when the user right-clicks on it.</remarks>
		Revision activeRev;
		
		/// <summary>Last position where mouse wheel was moved.</summary>
		System.Drawing.Point wheelPosition;
		private Grapher _grapher = new Grapher();
		
		/// <summary>Creates and shows the main window.</summary>
		public Main()
			{
				InitializeComponent();
				
				viewer.RemoveToolbar();
				viewer.OutsideAreaBrush = System.Drawing.Brushes.White;
				viewer.MouseWheel += new MouseEventHandler(viewer_MouseWheel);
				toolStripLimit.SelectedIndex = 3;
				
				_grapher.OnProgress += 
					delegate(object sender, EventArgs args)
					{
						toolStripProgressBar.PerformStep();
					};
				
				_grapher.GetRevisionFx = (revID) => database.rev(revID);
			}

		private void _init(IRevisionRepo repo)
		{
			database = repo;
			toolStripBranches.Items.Clear();

			foreach (string name in database.BranchNames)
				{ toolStripBranches.Items.Add(name); }

			if (toolStripBranches.Items.Count > 0)
				{
					toolStripBranches.SelectedIndex = 0;
					InitGUI();
					toolStripRefresh.PerformClick();
				}
			_grapher.Name = database.FileName;
		}
		
        /// <summary>Event handler for mouse wheel events.</summary>
        void viewer_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Location != wheelPosition)
            {
                if (e.Delta > 0)
                    viewer.ZoomInPressed();
                else
                    viewer.ZoomOutPressed();
                wheelPosition = e.Location;
            }
            else
            {
                wheelPosition = new System.Drawing.Point(-1, -1);
            }
        }

        /// <summary>Initializes the GUI.</summary>
        private void InitGUI()
        {
            viewer.PanButtonPressed = true;
            viewer.Visible = true;
            menuItemSave.Enabled = true;
            menuItemCompress.Enabled = true;
            toolStripRefresh.Enabled = true;
            toolStripZoomIn.Enabled = true;
            toolStripZoomOut.Enabled = true;
        }

        /// <summary>Event handler for clicks on menu buttons.</summary>
        private void MenuClick(object sender, EventArgs e)
        {
            if (sender == menuItemOpen)
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Text = "TFSTree - " + Regex.Replace(openFileDialog.FileName, @"^.*\\", "");
                    if (database == null )
											{ database = new Databases.FakeTFS(); }
										
                    database.load(openFileDialog.FileName);
                    _init(database);
                }
            }
            else if (sender == menuItemSave)
            {
                saveFileDialog.Title = "Save As";
                saveFileDialog.Filter = "PNG (*.png)|*.png";
                saveFileDialog.DefaultExt = ".png";
                saveFileDialog.FileName = Regex.Replace(database.FileName, @"\.[^.]*$", ".png");
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
									GraphRenderer renderer = new GraphRenderer(CreateGraph(revs));
                    renderer.CalculateLayout();
                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap((int)viewer.GraphWidth, (int)viewer.GraphHeight);
                    renderer.Render(bitmap);
                    bitmap.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    toolStripProgressBar.Visible = false;
                }
            }
            else if (sender == menuItemExit)
            {
                Close();
            }
            else if (sender == menuItemStatusBar)
            {
                statusStripMain.Visible = menuItemStatusBar.Checked;
            }
            else if (sender == menuItemCompress)
            {
                saveFileDialog.Title = "Compress";
                saveFileDialog.Filter = "MTN (*.mtn)|*.mtn";
                saveFileDialog.DefaultExt = ".mtn";
                saveFileDialog.FileName = Regex.Replace(database.FileName, @"\.[^.]*$", ".mtn");
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (database.FileName.Equals(saveFileDialog.FileName))
                    {
                        MessageBox.Show("The database is currently open and can't be overwritten.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Thread thread = new Thread(delegate()
                        {
														//File.Copy(database.FileName, saveFileDialog.FileName);
														//IRevisionRepo db = new Repository();
														//db.Compress();
                        });
                        thread.Start();
                    }
                }
            }
            else if (sender == menuItemOptions)
            {
                Options options = new Options();
                options.Pages.Add(new GeneralOptions());
                options.Pages.Add(new DisplayOptions());
                if (options.ShowDialog() == DialogResult.OK)
                    _grapher.Colors.Clear();
            }
            else if (sender == menuItemAbout)
            {
                About about = new About();
                about.ShowDialog();
            }
            else if (sender == ctxMenuItemCopyRevisionID)
            {
                if (activeRev != null)
                    Clipboard.SetText(activeRev.ID);
            }
        }

        /// <summary>Event handler for clicks on toolbar buttons.</summary>
        private void ToolbarClick(object sender, EventArgs e)
        {
            if (sender == toolStripRefresh)
            {
							string branch = toolStripBranches.SelectedItem.ToString();
							
                revs = database.revs(branch,
																		 UInt64.Parse(toolStripLimit.SelectedItem.ToString().Substring(0, 
																																																									toolStripLimit.SelectedItem.ToString().IndexOf(' '))));
                if (revs.Count < 2)
                {
									MessageBox.Show("The branch " + toolStripBranches.SelectedItem.ToString() + " can not be displayed as it contains only one revision.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Graph graph = CreateGraph(revs);
                    viewer.Graph = graph;
                    viewer.ZoomF = Math.Max(graph.Width / viewer.Width, graph.Height / viewer.Height);
                    toolStripProgressBar.Visible = false;
                    viewer.Focus();
                }
            }
            else if (sender == toolStripZoomIn)
            {
                viewer.ZoomInPressed();
            }
            else if (sender == toolStripZoomOut)
            {
                viewer.ZoomOutPressed();
            }
        }
				
				Graph CreateGraph(Dictionary<string,Revision> revisions)
				{
					toolStripProgressBar.Visible = true;
					toolStripProgressBar.Value = 0;
					toolStripProgressBar.Maximum = revs.Count;
					
					Graph g = _grapher.Create(revisions);//Create(revisions);
					toolStripProgressBar.Value = toolStripProgressBar.Maximum;
					
					return g;
				}
				

        /// <summary>Event handler called when a new shape is selected in the viewer.</summary>
        private void viewer_SelectionChanged(object sender, EventArgs e)
        {
            object selected = viewer.SelectedObject;
            if (selected == null)
            {
                viewer.SetToolTip(toolTip, "");
            }
            else if (selected is Node)
            {
                Revision rev = (Revision)((Node)selected).UserData;
                string text = rev.Log;
                
                if (rev.Parents.Count > 1)
									{
										text = string.Format("{0}\r\n{1}\r\n{2}\r\n{3}", rev.ID, rev.Author, rev.Date, rev.Log);
									}
                viewer.SetToolTip(toolTip, text);
            }
        }

        /// <summary>Event handlers for mouse clicks in the viewer.</summary>
        private void viewer_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                object clicked = viewer.GetObjectAt(e.X, e.Y);
                if (clicked is Node)
                    Clipboard.SetText(((Revision)((Node)clicked).UserData).ID);
            }
        }

        /// <summary>Event handler called when context menu is opened in the viewer.</summary>
        private void contextMenuStripViewer_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Point p = viewer.PointToClient(Cursor.Position);
            Node current = viewer.GetObjectAt(p.X, p.Y) as Node;
            activeRev = (current != null) ? (Revision)current.UserData : null;
            ctxMenuItemCopyRevisionID.Enabled = (activeRev != null);
        }

				private void toolStripButton1_Click(object sender, EventArgs e)
				{
					
				}

				private void xmldirToolStripMenuItem_Click(object sender, EventArgs e)
				{
					FolderBrowserDialog fbd = new FolderBrowserDialog();
					fbd.Description = "directory where xml-changeset info files are at";
					
					if (fbd.ShowDialog() == DialogResult.OK)
						{
							Text = "TFSTree - " + Regex.Replace(openFileDialog.FileName, @"^.*\\", "");
							if (database == null)
								{ database = new Databases.FakeTFS(); }
							
							database.loadfolder(fbd.SelectedPath);
							toolStripBranches.Items.Clear();
              
							foreach (string name in database.BranchNames)
								{ toolStripBranches.Items.Add(name); }
							
							if (toolStripBranches.Items.Count > 0)
								{
									toolStripBranches.SelectedIndex = 0;
									InitGUI();
									toolStripRefresh.PerformClick();
								}
							_grapher.Name = database.FileName;
						}
				}

				private void saveSnapshotToolStripMenuItem_Click(object sender, EventArgs e)
				{
					if (viewer.Graph != null)
						{
							saveFileDialog.DefaultExt = ".tfsnapshot";
							saveFileDialog.Filter = "TFSTree Snapshots|*.tfsnapshot";
							saveFileDialog.OverwritePrompt = true;
							saveFileDialog.SupportMultiDottedExtensions = true;
							saveFileDialog.Title = "Save TFSTree Snapshot";
							if (saveFileDialog.ShowDialog() == DialogResult.OK)
								{
									Databases.Snapshot snapshot = new Databases.Snapshot();
									snapshot.save(saveFileDialog.FileName, 
									              (string)toolStripBranches.SelectedItem, viewer.Graph);
									snapshot = null;
								}
						}
				}

				private void loadSnapshotToolStripMenuItem_Click(object sender, EventArgs e)
				{
					openFileDialog.Title = "Open TFSTree Snapshot";
					if (openFileDialog.ShowDialog() == DialogResult.OK)
						{
							Databases.Snapshot snapshot = new Databases.Snapshot();
							snapshot.load(openFileDialog.FileName);
							
							_init(snapshot);
						}
				}
    }
}
