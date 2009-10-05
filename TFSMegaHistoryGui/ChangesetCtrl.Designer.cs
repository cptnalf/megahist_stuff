namespace tfs_fullhistory
	{
	partial class ChangesetCtrl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
			{
			this._id = new System.Windows.Forms.TextBox();
			this._comment = new System.Windows.Forms.TextBox();
			this._owner = new System.Windows.Forms.TextBox();
			this._changesDGV = new System.Windows.Forms.DataGridView();
			this._workItemsDGV = new System.Windows.Forms.DataGridView();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this._date = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this._changesDGV)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._workItemsDGV)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _id
			// 
			this._id.Location = new System.Drawing.Point(3, 3);
			this._id.Name = "_id";
			this._id.ReadOnly = true;
			this._id.Size = new System.Drawing.Size(116, 20);
			this._id.TabIndex = 0;
			// 
			// _comment
			// 
			this.tableLayoutPanel1.SetColumnSpan(this._comment, 2);
			this._comment.Dock = System.Windows.Forms.DockStyle.Fill;
			this._comment.Location = new System.Drawing.Point(3, 65);
			this._comment.Multiline = true;
			this._comment.Name = "_comment";
			this._comment.ReadOnly = true;
			this._comment.Size = new System.Drawing.Size(284, 33);
			this._comment.TabIndex = 1;
			// 
			// _owner
			// 
			this._owner.Location = new System.Drawing.Point(3, 29);
			this._owner.Name = "_owner";
			this._owner.ReadOnly = true;
			this._owner.Size = new System.Drawing.Size(147, 20);
			this._owner.TabIndex = 2;
			// 
			// _changesDGV
			// 
			this._changesDGV.AllowUserToAddRows = false;
			this._changesDGV.AllowUserToDeleteRows = false;
			this._changesDGV.AllowUserToOrderColumns = true;
			this._changesDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this._changesDGV.Dock = System.Windows.Forms.DockStyle.Fill;
			this._changesDGV.Location = new System.Drawing.Point(3, 3);
			this._changesDGV.MultiSelect = false;
			this._changesDGV.Name = "_changesDGV";
			this._changesDGV.ReadOnly = true;
			this._changesDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this._changesDGV.ShowEditingIcon = false;
			this._changesDGV.ShowRowErrors = false;
			this._changesDGV.Size = new System.Drawing.Size(263, 120);
			this._changesDGV.TabIndex = 3;
			// 
			// _workItemsDGV
			// 
			this._workItemsDGV.AllowUserToAddRows = false;
			this._workItemsDGV.AllowUserToDeleteRows = false;
			this._workItemsDGV.AllowUserToOrderColumns = true;
			this._workItemsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this._workItemsDGV.Dock = System.Windows.Forms.DockStyle.Fill;
			this._workItemsDGV.Location = new System.Drawing.Point(3, 3);
			this._workItemsDGV.Name = "_workItemsDGV";
			this._workItemsDGV.ReadOnly = true;
			this._workItemsDGV.Size = new System.Drawing.Size(263, 120);
			this._workItemsDGV.TabIndex = 4;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(277, 152);
			this.tabControl1.TabIndex = 5;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this._changesDGV);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(269, 126);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Changes";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this._workItemsDGV);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(269, 126);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Work Items";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
			this.splitContainer1.Size = new System.Drawing.Size(277, 257);
			this.splitContainer1.SplitterDistance = 101;
			this.splitContainer1.TabIndex = 6;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this._comment, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this._id, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this._owner, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this._date, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(277, 101);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// _date
			// 
			this._date.Location = new System.Drawing.Point(156, 3);
			this._date.Name = "_date";
			this._date.ReadOnly = true;
			this._date.Size = new System.Drawing.Size(100, 20);
			this._date.TabIndex = 3;
			// 
			// ChangesetCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.Controls.Add(this.splitContainer1);
			this.Name = "ChangesetCtrl";
			this.Size = new System.Drawing.Size(277, 257);
			((System.ComponentModel.ISupportInitialize)(this._changesDGV)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._workItemsDGV)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

			}

		#endregion

		private System.Windows.Forms.TextBox _id;
		private System.Windows.Forms.TextBox _comment;
		private System.Windows.Forms.TextBox _owner;
		private System.Windows.Forms.DataGridView _changesDGV;
		private System.Windows.Forms.DataGridView _workItemsDGV;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TextBox _date;
		}
	}
