namespace tfs_fullhistory
	{
	partial class Form1
		{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
			{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.button1 = new System.Windows.Forms.Button();
			this._tfsPath = new System.Windows.Forms.TextBox();
			this._changeset = new System.Windows.Forms.TextBox();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.treeListView1 = new BrightIdeasSoftware.TreeListView();
			this._csID = new BrightIdeasSoftware.OLVColumn();
			this._branch = new BrightIdeasSoftware.OLVColumn();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.changesetCtrl1 = new tfs_fullhistory.ChangesetCtrl();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.button2 = new System.Windows.Forms.Button();
			this._noRecurse = new System.Windows.Forms.CheckBox();
			this._allowBranchRevisiting = new System.Windows.Forms.CheckBox();
			this._forceDecomposition = new System.Windows.Forms.CheckBox();
			this._branchesToo = new System.Windows.Forms.CheckBox();
			this._maxChanges = new System.Windows.Forms.NumericUpDown();
			this._time = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this._changeCntNUD = new System.Windows.Forms.NumericUpDown();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.treeListView1)).BeginInit();
			this.contextMenuStrip1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._maxChanges)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._changeCntNUD)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.toolStripContainer1.ContentPanel.SuspendLayout();
			this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(84, 78);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(269, 23);
			this.progressBar1.TabIndex = 2;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(3, 78);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 3;
			this.button1.Text = "go";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// _tfsPath
			// 
			this._tfsPath.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this._tfsPath.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this._tfsPath.Location = new System.Drawing.Point(107, 3);
			this._tfsPath.Name = "_tfsPath";
			this._tfsPath.Size = new System.Drawing.Size(359, 20);
			this._tfsPath.TabIndex = 4;
			// 
			// _changeset
			// 
			this.flowLayoutPanel1.SetFlowBreak(this._changeset, true);
			this._changeset.Location = new System.Drawing.Point(110, 52);
			this._changeset.Name = "_changeset";
			this._changeset.Size = new System.Drawing.Size(100, 20);
			this._changeset.TabIndex = 5;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.treeListView1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.changesetCtrl1);
			this.splitContainer1.Size = new System.Drawing.Size(604, 331);
			this.splitContainer1.SplitterDistance = 206;
			this.splitContainer1.TabIndex = 6;
			// 
			// treeListView1
			// 
			this.treeListView1.AllColumns.Add(this._csID);
			this.treeListView1.AllColumns.Add(this._branch);
			this.treeListView1.AlternateRowBackColor = System.Drawing.Color.LightCyan;
			this.treeListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._csID,
            this._branch});
			this.treeListView1.ContextMenuStrip = this.contextMenuStrip1;
			this.treeListView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeListView1.EmptyListMsg = "No Changesets Found";
			this.treeListView1.EmptyListMsgFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.treeListView1.FullRowSelect = true;
			this.treeListView1.HideSelection = false;
			this.treeListView1.Location = new System.Drawing.Point(0, 0);
			this.treeListView1.Name = "treeListView1";
			this.treeListView1.OwnerDraw = true;
			this.treeListView1.SelectedColumnTint = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(255)))), ((int)(((byte)(250)))), ((int)(((byte)(205)))));
			this.treeListView1.ShowGroups = false;
			this.treeListView1.Size = new System.Drawing.Size(206, 331);
			this.treeListView1.TabIndex = 0;
			this.treeListView1.TintSortColumn = true;
			this.treeListView1.UseAlternatingBackColors = true;
			this.treeListView1.UseCompatibleStateImageBehavior = false;
			this.treeListView1.UseHotItem = true;
			this.treeListView1.View = System.Windows.Forms.View.Details;
			this.treeListView1.VirtualMode = true;
			this.treeListView1.CellToolTipShowing += new System.EventHandler<BrightIdeasSoftware.ToolTipShowingEventArgs>(this.treeListView1_CellToolTipShowing);
			this.treeListView1.SelectionChanged += new System.EventHandler(this.treeListView1_SelectionChanged);
			// 
			// _csID
			// 
			this._csID.AspectName = "id";
			this._csID.AspectToStringFormat = "{0:d}";
			this._csID.IsEditable = false;
			this._csID.Text = "Changeset";
			this._csID.Width = 69;
			// 
			// _branch
			// 
			this._branch.IsEditable = false;
			this._branch.Text = "Branch";
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.ShowImageMargin = false;
			this.contextMenuStrip1.Size = new System.Drawing.Size(116, 26);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(115, 22);
			this.toolStripMenuItem1.Text = "Compare...";
			this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
			// 
			// changesetCtrl1
			// 
			this.changesetCtrl1.AutoSize = true;
			this.changesetCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.changesetCtrl1.Location = new System.Drawing.Point(0, 0);
			this.changesetCtrl1.Name = "changesetCtrl1";
			this.changesetCtrl1.Size = new System.Drawing.Size(394, 331);
			this.changesetCtrl1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(98, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "TFS Path (full path)";
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 49);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(101, 26);
			this.label2.TabIndex = 8;
			this.label2.Text = "Change Identifier\r\n(changeset number)";
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.label1);
			this.flowLayoutPanel1.Controls.Add(this._tfsPath);
			this.flowLayoutPanel1.Controls.Add(this.button2);
			this.flowLayoutPanel1.Controls.Add(this._noRecurse);
			this.flowLayoutPanel1.Controls.Add(this._allowBranchRevisiting);
			this.flowLayoutPanel1.Controls.Add(this._forceDecomposition);
			this.flowLayoutPanel1.Controls.Add(this._branchesToo);
			this.flowLayoutPanel1.Controls.Add(this.label2);
			this.flowLayoutPanel1.Controls.Add(this._changeset);
			this.flowLayoutPanel1.Controls.Add(this.button1);
			this.flowLayoutPanel1.Controls.Add(this.progressBar1);
			this.flowLayoutPanel1.Controls.Add(this._maxChanges);
			this.flowLayoutPanel1.Controls.Add(this._time);
			this.flowLayoutPanel1.Controls.Add(this.label3);
			this.flowLayoutPanel1.Controls.Add(this._changeCntNUD);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(604, 139);
			this.flowLayoutPanel1.TabIndex = 9;
			// 
			// button2
			// 
			this.flowLayoutPanel1.SetFlowBreak(this.button2, true);
			this.button2.Location = new System.Drawing.Point(472, 3);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(24, 20);
			this.button2.TabIndex = 9;
			this.button2.Text = "...";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// _noRecurse
			// 
			this._noRecurse.AutoSize = true;
			this._noRecurse.Location = new System.Drawing.Point(3, 29);
			this._noRecurse.Name = "_noRecurse";
			this._noRecurse.Size = new System.Drawing.Size(80, 17);
			this._noRecurse.TabIndex = 12;
			this._noRecurse.Text = "NoRecurse";
			this.toolTip1.SetToolTip(this._noRecurse, "Only execute one \'QueryMerges\'\r\n(just for the first line of merges)");
			this._noRecurse.UseVisualStyleBackColor = true;
			// 
			// _allowBranchRevisiting
			// 
			this._allowBranchRevisiting.AutoSize = true;
			this._allowBranchRevisiting.Location = new System.Drawing.Point(89, 29);
			this._allowBranchRevisiting.Name = "_allowBranchRevisiting";
			this._allowBranchRevisiting.Size = new System.Drawing.Size(131, 17);
			this._allowBranchRevisiting.TabIndex = 13;
			this._allowBranchRevisiting.Text = "AllowBranchRevisiting";
			this.toolTip1.SetToolTip(this._allowBranchRevisiting, resources.GetString("_allowBranchRevisiting.ToolTip"));
			this._allowBranchRevisiting.UseVisualStyleBackColor = true;
			this._allowBranchRevisiting.CheckStateChanged += new System.EventHandler(this._allowBranchRevisiting_CheckStateChanged);
			// 
			// _forceDecomposition
			// 
			this._forceDecomposition.AutoSize = true;
			this._forceDecomposition.Location = new System.Drawing.Point(226, 29);
			this._forceDecomposition.Name = "_forceDecomposition";
			this._forceDecomposition.Size = new System.Drawing.Size(123, 17);
			this._forceDecomposition.TabIndex = 14;
			this._forceDecomposition.Text = "ForceDecomposition";
			this.toolTip1.SetToolTip(this._forceDecomposition, resources.GetString("_forceDecomposition.ToolTip"));
			this._forceDecomposition.UseVisualStyleBackColor = true;
			// 
			// _branchesToo
			// 
			this._branchesToo.AutoSize = true;
			this.flowLayoutPanel1.SetFlowBreak(this._branchesToo, true);
			this._branchesToo.Location = new System.Drawing.Point(355, 29);
			this._branchesToo.Name = "_branchesToo";
			this._branchesToo.Size = new System.Drawing.Size(90, 17);
			this._branchesToo.TabIndex = 15;
			this._branchesToo.Text = "BranchesToo";
			this.toolTip1.SetToolTip(this._branchesToo, resources.GetString("_branchesToo.ToolTip"));
			this._branchesToo.UseVisualStyleBackColor = true;
			// 
			// _maxChanges
			// 
			this._maxChanges.Location = new System.Drawing.Point(359, 78);
			this._maxChanges.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
			this._maxChanges.Name = "_maxChanges";
			this._maxChanges.Size = new System.Drawing.Size(66, 20);
			this._maxChanges.TabIndex = 10;
			this._maxChanges.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			// 
			// _time
			// 
			this.flowLayoutPanel1.SetFlowBreak(this._time, true);
			this._time.Location = new System.Drawing.Point(431, 78);
			this._time.Name = "_time";
			this._time.ReadOnly = true;
			this._time.Size = new System.Drawing.Size(125, 20);
			this._time.TabIndex = 11;
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 110);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 13);
			this.label3.TabIndex = 17;
			this.label3.Text = "max changes count";
			// 
			// _changeCntNUD
			// 
			this._changeCntNUD.Location = new System.Drawing.Point(109, 107);
			this._changeCntNUD.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
			this._changeCntNUD.Name = "_changeCntNUD";
			this._changeCntNUD.Size = new System.Drawing.Size(75, 20);
			this._changeCntNUD.TabIndex = 16;
			this.toolTip1.SetToolTip(this._changeCntNUD, "Maximum number of changes to show in the \'changes\' tab\r\n(too many will result in " +
							"alot of memory usage, and slowdowns)\r\n");
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.flowLayoutPanel1);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.splitContainer1);
			this.splitContainer2.Size = new System.Drawing.Size(604, 474);
			this.splitContainer2.SplitterDistance = 139;
			this.splitContainer2.TabIndex = 10;
			// 
			// toolStripContainer1
			// 
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.AutoScroll = true;
			this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer2);
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(604, 474);
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
			this.toolStripContainer1.Name = "toolStripContainer1";
			this.toolStripContainer1.Size = new System.Drawing.Size(604, 498);
			this.toolStripContainer1.TabIndex = 11;
			this.toolStripContainer1.Text = "toolStripContainer1";
			// 
			// toolStripContainer1.TopToolStripPanel
			// 
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(604, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.toolsToolStripMenuItem.Text = "Tools";
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.optionsToolStripMenuItem.Text = "Options...";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(604, 498);
			this.Controls.Add(this.toolStripContainer1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "TFSMegaHistory GUI";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.treeListView1)).EndInit();
			this.contextMenuStrip1.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._maxChanges)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._changeCntNUD)).EndInit();
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.ResumeLayout(false);
			this.toolStripContainer1.ContentPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.PerformLayout();
			this.toolStripContainer1.ResumeLayout(false);
			this.toolStripContainer1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);

			}

		#endregion

		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox _tfsPath;
		private System.Windows.Forms.TextBox _changeset;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.Button button2;
		private ChangesetCtrl changesetCtrl1;
		private System.Windows.Forms.NumericUpDown _maxChanges;
		private System.Windows.Forms.TextBox _time;
		private System.Windows.Forms.CheckBox _noRecurse;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.CheckBox _allowBranchRevisiting;
		private System.Windows.Forms.CheckBox _forceDecomposition;
		private System.Windows.Forms.CheckBox _branchesToo;
		private BrightIdeasSoftware.TreeListView treeListView1;
		private BrightIdeasSoftware.OLVColumn _csID;
		private BrightIdeasSoftware.OLVColumn _branch;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown _changeCntNUD;
		private System.Windows.Forms.ToolStripContainer toolStripContainer1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		}
	}

