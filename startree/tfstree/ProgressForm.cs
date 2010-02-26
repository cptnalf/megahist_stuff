using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TFSTree
{
	public partial class ProgressForm : Form
	{
		private ErrorProvider _err;
		
		public BackgroundWorker worker;
		
		public ProgressForm()
		{
			InitializeComponent();
			
			_err = new ErrorProvider();
			
			Load += _onLoad;
		}
		
		private void _onLoad(object sender, EventArgs args)
		{
			if (worker != null)
				{
					if (worker.WorkerReportsProgress)
						{
							worker.ProgressChanged += _progressChanged;
							
							progressBar1.Style = ProgressBarStyle.Blocks;
							progressBar1.Step = 1;
							progressBar1.Minimum = 0;
							progressBar1.Maximum = 100;
							progressBar1.Value = 0;
						}
					else
						{
							progressBar1.Style = ProgressBarStyle.Marquee;
							progressBar1.Step = 10;
							progressBar1.Value = 10;
						}
					
					progressBar1.Refresh();
					worker.RunWorkerCompleted += _runWorkerCompleted;
					
					worker.RunWorkerAsync();
				}
			else
				{
					this.DialogResult = DialogResult.Abort;
					this.Close();
				}
		}
		
		private void _runWorkerCompleted(object sender, RunWorkerCompletedEventArgs args)
		{
			if (args.Error != null)
				{
					_err.SetError(progressBar1, args.Error.ToString());
					Button okBtn = new Button();
					okBtn.Text = "ok";
					okBtn.Click += _okClicked;
					
					flowLayoutPanel1.Controls.Add(okBtn);
				}
			else
				{
					this.DialogResult = DialogResult.OK;
					this.Close();
				}
		}
		
		private void _okClicked(object sender, EventArgs args)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
		
		private void _progressChanged(object sender, ProgressChangedEventArgs args)
		{
			if (args.ProgressPercentage > 0)
				{
					progressBar1.PerformStep();
				}
		}
	}
}
