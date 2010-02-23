namespace TFSTree
{
    partial class DisplayOptions
    {
        /// <summary>Required designer variable.</summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>Cleans up any resources being used.</summary>
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
            this.groupBoxDisplay = new System.Windows.Forms.GroupBox();
            this.labelBgColors = new System.Windows.Forms.Label();
            this.buttonReset = new System.Windows.Forms.Button();
            this.viewer = new Microsoft.Glee.GraphViewerGdi.GViewer();
            this.colorDialogAdd = new System.Windows.Forms.ColorDialog();
            this.groupBoxDisplay.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxDisplay
            // 
            this.groupBoxDisplay.Controls.Add(this.labelBgColors);
            this.groupBoxDisplay.Controls.Add(this.buttonReset);
            this.groupBoxDisplay.Controls.Add(this.viewer);
            this.groupBoxDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDisplay.Location = new System.Drawing.Point(0, 0);
            this.groupBoxDisplay.Name = "groupBoxDisplay";
            this.groupBoxDisplay.Size = new System.Drawing.Size(327, 235);
            this.groupBoxDisplay.TabIndex = 0;
            this.groupBoxDisplay.TabStop = false;
            this.groupBoxDisplay.Text = "Background Colors";
            // 
            // labelBgColors
            // 
            this.labelBgColors.AutoSize = true;
            this.labelBgColors.Location = new System.Drawing.Point(6, 211);
            this.labelBgColors.Name = "labelBgColors";
            this.labelBgColors.Size = new System.Drawing.Size(211, 13);
            this.labelBgColors.TabIndex = 11;
            this.labelBgColors.Text = "Double-click to change background colors.";
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(246, 206);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(75, 23);
            this.buttonReset.TabIndex = 10;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // viewer
            // 
            this.viewer.AsyncLayout = false;
            this.viewer.AutoScroll = true;
            this.viewer.BackColor = System.Drawing.Color.White;
            this.viewer.BackwardEnabled = false;
            this.viewer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.viewer.ForwardEnabled = false;
            this.viewer.Graph = null;
            this.viewer.Location = new System.Drawing.Point(6, 19);
            this.viewer.MouseHitDistance = 0.05;
            this.viewer.Name = "viewer";
            this.viewer.NavigationVisible = true;
            this.viewer.PanButtonPressed = true;
            this.viewer.SaveButtonVisible = true;
            this.viewer.Size = new System.Drawing.Size(315, 181);
            this.viewer.TabIndex = 1;
            this.viewer.ZoomF = 1;
            this.viewer.ZoomFraction = 0.5;
            this.viewer.ZoomWindowThreshold = 0.05;
            this.viewer.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.viewer_MouseDoubleClick);
            // 
            // DisplayOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.groupBoxDisplay);
            this.Name = "DisplayOptions";
            this.Size = new System.Drawing.Size(327, 235);
            this.groupBoxDisplay.ResumeLayout(false);
            this.groupBoxDisplay.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxDisplay;
        private System.Windows.Forms.ColorDialog colorDialogAdd;
        private System.Windows.Forms.Button buttonReset;
        private Microsoft.Glee.GraphViewerGdi.GViewer viewer;
        private System.Windows.Forms.Label labelBgColors;
    }
}
