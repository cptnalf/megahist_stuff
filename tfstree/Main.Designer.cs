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
				this.saveSnapshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
				this.loadSnapshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
				this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
				this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
				this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
				this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
				this.menuItemView = new System.Windows.Forms.ToolStripMenuItem();
				this.menuItemStatusBar = new System.Windows.Forms.ToolStripMenuItem();
				this.menuItemTools = new System.Windows.Forms.ToolStripMenuItem();
				this.menuItemCompress = new System.Windows.Forms.ToolStripMenuItem();
				this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
				this.xmldirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
				this.toolStrip1 = new System.Windows.Forms.ToolStrip();
				this.newBtn = new System.Windows.Forms.ToolStripButton();
				this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
				this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
				this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
				this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
				this.cutToolStripButton = new System.Windows.Forms.ToolStripButton();
				this.copyToolStripButton = new System.Windows.Forms.ToolStripButton();
				this.pasteToolStripButton = new System.Windows.Forms.ToolStripButton();
				this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
				this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
				this._serverNameTB = new System.Windows.Forms.ToolStripTextBox();
				this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
				this.menuStripMain.SuspendLayout();
				this.toolStripMain.SuspendLayout();
				this.contextMenuStripViewer.SuspendLayout();
				this.statusStripMain.SuspendLayout();
				this.toolStrip1.SuspendLayout();
				this.toolStripContainer1.ContentPanel.SuspendLayout();
				this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
				this.toolStripContainer1.SuspendLayout();
				this.SuspendLayout();
				// 
				// menuStripMain
				// 
				this.menuStripMain.Dock = System.Windows.Forms.DockStyle.None;
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
            this.saveSnapshotToolStripMenuItem,
            this.loadSnapshotToolStripMenuItem,
            this.toolStripSeparator5,
            this.clearToolStripMenuItem,
            this.toolStripSeparator6,
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
				// saveSnapshotToolStripMenuItem
				// 
				this.saveSnapshotToolStripMenuItem.Name = "saveSnapshotToolStripMenuItem";
				this.saveSnapshotToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
				this.saveSnapshotToolStripMenuItem.Text = "save snapshot";
				this.saveSnapshotToolStripMenuItem.Click += new System.EventHandler(this.saveSnapshotToolStripMenuItem_Click);
				// 
				// loadSnapshotToolStripMenuItem
				// 
				this.loadSnapshotToolStripMenuItem.Name = "loadSnapshotToolStripMenuItem";
				this.loadSnapshotToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
				this.loadSnapshotToolStripMenuItem.Text = "load snapshot";
				this.loadSnapshotToolStripMenuItem.Click += new System.EventHandler(this.loadSnapshotToolStripMenuItem_Click);
				// 
				// toolStripSeparator5
				// 
				this.toolStripSeparator5.Name = "toolStripSeparator5";
				this.toolStripSeparator5.Size = new System.Drawing.Size(174, 6);
				// 
				// clearToolStripMenuItem
				// 
				this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
				this.clearToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
				this.clearToolStripMenuItem.Text = "clear";
				this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
				// 
				// toolStripSeparator6
				// 
				this.toolStripSeparator6.Name = "toolStripSeparator6";
				this.toolStripSeparator6.Size = new System.Drawing.Size(174, 6);
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
            this.xmldirToolStripMenuItem,
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
				// xmldirToolStripMenuItem
				// 
				this.xmldirToolStripMenuItem.Name = "xmldirToolStripMenuItem";
				this.xmldirToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
				this.xmldirToolStripMenuItem.Text = "xmldir";
				this.xmldirToolStripMenuItem.Click += new System.EventHandler(this.xmldirToolStripMenuItem_Click);
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
				this.toolStripMain.Dock = System.Windows.Forms.DockStyle.None;
				this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripBranches,
            this.toolStripLimit,
            this.toolStripRefresh,
            this.toolStripSeparator3,
            this.toolStripZoomIn,
            this.toolStripZoomOut});
				this.toolStripMain.Location = new System.Drawing.Point(3, 24);
				this.toolStripMain.Name = "toolStripMain";
				this.toolStripMain.Size = new System.Drawing.Size(491, 25);
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
				this.viewer.Location = new System.Drawing.Point(0, 0);
				this.viewer.MouseHitDistance = 0.05;
				this.viewer.Name = "viewer";
				this.viewer.NavigationVisible = true;
				this.viewer.PanButtonPressed = false;
				this.viewer.SaveButtonVisible = true;
				this.viewer.Size = new System.Drawing.Size(592, 387);
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
				this.statusStripMain.Location = new System.Drawing.Point(0, 461);
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
				// toolStrip1
				// 
				this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
				this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newBtn,
            this.openToolStripButton,
            this.saveToolStripButton,
            this.printToolStripButton,
            this.toolStripSeparator,
            this.cutToolStripButton,
            this.copyToolStripButton,
            this.pasteToolStripButton,
            this.toolStripSeparator4,
            this.helpToolStripButton,
            this._serverNameTB});
				this.toolStrip1.Location = new System.Drawing.Point(3, 49);
				this.toolStrip1.Name = "toolStrip1";
				this.toolStrip1.Size = new System.Drawing.Size(341, 25);
				this.toolStrip1.TabIndex = 2;
				this.toolStrip1.Text = "toolStrip1";
				// 
				// newBtn
				// 
				this.newBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
				this.newBtn.Image = ((System.Drawing.Image)(resources.GetObject("newBtn.Image")));
				this.newBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
				this.newBtn.Name = "newBtn";
				this.newBtn.Size = new System.Drawing.Size(23, 22);
				this.newBtn.Text = "&New";
				this.newBtn.Click += new System.EventHandler(this.newBtn_Click);
				// 
				// openToolStripButton
				// 
				this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
				this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
				this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
				this.openToolStripButton.Name = "openToolStripButton";
				this.openToolStripButton.Size = new System.Drawing.Size(23, 22);
				this.openToolStripButton.Text = "&Open";
				// 
				// saveToolStripButton
				// 
				this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
				this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
				this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
				this.saveToolStripButton.Name = "saveToolStripButton";
				this.saveToolStripButton.Size = new System.Drawing.Size(23, 22);
				this.saveToolStripButton.Text = "&Save";
				// 
				// printToolStripButton
				// 
				this.printToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
				this.printToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripButton.Image")));
				this.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
				this.printToolStripButton.Name = "printToolStripButton";
				this.printToolStripButton.Size = new System.Drawing.Size(23, 22);
				this.printToolStripButton.Text = "&Print";
				// 
				// toolStripSeparator
				// 
				this.toolStripSeparator.Name = "toolStripSeparator";
				this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
				// 
				// cutToolStripButton
				// 
				this.cutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
				this.cutToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripButton.Image")));
				this.cutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
				this.cutToolStripButton.Name = "cutToolStripButton";
				this.cutToolStripButton.Size = new System.Drawing.Size(23, 22);
				this.cutToolStripButton.Text = "C&ut";
				// 
				// copyToolStripButton
				// 
				this.copyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
				this.copyToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripButton.Image")));
				this.copyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
				this.copyToolStripButton.Name = "copyToolStripButton";
				this.copyToolStripButton.Size = new System.Drawing.Size(23, 22);
				this.copyToolStripButton.Text = "&Copy";
				// 
				// pasteToolStripButton
				// 
				this.pasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
				this.pasteToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripButton.Image")));
				this.pasteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
				this.pasteToolStripButton.Name = "pasteToolStripButton";
				this.pasteToolStripButton.Size = new System.Drawing.Size(23, 22);
				this.pasteToolStripButton.Text = "&Paste";
				// 
				// toolStripSeparator4
				// 
				this.toolStripSeparator4.Name = "toolStripSeparator4";
				this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
				// 
				// helpToolStripButton
				// 
				this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
				this.helpToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("helpToolStripButton.Image")));
				this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
				this.helpToolStripButton.Name = "helpToolStripButton";
				this.helpToolStripButton.Size = new System.Drawing.Size(23, 22);
				this.helpToolStripButton.Text = "He&lp";
				// 
				// _serverNameTB
				// 
				this._serverNameTB.Name = "_serverNameTB";
				this._serverNameTB.Size = new System.Drawing.Size(100, 25);
				this._serverNameTB.Text = "rnotfsat";
				// 
				// toolStripContainer1
				// 
				// 
				// toolStripContainer1.ContentPanel
				// 
				this.toolStripContainer1.ContentPanel.Controls.Add(this.viewer);
				this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(592, 387);
				this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
				this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
				this.toolStripContainer1.Name = "toolStripContainer1";
				this.toolStripContainer1.Size = new System.Drawing.Size(592, 461);
				this.toolStripContainer1.TabIndex = 3;
				this.toolStripContainer1.Text = "toolStripContainer1";
				// 
				// toolStripContainer1.TopToolStripPanel
				// 
				this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStripMain);
				this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripMain);
				this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
				// 
				// Main
				// 
				this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
				this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
				this.BackColor = System.Drawing.Color.White;
				this.ClientSize = new System.Drawing.Size(592, 483);
				this.Controls.Add(this.toolStripContainer1);
				this.Controls.Add(this.statusStripMain);
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
				this.toolStrip1.ResumeLayout(false);
				this.toolStrip1.PerformLayout();
				this.toolStripContainer1.ContentPanel.ResumeLayout(false);
				this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
				this.toolStripContainer1.TopToolStripPanel.PerformLayout();
				this.toolStripContainer1.ResumeLayout(false);
				this.toolStripContainer1.PerformLayout();
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
				private System.Windows.Forms.ToolStripMenuItem saveSnapshotToolStripMenuItem;
				private System.Windows.Forms.ToolStripMenuItem loadSnapshotToolStripMenuItem;
				private System.Windows.Forms.ToolStrip toolStrip1;
				private System.Windows.Forms.ToolStripButton newBtn;
				private System.Windows.Forms.ToolStripButton openToolStripButton;
				private System.Windows.Forms.ToolStripButton saveToolStripButton;
				private System.Windows.Forms.ToolStripButton printToolStripButton;
				private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
				private System.Windows.Forms.ToolStripButton cutToolStripButton;
				private System.Windows.Forms.ToolStripButton copyToolStripButton;
				private System.Windows.Forms.ToolStripButton pasteToolStripButton;
				private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
				private System.Windows.Forms.ToolStripButton helpToolStripButton;
				private System.Windows.Forms.ToolStripTextBox _serverNameTB;
				private System.Windows.Forms.ToolStripContainer toolStripContainer1;
				private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
				private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
				private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
				private System.Windows.Forms.ToolStripMenuItem xmldirToolStripMenuItem;
    }
}
