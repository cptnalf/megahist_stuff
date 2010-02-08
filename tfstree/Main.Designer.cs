namespace TFSTree
{
    partial class Main
    {
        /// <summary>Required designer variable.</summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>Clean up any resources being used.</summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
        private void InitializeComponent()
        {
				this.components = new System.ComponentModel.Container();
				System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
				this.menuStripMain = new System.Windows.Forms.MenuStrip();
				this.menuItemFile = new System.Windows.Forms.ToolStripMenuItem();
				this.menuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
				this.menuItemSave = new System.Windows.Forms.ToolStripMenuItem();
				this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
				this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
				this.menuItemView = new System.Windows.Forms.ToolStripMenuItem();
				this.menuItemStatusBar = new System.Windows.Forms.ToolStripMenuItem();
				this.menuItemTools = new System.Windows.Forms.ToolStripMenuItem();
				this.menuItemCompress = new System.Windows.Forms.ToolStripMenuItem();
				this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
				this.menuItemOptions = new System.Windows.Forms.ToolStripMenuItem();
				this.menuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
				this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
				this.toolStripMain = new System.Windows.Forms.ToolStrip();
				this.toolStripBranches = new System.Windows.Forms.ToolStripComboBox();
				this.toolStripLimit = new System.Windows.Forms.ToolStripComboBox();
				this.toolStripRefresh = new System.Windows.Forms.ToolStripButton();
				this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
				this.toolStripZoomIn = new System.Windows.Forms.ToolStripButton();
				this.toolStripZoomOut = new System.Windows.Forms.ToolStripButton();
				this.viewer = new Microsoft.Glee.GraphViewerGdi.GViewer();
				this.contextMenuStripViewer = new System.Windows.Forms.ContextMenuStrip(this.components);
				this.ctxMenuItemCopyRevisionID = new System.Windows.Forms.ToolStripMenuItem();
				this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
				this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
				this.statusStripMain = new System.Windows.Forms.StatusStrip();
				this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
				this.toolTip = new System.Windows.Forms.ToolTip(this.components);
				this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
				this.menuStripMain.SuspendLayout();
				this.toolStripMain.SuspendLayout();
				this.contextMenuStripViewer.SuspendLayout();
				this.statusStripMain.SuspendLayout();
				this.SuspendLayout();
				// 
				// menuStripMain
				// 
				this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFile,
            this.menuItemView,
            this.menuItemTools,
            this.menuItemHelp});
				this.menuStripMain.Location = new System.Drawing.Point(0, 0);
				this.menuStripMain.Name = "menuStripMain";
				this.menuStripMain.Size = new System.Drawing.Size(592, 24);
				this.menuStripMain.TabIndex = 0;
				this.menuStripMain.Text = "menuStrip1";
				// 
				// menuItemFile
				// 
				this.menuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemOpen,
            this.menuItemSave,
            this.toolStripSeparator1,
            this.menuItemExit});
				this.menuItemFile.Name = "menuItemFile";
				this.menuItemFile.Size = new System.Drawing.Size(35, 20);
				this.menuItemFile.Text = "&File";
				// 
				// menuItemOpen
				// 
				this.menuItemOpen.Image = global::TFSTree.Properties.Resources.folder_page;
				this.menuItemOpen.Name = "menuItemOpen";
				this.menuItemOpen.ShortcutKeyDisplayString = "";
				this.menuItemOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
				this.menuItemOpen.Size = new System.Drawing.Size(177, 22);
				this.menuItemOpen.Text = "&Open ...";
				this.menuItemOpen.Click += new System.EventHandler(this.MenuClick);
				// 
				// menuItemSave
				// 
				this.menuItemSave.Enabled = false;
				this.menuItemSave.Image = global::TFSTree.Properties.Resources.picture_save;
				this.menuItemSave.Name = "menuItemSave";
				this.menuItemSave.ShortcutKeyDisplayString = "";
				this.menuItemSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
				this.menuItemSave.Size = new System.Drawing.Size(177, 22);
				this.menuItemSave.Text = "&Save As ...";
				this.menuItemSave.Click += new System.EventHandler(this.MenuClick);
				// 
				// toolStripSeparator1
				// 
				this.toolStripSeparator1.Name = "toolStripSeparator1";
				this.toolStripSeparator1.Size = new System.Drawing.Size(174, 6);
				// 
				// menuItemExit
				// 
				this.menuItemExit.Image = global::TFSTree.Properties.Resources.door_in;
				this.menuItemExit.Name = "menuItemExit";
				this.menuItemExit.ShortcutKeyDisplayString = "";
				this.menuItemExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
				this.menuItemExit.Size = new System.Drawing.Size(177, 22);
				this.menuItemExit.Text = "E&xit";
				this.menuItemExit.Click += new System.EventHandler(this.MenuClick);
				// 
				// menuItemView
				// 
				this.menuItemView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemStatusBar});
				this.menuItemView.Name = "menuItemView";
				this.menuItemView.Size = new System.Drawing.Size(41, 20);
				this.menuItemView.Text = "&View";
				// 
				// menuItemStatusBar
				// 
				this.menuItemStatusBar.Checked = true;
				this.menuItemStatusBar.CheckOnClick = true;
				this.menuItemStatusBar.CheckState = System.Windows.Forms.CheckState.Checked;
				this.menuItemStatusBar.Name = "menuItemStatusBar";
				this.menuItemStatusBar.Size = new System.Drawing.Size(163, 22);
				this.menuItemStatusBar.Text = "Show status bar";
				this.menuItemStatusBar.Click += new System.EventHandler(this.MenuClick);
				// 
				// menuItemTools
				// 
				this.menuItemTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemCompress,
            this.toolStripSeparator2,
            this.menuItemOptions});
				this.menuItemTools.Name = "menuItemTools";
				this.menuItemTools.Size = new System.Drawing.Size(44, 20);
				this.menuItemTools.Text = "&Tools";
				// 
				// menuItemCompress
				// 
				this.menuItemCompress.Enabled = false;
				this.menuItemCompress.Image = global::TFSTree.Properties.Resources.page_white_compressed;
				this.menuItemCompress.Name = "menuItemCompress";
				this.menuItemCompress.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
				this.menuItemCompress.Size = new System.Drawing.Size(186, 22);
				this.menuItemCompress.Text = "Compress ...";
				this.menuItemCompress.Click += new System.EventHandler(this.MenuClick);
				// 
				// toolStripSeparator2
				// 
				this.toolStripSeparator2.Name = "toolStripSeparator2";
				this.toolStripSeparator2.Size = new System.Drawing.Size(183, 6);
				// 
				// menuItemOptions
				// 
				this.menuItemOptions.Image = global::TFSTree.Properties.Resources.wrench;
				this.menuItemOptions.Name = "menuItemOptions";
				this.menuItemOptions.Size = new System.Drawing.Size(186, 22);
				this.menuItemOptions.Text = "&Options";
				this.menuItemOptions.Click += new System.EventHandler(this.MenuClick);
				// 
				// menuItemHelp
				// 
				this.menuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAbout});
				this.menuItemHelp.Name = "menuItemHelp";
				this.menuItemHelp.Size = new System.Drawing.Size(40, 20);
				this.menuItemHelp.Text = "&Help";
				// 
				// menuItemAbout
				// 
				this.menuItemAbout.Image = global::TFSTree.Properties.Resources.user;
				this.menuItemAbout.Name = "menuItemAbout";
				this.menuItemAbout.Size = new System.Drawing.Size(114, 22);
				this.menuItemAbout.Text = "&About";
				this.menuItemAbout.Click += new System.EventHandler(this.MenuClick);
				// 
				// toolStripMain
				// 
				this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripBranches,
            this.toolStripLimit,
            this.toolStripRefresh,
            this.toolStripSeparator3,
            this.toolStripZoomIn,
            this.toolStripZoomOut,
            this.toolStripButton1});
				this.toolStripMain.Location = new System.Drawing.Point(0, 24);
				this.toolStripMain.Name = "toolStripMain";
				this.toolStripMain.Size = new System.Drawing.Size(592, 25);
				this.toolStripMain.TabIndex = 1;
				this.toolStripMain.Text = "toolStrip1";
				// 
				// toolStripBranches
				// 
				this.toolStripBranches.Name = "toolStripBranches";
				this.toolStripBranches.Size = new System.Drawing.Size(300, 25);
				this.toolStripBranches.Sorted = true;
				// 
				// toolStripLimit
				// 
				this.toolStripLimit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
				this.toolStripLimit.Items.AddRange(new object[] {
            "10 revisions",
            "20 revisions",
            "50 revisions",
            "100 revisions",
            "200 revisions",
            "500 revisions"});
				this.toolStripLimit.Name = "toolStripLimit";
				this.toolStripLimit.Size = new System.Drawing.Size(100, 25);
				// 
				// toolStripRefresh
				// 
				this.toolStripRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
				this.toolStripRefresh.Enabled = false;
				this.toolStripRefresh.Image = global::TFSTree.Properties.Resources.table_refresh;
				this.toolStripRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
				this.toolStripRefresh.Name = "toolStripRefresh";
				this.toolStripRefresh.Size = new System.Drawing.Size(23, 22);
				this.toolStripRefresh.Text = "Refresh";
				this.toolStripRefresh.Click += new System.EventHandler(this.ToolbarClick);
				// 
				// toolStripSeparator3
				// 
				this.toolStripSeparator3.Name = "toolStripSeparator3";
				this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
				// 
				// toolStripZoomIn
				// 
				this.toolStripZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
				this.toolStripZoomIn.Enabled = false;
				this.toolStripZoomIn.Image = global::TFSTree.Properties.Resources.zoom_in;
				this.toolStripZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
				this.toolStripZoomIn.Name = "toolStripZoomIn";
				this.toolStripZoomIn.Size = new System.Drawing.Size(23, 22);
				this.toolStripZoomIn.Text = "Zoom In";
				this.toolStripZoomIn.Click += new System.EventHandler(this.ToolbarClick);
				// 
				// toolStripZoomOut
				// 
				this.toolStripZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
				this.toolStripZoomOut.Enabled = false;
				this.toolStripZoomOut.Image = global::TFSTree.Properties.Resources.zoom_out;
				this.toolStripZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
				this.toolStripZoomOut.Name = "toolStripZoomOut";
				this.toolStripZoomOut.Size = new System.Drawing.Size(23, 22);
				this.toolStripZoomOut.Text = "Zoom Out";
				this.toolStripZoomOut.Click += new System.EventHandler(this.ToolbarClick);
				// 
				// viewer
				// 
				this.viewer.AsyncLayout = false;
				this.viewer.AutoScroll = true;
				this.viewer.BackwardEnabled = false;
				this.viewer.ContextMenuStrip = this.contextMenuStripViewer;
				this.viewer.Dock = System.Windows.Forms.DockStyle.Fill;
				this.viewer.EditObjects = false;
				this.viewer.ForwardEnabled = false;
				this.viewer.Graph = null;
				this.viewer.Location = new System.Drawing.Point(0, 49);
				this.viewer.MouseHitDistance = 0.05;
				this.viewer.Name = "viewer";
				this.viewer.NavigationVisible = true;
				this.viewer.PanButtonPressed = false;
				this.viewer.SaveButtonVisible = true;
				this.viewer.Size = new System.Drawing.Size(592, 345);
				this.viewer.TabIndex = 0;
				this.viewer.Visible = false;
				this.viewer.ZoomF = 1;
				this.viewer.ZoomFraction = 0.5;
				this.viewer.ZoomWindowThreshold = 0.05;
				this.viewer.SelectionChanged += new System.EventHandler(this.viewer_SelectionChanged);
				this.viewer.MouseClick += new System.Windows.Forms.MouseEventHandler(this.viewer_MouseClick);
				// 
				// contextMenuStripViewer
				// 
				this.contextMenuStripViewer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxMenuItemCopyRevisionID});
				this.contextMenuStripViewer.Name = "contextMenuStripViewer";
				this.contextMenuStripViewer.Size = new System.Drawing.Size(168, 26);
				this.contextMenuStripViewer.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripViewer_Opening);
				// 
				// ctxMenuItemCopyRevisionID
				// 
				this.ctxMenuItemCopyRevisionID.Name = "ctxMenuItemCopyRevisionID";
				this.ctxMenuItemCopyRevisionID.Size = new System.Drawing.Size(167, 22);
				this.ctxMenuItemCopyRevisionID.Text = "&Copy Revision ID";
				this.ctxMenuItemCopyRevisionID.Click += new System.EventHandler(this.MenuClick);
				// 
				// openFileDialog
				// 
				this.openFileDialog.Filter = "monotone database|*.mtn|tfs tree files|*.tfstree|All files|*.*";
				this.openFileDialog.Title = "Open";
				// 
				// statusStripMain
				// 
				this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar});
				this.statusStripMain.Location = new System.Drawing.Point(0, 394);
				this.statusStripMain.Name = "statusStripMain";
				this.statusStripMain.Size = new System.Drawing.Size(592, 22);
				this.statusStripMain.TabIndex = 1;
				// 
				// toolStripProgressBar
				// 
				this.toolStripProgressBar.Name = "toolStripProgressBar";
				this.toolStripProgressBar.Size = new System.Drawing.Size(100, 16);
				this.toolStripProgressBar.Step = 1;
				this.toolStripProgressBar.Visible = false;
				// 
				// toolStripButton1
				// 
				this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
				this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
				this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
				this.toolStripButton1.Name = "toolStripButton1";
				this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
				this.toolStripButton1.Text = "toolStripButton1";
				this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
				// 
				// Main
				// 
				this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
				this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
				this.BackColor = System.Drawing.Color.White;
				this.ClientSize = new System.Drawing.Size(592, 416);
				this.Controls.Add(this.viewer);
				this.Controls.Add(this.statusStripMain);
				this.Controls.Add(this.toolStripMain);
				this.Controls.Add(this.menuStripMain);
				this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
				this.MainMenuStrip = this.menuStripMain;
				this.Name = "Main";
				this.Text = "tfstree";
				this.menuStripMain.ResumeLayout(false);
				this.menuStripMain.PerformLayout();
				this.toolStripMain.ResumeLayout(false);
				this.toolStripMain.PerformLayout();
				this.contextMenuStripViewer.ResumeLayout(false);
				this.statusStripMain.ResumeLayout(false);
				this.statusStripMain.PerformLayout();
				this.ResumeLayout(false);
				this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripMenuItem menuItemFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemOpen;
        private System.Windows.Forms.ToolStripMenuItem menuItemSave;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private Microsoft.Glee.GraphViewerGdi.GViewer viewer;
        private System.Windows.Forms.ToolStripComboBox toolStripBranches;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemHelp;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private System.Windows.Forms.ToolStripComboBox toolStripLimit;
        private System.Windows.Forms.ToolStripButton toolStripRefresh;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripZoomIn;
        private System.Windows.Forms.ToolStripButton toolStripZoomOut;
        private System.Windows.Forms.ToolStripMenuItem menuItemTools;
        private System.Windows.Forms.ToolStripMenuItem menuItemOptions;
        private System.Windows.Forms.ToolStripMenuItem menuItemView;
        private System.Windows.Forms.ToolStripMenuItem menuItemStatusBar;
        private System.Windows.Forms.ToolStripMenuItem menuItemCompress;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripViewer;
        private System.Windows.Forms.ToolStripMenuItem ctxMenuItemCopyRevisionID;
				private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}
