namespace tfs_fullhistory
	{
	partial class OptionsForm
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
			this._save = new System.Windows.Forms.Button();
			this._cancel = new System.Windows.Forms.Button();
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.SuspendLayout();
			// 
			// _save
			// 
			this._save.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._save.Location = new System.Drawing.Point(124, 238);
			this._save.Name = "_save";
			this._save.Size = new System.Drawing.Size(75, 23);
			this._save.TabIndex = 0;
			this._save.Text = "Save";
			this._save.UseVisualStyleBackColor = true;
			this._save.Click += new System.EventHandler(this._save_Click);
			// 
			// _cancel
			// 
			this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancel.Location = new System.Drawing.Point(205, 238);
			this._cancel.Name = "_cancel";
			this._cancel.Size = new System.Drawing.Size(75, 23);
			this._cancel.TabIndex = 1;
			this._cancel.Text = "cancel";
			this._cancel.UseVisualStyleBackColor = true;
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.Location = new System.Drawing.Point(12, 12);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(268, 220);
			this.propertyGrid1.TabIndex = 2;
			// 
			// OptionsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.Add(this.propertyGrid1);
			this.Controls.Add(this._cancel);
			this.Controls.Add(this._save);
			this.Name = "OptionsForm";
			this.Text = "OptionsForm";
			this.ResumeLayout(false);

			}

		#endregion

		private System.Windows.Forms.Button _save;
		private System.Windows.Forms.Button _cancel;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		}
	}