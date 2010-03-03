namespace tfs_fullhistory
	{
	partial class HistoryForm
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
			this.treeListView1 = new BrightIdeasSoftware.TreeListView();
			this._csID = new BrightIdeasSoftware.OLVColumn();
			this._branch = new BrightIdeasSoftware.OLVColumn();
			this._csDate = new BrightIdeasSoftware.OLVColumn();
			this._csOwner = new BrightIdeasSoftware.OLVColumn();
			this._csComment = new BrightIdeasSoftware.OLVColumn();
			((System.ComponentModel.ISupportInitialize)(this.treeListView1)).BeginInit();
			this.SuspendLayout();
			// 
			// treeListView1
			// 
			this.treeListView1.AllColumns.Add(this._csID);
			this.treeListView1.AllColumns.Add(this._branch);
			this.treeListView1.AllColumns.Add(this._csDate);
			this.treeListView1.AllColumns.Add(this._csOwner);
			this.treeListView1.AllColumns.Add(this._csComment);
			this.treeListView1.AlternateRowBackColor = System.Drawing.Color.LightCyan;
			this.treeListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._csID,
            this._branch,
            this._csDate,
            this._csOwner,
            this._csComment});
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
			this.treeListView1.Size = new System.Drawing.Size(604, 372);
			this.treeListView1.TabIndex = 1;
			this.treeListView1.TintSortColumn = true;
			this.treeListView1.UseAlternatingBackColors = true;
			this.treeListView1.UseCompatibleStateImageBehavior = false;
			this.treeListView1.UseHotItem = true;
			this.treeListView1.View = System.Windows.Forms.View.Details;
			this.treeListView1.VirtualMode = true;
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
			// _csDate
			// 
			this._csDate.AspectName = "creationDate";
			this._csDate.AspectToStringFormat = "{0}";
			this._csDate.IsEditable = false;
			this._csDate.Text = "Date";
			// 
			// _csOwner
			// 
			this._csOwner.AspectName = "user";
			this._csOwner.Text = "user";
			// 
			// _csComment
			// 
			this._csComment.AspectName = "comment";
			this._csComment.Text = "Comment";
			// 
			// HistoryForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(604, 372);
			this.Controls.Add(this.treeListView1);
			this.Name = "HistoryForm";
			this.Text = "HistoryForm";
			this.Load += new System.EventHandler(this.HistoryForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.treeListView1)).EndInit();
			this.ResumeLayout(false);

			}

		#endregion

		private BrightIdeasSoftware.TreeListView treeListView1;
		private BrightIdeasSoftware.OLVColumn _csID;
		private BrightIdeasSoftware.OLVColumn _branch;
		private BrightIdeasSoftware.OLVColumn _csDate;
		private BrightIdeasSoftware.OLVColumn _csOwner;
		private BrightIdeasSoftware.OLVColumn _csComment;
		}
	}