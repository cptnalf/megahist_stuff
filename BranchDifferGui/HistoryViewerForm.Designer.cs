namespace BranchDifferGui
	{
	partial class HistoryViewerForm
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
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this._changeSetID = new BrightIdeasSoftware.OLVColumn();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// objectListView1
			// 
			this.objectListView1.AllColumns.Add(this._changeSetID);
			this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._changeSetID});
			this.objectListView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.objectListView1.Location = new System.Drawing.Point(3, 3);
			this.objectListView1.Name = "objectListView1";
			this.objectListView1.Size = new System.Drawing.Size(199, 382);
			this.objectListView1.TabIndex = 0;
			this.objectListView1.UseCompatibleStateImageBehavior = false;
			this.objectListView1.View = System.Windows.Forms.View.Details;
			this.objectListView1.SelectionChanged += new System.EventHandler(this.objectListView1_SelectionChanged);
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid1.Location = new System.Drawing.Point(208, 3);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(199, 382);
			this.propertyGrid1.TabIndex = 1;
			// 
			// _changeSetID
			// 
			this._changeSetID.AspectName = "ChangesetId";
			this._changeSetID.IsEditable = false;
			this._changeSetID.Text = "ChangesetID";
			this._changeSetID.Width = 73;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.objectListView1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.propertyGrid1, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(410, 388);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// HistoryViewerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(410, 388);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "HistoryViewerForm";
			this.Text = "HistoryViewerForm";
			this.Load += new System.EventHandler(this.HistoryViewerForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

			}

		#endregion

		private BrightIdeasSoftware.ObjectListView objectListView1;
		private BrightIdeasSoftware.OLVColumn _changeSetID;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		}
	}