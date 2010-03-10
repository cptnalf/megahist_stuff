namespace tfs_fullhistory
	{
	partial class MainForm
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
			this._tfsTree = new System.Windows.Forms.TreeView();
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this._distN = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this._targetVerTB = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this._limitN = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this._fromTB = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this._toTB = new System.Windows.Forms.TextBox();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this._branchesCB = new System.Windows.Forms.ComboBox();
			this._goBtn = new System.Windows.Forms.Button();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.historyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.megahistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripContainer1.ContentPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._distN)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._limitN)).BeginInit();
			this.flowLayoutPanel2.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _tfsTree
			// 
			this._tfsTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tfsTree.Location = new System.Drawing.Point(3, 114);
			this._tfsTree.Name = "_tfsTree";
			this._tfsTree.Size = new System.Drawing.Size(548, 399);
			this._tfsTree.TabIndex = 0;
			// 
			// toolStripContainer1
			// 
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.AutoScroll = true;
			this.toolStripContainer1.ContentPanel.Controls.Add(this.tableLayoutPanel1);
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(554, 516);
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
			this.toolStripContainer1.Name = "toolStripContainer1";
			this.toolStripContainer1.Size = new System.Drawing.Size(554, 541);
			this.toolStripContainer1.TabIndex = 1;
			this.toolStripContainer1.Text = "toolStripContainer1";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this._tfsTree, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 0, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(554, 516);
			this.tableLayoutPanel1.TabIndex = 12;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.Controls.Add(this.label1);
			this.flowLayoutPanel1.Controls.Add(this._targetVerTB);
			this.flowLayoutPanel1.Controls.Add(this.label2);
			this.flowLayoutPanel1.Controls.Add(this._limitN);
			this.flowLayoutPanel1.Controls.Add(this.label3);
			this.flowLayoutPanel1.Controls.Add(this._distN);
			this.flowLayoutPanel1.Controls.Add(this.label4);
			this.flowLayoutPanel1.Controls.Add(this._fromTB);
			this.flowLayoutPanel1.Controls.Add(this.label5);
			this.flowLayoutPanel1.Controls.Add(this._toTB);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(548, 52);
			this.flowLayoutPanel1.TabIndex = 11;
			// 
			// _distN
			// 
			this.flowLayoutPanel1.SetFlowBreak(this._distN, true);
			this._distN.Location = new System.Drawing.Point(314, 3);
			this._distN.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this._distN.Name = "_distN";
			this._distN.Size = new System.Drawing.Size(39, 20);
			this._distN.TabIndex = 3;
			this._distN.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(71, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "target version";
			// 
			// _targetVerTB
			// 
			this._targetVerTB.Location = new System.Drawing.Point(80, 3);
			this._targetVerTB.Name = "_targetVerTB";
			this._targetVerTB.Size = new System.Drawing.Size(100, 20);
			this._targetVerTB.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(186, 6);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(24, 13);
			this.label2.TabIndex = 7;
			this.label2.Text = "limit";
			// 
			// _limitN
			// 
			this._limitN.Location = new System.Drawing.Point(216, 3);
			this._limitN.Name = "_limitN";
			this._limitN.Size = new System.Drawing.Size(39, 20);
			this._limitN.TabIndex = 2;
			this._limitN.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(261, 6);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(47, 13);
			this.label3.TabIndex = 8;
			this.label3.Text = "distance";
			// 
			// label4
			// 
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 32);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(27, 13);
			this.label4.TabIndex = 9;
			this.label4.Text = "from";
			// 
			// _fromTB
			// 
			this._fromTB.Location = new System.Drawing.Point(36, 29);
			this._fromTB.Name = "_fromTB";
			this._fromTB.Size = new System.Drawing.Size(100, 20);
			this._fromTB.TabIndex = 4;
			// 
			// label5
			// 
			this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(142, 32);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(16, 13);
			this.label5.TabIndex = 10;
			this.label5.Text = "to";
			// 
			// _toTB
			// 
			this._toTB.Location = new System.Drawing.Point(164, 29);
			this._toTB.Name = "_toTB";
			this._toTB.Size = new System.Drawing.Size(100, 20);
			this._toTB.TabIndex = 5;
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.AutoSize = true;
			this.flowLayoutPanel2.Controls.Add(this._branchesCB);
			this.flowLayoutPanel2.Controls.Add(this._goBtn);
			this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 81);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(548, 27);
			this.flowLayoutPanel2.TabIndex = 12;
			// 
			// _branchesCB
			// 
			this._branchesCB.FormattingEnabled = true;
			this._branchesCB.Location = new System.Drawing.Point(3, 3);
			this._branchesCB.Name = "_branchesCB";
			this._branchesCB.Size = new System.Drawing.Size(360, 21);
			this._branchesCB.TabIndex = 0;
			// 
			// _goBtn
			// 
			this._goBtn.Location = new System.Drawing.Point(369, 3);
			this._goBtn.Name = "_goBtn";
			this._goBtn.Size = new System.Drawing.Size(33, 21);
			this._goBtn.TabIndex = 1;
			this._goBtn.Text = "go";
			this._goBtn.UseVisualStyleBackColor = true;
			this._goBtn.Click += new System.EventHandler(this._goBtn_Click);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.historyToolStripMenuItem,
            this.megahistoryToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(153, 70);
			// 
			// historyToolStripMenuItem
			// 
			this.historyToolStripMenuItem.Name = "historyToolStripMenuItem";
			this.historyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.historyToolStripMenuItem.Text = "history";
			this.historyToolStripMenuItem.Click += new System.EventHandler(this.historyToolStripMenuItem_Click);
			// 
			// megahistoryToolStripMenuItem
			// 
			this.megahistoryToolStripMenuItem.Name = "megahistoryToolStripMenuItem";
			this.megahistoryToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.megahistoryToolStripMenuItem.Text = "megahistory";
			this.megahistoryToolStripMenuItem.Click += new System.EventHandler(this.megahistoryToolStripMenuItem_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(554, 541);
			this.Controls.Add(this.toolStripContainer1);
			this.Name = "MainForm";
			this.Text = "TFS MegaHistory";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.toolStripContainer1.ContentPanel.ResumeLayout(false);
			this.toolStripContainer1.ResumeLayout(false);
			this.toolStripContainer1.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._distN)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._limitN)).EndInit();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);

			}

		#endregion

		private System.Windows.Forms.TreeView _tfsTree;
		private System.Windows.Forms.ToolStripContainer toolStripContainer1;
		private System.Windows.Forms.TextBox _toTB;
		private System.Windows.Forms.TextBox _fromTB;
		private System.Windows.Forms.NumericUpDown _distN;
		private System.Windows.Forms.NumericUpDown _limitN;
		private System.Windows.Forms.TextBox _targetVerTB;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.ComboBox _branchesCB;
		private System.Windows.Forms.Button _goBtn;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem historyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem megahistoryToolStripMenuItem;
		}
	}