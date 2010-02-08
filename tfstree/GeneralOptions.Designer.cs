namespace TFSTree
{
    partial class GeneralOptions
    {
        /// <summary>Required designer variable.</summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>Clean up any resources being used.</summary>
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
            this.groupBoxGeneral = new System.Windows.Forms.GroupBox();
            this.checkBoxToLocalTime = new System.Windows.Forms.CheckBox();
            this.groupBoxGeneral.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxGeneral
            // 
            this.groupBoxGeneral.Controls.Add(this.checkBoxToLocalTime);
            this.groupBoxGeneral.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxGeneral.Location = new System.Drawing.Point(0, 0);
            this.groupBoxGeneral.Name = "groupBoxGeneral";
            this.groupBoxGeneral.Size = new System.Drawing.Size(219, 71);
            this.groupBoxGeneral.TabIndex = 0;
            this.groupBoxGeneral.TabStop = false;
            this.groupBoxGeneral.Text = "General Settings";
            // 
            // checkBoxToLocalTime
            // 
            this.checkBoxToLocalTime.AutoSize = true;
            this.checkBoxToLocalTime.Location = new System.Drawing.Point(6, 19);
            this.checkBoxToLocalTime.Name = "checkBoxToLocalTime";
            this.checkBoxToLocalTime.Size = new System.Drawing.Size(170, 17);
            this.checkBoxToLocalTime.TabIndex = 0;
            this.checkBoxToLocalTime.Text = "Convert datetimes to local time";
            this.checkBoxToLocalTime.UseVisualStyleBackColor = true;
            // 
            // GeneralOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.groupBoxGeneral);
            this.Name = "GeneralOptions";
            this.Size = new System.Drawing.Size(219, 143);
            this.groupBoxGeneral.ResumeLayout(false);
            this.groupBoxGeneral.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxGeneral;
        private System.Windows.Forms.CheckBox checkBoxToLocalTime;
    }
}
