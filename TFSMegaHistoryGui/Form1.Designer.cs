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
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.button1 = new System.Windows.Forms.Button();
			this._tfsPath = new System.Windows.Forms.TextBox();
			this._changeset = new System.Windows.Forms.TextBox();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.changesetCtrl1 = new tfs_fullhistory.ChangesetCtrl();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.button2 = new System.Windows.Forms.Button();
			this._noRecurse = new System.Windows.Forms.CheckBox();
			this._allowBranchRevisiting = new System.Windows.Forms.CheckBox();
			this._forceDecomposition = new System.Windows.Forms.CheckBox();
			this._maxChanges = new System.Windows.Forms.NumericUpDown();
			this._time = new System.Windows.Forms.TextBox();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this._branchesToo = new System.Windows.Forms.CheckBox();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._maxChanges)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.SuspendLayout();
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.Location = new System.Drawing.Point(0, 0);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(188, 323);
			this.treeView1.TabIndex = 0;
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
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
			this.splitContainer1.Panel1.Controls.Add(this.treeView1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.changesetCtrl1);
			this.splitContainer1.Size = new System.Drawing.Size(568, 323);
			this.splitContainer1.SplitterDistance = 188;
			this.splitContainer1.TabIndex = 6;
			// 
			// changesetCtrl1
			// 
			this.changesetCtrl1.AutoSize = true;
			this.changesetCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.changesetCtrl1.Location = new System.Drawing.Point(0, 0);
			this.changesetCtrl1.Name = "changesetCtrl1";
			this.changesetCtrl1.Size = new System.Drawing.Size(376, 323);
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
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(568, 108);
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
			this._time.Location = new System.Drawing.Point(431, 78);
			this._time.Name = "_time";
			this._time.ReadOnly = true;
			this._time.Size = new System.Drawing.Size(125, 20);
			this._time.TabIndex = 11;
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
			this.splitContainer2.Size = new System.Drawing.Size(568, 435);
			this.splitContainer2.SplitterDistance = 108;
			this.splitContainer2.TabIndex = 10;
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
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(568, 435);
			this.Controls.Add(this.splitContainer2);
			this.Name = "Form1";
			this.Text = "Form1";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._maxChanges)).EndInit();
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.ResumeLayout(false);
			this.ResumeLayout(false);

			}

		#endregion

		private System.Windows.Forms.TreeView treeView1;
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
		}
	}

