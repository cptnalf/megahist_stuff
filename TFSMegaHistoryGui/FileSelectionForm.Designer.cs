namespace tfs_fullhistory
	{
	partial class FileSelectionForm
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
			this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
			this._ok = new System.Windows.Forms.Button();
			this._cancel = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this._filename = new BrightIdeasSoftware.OLVColumn();
			((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// objectListView1
			// 
			this.objectListView1.AllColumns.Add(this._filename);
			this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._filename});
			this.objectListView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.objectListView1.FullRowSelect = true;
			this.objectListView1.Location = new System.Drawing.Point(3, 3);
			this.objectListView1.Name = "objectListView1";
			this.objectListView1.ShowGroups = false;
			this.objectListView1.Size = new System.Drawing.Size(350, 309);
			this.objectListView1.TabIndex = 0;
			this.objectListView1.UseAlternatingBackColors = true;
			this.objectListView1.UseCompatibleStateImageBehavior = false;
			this.objectListView1.View = System.Windows.Forms.View.Details;
			// 
			// _ok
			// 
			this._ok.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._ok.Location = new System.Drawing.Point(191, 3);
			this._ok.Name = "_ok";
			this._ok.Size = new System.Drawing.Size(75, 23);
			this._ok.TabIndex = 1;
			this._ok.Text = "ok";
			this._ok.UseVisualStyleBackColor = true;
			// 
			// _cancel
			// 
			this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancel.Location = new System.Drawing.Point(272, 3);
			this._cancel.Name = "_cancel";
			this._cancel.Size = new System.Drawing.Size(75, 23);
			this._cancel.TabIndex = 2;
			this._cancel.Text = "cancel";
			this._cancel.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.objectListView1, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(356, 350);
			this.tableLayoutPanel1.TabIndex = 3;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.Controls.Add(this._cancel);
			this.flowLayoutPanel1.Controls.Add(this._ok);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 318);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(350, 29);
			this.flowLayoutPanel1.TabIndex = 4;
			// 
			// _filename
			// 
			this._filename.AspectName = "Key";
			this._filename.IsEditable = false;
			this._filename.Text = "Item Path";
			// 
			// FileSelectionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(356, 350);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "FileSelectionForm";
			this.Text = "FileSelectionForm";
			this.Load += new System.EventHandler(this.FileSelectionForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

			}

		#endregion

		private BrightIdeasSoftware.ObjectListView objectListView1;
		private System.Windows.Forms.Button _ok;
		private System.Windows.Forms.Button _cancel;
		private BrightIdeasSoftware.OLVColumn _filename;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		}
	}