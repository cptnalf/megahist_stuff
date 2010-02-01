namespace workitem_viewer
	{
	partial class WorkItemViewerForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkItemViewerForm));
			this.treeListView1 = new BrightIdeasSoftware.TreeListView();
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
			this.fooToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.treeListView1)).BeginInit();
			this.toolStripContainer1.ContentPanel.SuspendLayout();
			this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// treeListView1
			// 
			this.treeListView1.Location = new System.Drawing.Point(12, 81);
			this.treeListView1.Name = "treeListView1";
			this.treeListView1.OwnerDraw = true;
			this.treeListView1.ShowGroups = false;
			this.treeListView1.Size = new System.Drawing.Size(276, 277);
			this.treeListView1.TabIndex = 0;
			this.treeListView1.UseCompatibleStateImageBehavior = false;
			this.treeListView1.View = System.Windows.Forms.View.Details;
			this.treeListView1.VirtualMode = true;
			// 
			// toolStripContainer1
			// 
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.AutoScroll = true;
			this.toolStripContainer1.ContentPanel.Controls.Add(this.treeListView1);
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(485, 451);
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
			this.toolStripContainer1.Name = "toolStripContainer1";
			this.toolStripContainer1.Size = new System.Drawing.Size(485, 476);
			this.toolStripContainer1.TabIndex = 1;
			this.toolStripContainer1.Text = "toolStripContainer1";
			// 
			// toolStripContainer1.TopToolStripPanel
			// 
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButton1});
			this.toolStrip1.Location = new System.Drawing.Point(3, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(86, 25);
			this.toolStrip1.TabIndex = 0;
			// 
			// toolStripSplitButton1
			// 
			this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fooToolStripMenuItem});
			this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
			this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripSplitButton1.Name = "toolStripSplitButton1";
			this.toolStripSplitButton1.Size = new System.Drawing.Size(74, 22);
			this.toolStripSplitButton1.Text = "queries";
			// 
			// fooToolStripMenuItem
			// 
			this.fooToolStripMenuItem.CheckOnClick = true;
			this.fooToolStripMenuItem.Name = "fooToolStripMenuItem";
			this.fooToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.fooToolStripMenuItem.Text = "foo";
			// 
			// WorkItemViewerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(485, 476);
			this.Controls.Add(this.toolStripContainer1);
			this.Name = "WorkItemViewerForm";
			this.Text = "Work Item Viewer";
			this.Load += new System.EventHandler(this.WorkItemViewerForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.treeListView1)).EndInit();
			this.toolStripContainer1.ContentPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.PerformLayout();
			this.toolStripContainer1.ResumeLayout(false);
			this.toolStripContainer1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);

			}

		#endregion

		private BrightIdeasSoftware.TreeListView treeListView1;
		private System.Windows.Forms.ToolStripContainer toolStripContainer1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
		private System.Windows.Forms.ToolStripMenuItem fooToolStripMenuItem;
		}
	}

