namespace TFSTree
{
    partial class Options
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
            this.panelBottom = new System.Windows.Forms.Panel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.listViewGroups = new System.Windows.Forms.ListView();
            this.imageListGroups = new System.Windows.Forms.ImageList(this.components);
            this.panelPages = new System.Windows.Forms.Panel();
            this.etchedLine = new TFSTree.EtchedLine();
            this.panelBottom.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.etchedLine);
            this.panelBottom.Controls.Add(this.buttonCancel);
            this.panelBottom.Controls.Add(this.buttonOK);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 235);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(435, 48);
            this.panelBottom.TabIndex = 0;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(348, 13);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonOK.Location = new System.Drawing.Point(267, 13);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.listViewGroups);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Padding = new System.Windows.Forms.Padding(8);
            this.panelLeft.Size = new System.Drawing.Size(108, 235);
            this.panelLeft.TabIndex = 2;
            // 
            // listViewGroups
            // 
            this.listViewGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewGroups.LargeImageList = this.imageListGroups;
            this.listViewGroups.Location = new System.Drawing.Point(8, 8);
            this.listViewGroups.MultiSelect = false;
            this.listViewGroups.Name = "listViewGroups";
            this.listViewGroups.Size = new System.Drawing.Size(92, 219);
            this.listViewGroups.TabIndex = 0;
            this.listViewGroups.UseCompatibleStateImageBehavior = false;
            this.listViewGroups.SelectedIndexChanged += new System.EventHandler(this.listViewGroups_SelectedIndexChanged);
            // 
            // imageListGroups
            // 
            this.imageListGroups.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListGroups.ImageStream")));
            this.imageListGroups.TransparentColor = System.Drawing.Color.Magenta;
            this.imageListGroups.Images.SetKeyName(0, "");
            this.imageListGroups.Images.SetKeyName(1, "");
            this.imageListGroups.Images.SetKeyName(2, "");
            this.imageListGroups.Images.SetKeyName(3, "");
            this.imageListGroups.Images.SetKeyName(4, "");
            this.imageListGroups.Images.SetKeyName(5, "");
            this.imageListGroups.Images.SetKeyName(6, "");
            this.imageListGroups.Images.SetKeyName(7, "");
            this.imageListGroups.Images.SetKeyName(8, "");
            this.imageListGroups.Images.SetKeyName(9, "");
            this.imageListGroups.Images.SetKeyName(10, "");
            this.imageListGroups.Images.SetKeyName(11, "");
            this.imageListGroups.Images.SetKeyName(12, "");
            this.imageListGroups.Images.SetKeyName(13, "");
            this.imageListGroups.Images.SetKeyName(14, "");
            this.imageListGroups.Images.SetKeyName(15, "");
            this.imageListGroups.Images.SetKeyName(16, "");
            this.imageListGroups.Images.SetKeyName(17, "");
            this.imageListGroups.Images.SetKeyName(18, "");
            this.imageListGroups.Images.SetKeyName(19, "");
            this.imageListGroups.Images.SetKeyName(20, "");
            this.imageListGroups.Images.SetKeyName(21, "");
            this.imageListGroups.Images.SetKeyName(22, "");
            this.imageListGroups.Images.SetKeyName(23, "");
            this.imageListGroups.Images.SetKeyName(24, "");
            this.imageListGroups.Images.SetKeyName(25, "");
            // 
            // panelPages
            // 
            this.panelPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPages.Location = new System.Drawing.Point(108, 0);
            this.panelPages.Name = "panelPages";
            this.panelPages.Padding = new System.Windows.Forms.Padding(8);
            this.panelPages.Size = new System.Drawing.Size(327, 235);
            this.panelPages.TabIndex = 0;
            // 
            // etchedLine
            // 
            this.etchedLine.DarkColor = System.Drawing.SystemColors.ControlDark;
            this.etchedLine.Dock = System.Windows.Forms.DockStyle.Top;
            this.etchedLine.LightColor = System.Drawing.SystemColors.ControlLight;
            this.etchedLine.Location = new System.Drawing.Point(0, 0);
            this.etchedLine.Name = "etchedLine";
            this.etchedLine.Size = new System.Drawing.Size(435, 8);
            this.etchedLine.TabIndex = 2;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 283);
            this.Controls.Add(this.panelPages);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.panelBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Options";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.Options_Load);
            this.panelBottom.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.ListView listViewGroups;
        private System.Windows.Forms.Panel panelPages;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ImageList imageListGroups;
        private TFSTree.EtchedLine etchedLine;
    }
}
