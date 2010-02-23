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
	public partial class RevisionViewer : Form
	{
		public RevisionViewer()
		{
			InitializeComponent();
		}
		
		public void setRevision(StarTree.Host.Database.DisplayNames names, StarTree.Host.Database.Revision revision)
		{
			this.SuspendLayout();
			
			TextBox textBox1 = new System.Windows.Forms.TextBox();
			
			// 
			// textBox1
			// 
			textBox1.Dock = DockStyle.Fill;
			textBox1.Font = new Font("Lucida Console", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
			textBox1.Location = new Point(0, 0);
			textBox1.Multiline = true;
			textBox1.Name = "textBox1";
			textBox1.Size = new Size(326, 307);
			textBox1.TabIndex = 0;
			this.Controls.Add(textBox1);
			
			StringBuilder bldr = new StringBuilder();
			
			bldr.AppendFormat("branch {0}", revision.Branch);
			bldr.AppendLine();
			bldr.AppendLine(names.parent);
			foreach(string parent in revision.Parents)
				{
					bldr.AppendLine(parent);
				}
			bldr.AppendFormat("{0} {1}",names.id, revision.ID);
			bldr.AppendLine();
			bldr.AppendFormat("{0}: {1}", names.author, revision.Author);
			bldr.AppendLine();
			bldr.AppendFormat("{0} {1}", names.date, (Properties.Settings.Default.ToLocalTime ? revision.Date.ToLocalTime() : revision.Date));
			bldr.AppendLine();
			bldr.AppendLine(revision.Log);
			textBox1.Text = bldr.ToString();
			
			this.ResumeLayout();
		}

		public void setRevision(StarTree.Host.Database.DisplayNames names, List<StarTree.Host.Database.Revision> revisions)
		{
			DataGridView dgv = new DataGridView();
			DataGridViewTextBoxColumn idCol = new DataGridViewTextBoxColumn();
			idCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
			idCol.DataPropertyName = "ID";
			idCol.HeaderText = names.id;
			idCol.ReadOnly = true;
			idCol.ValueType = typeof(string);
			
			DataGridViewTextBoxColumn branchCol = new DataGridViewTextBoxColumn();
			branchCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
			branchCol.DataPropertyName = "Branch";
			branchCol.HeaderText = "branch";
			branchCol.ReadOnly = true;
			branchCol.ValueType = typeof(string);
			
			DataGridViewTextBoxColumn authorCol = new DataGridViewTextBoxColumn();
			authorCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
			authorCol.DataPropertyName = "Author";
			authorCol.HeaderText = names.author;
			authorCol.ReadOnly = true;
			authorCol.ValueType = typeof(string);
			
			DataGridViewTextBoxColumn dateCol = new DataGridViewTextBoxColumn();
			dateCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
			dateCol.DataPropertyName = "Date";
			dateCol.HeaderText = names.date;
			dateCol.ReadOnly = true;
			dateCol.ValueType = typeof(DateTime);
			
			DataGridViewTextBoxColumn logCol = new DataGridViewTextBoxColumn();
			logCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			logCol.DataPropertyName = "Log";
			logCol.HeaderText = names.log;
			logCol.ReadOnly = true;
			logCol.ValueType = typeof(string);
			
			dgv.Columns.AddRange( idCol, branchCol, authorCol, dateCol, logCol);

			foreach (StarTree.Host.Database.Revision rev in revisions)
				{
					dgv.Rows.Add(rev.ID, rev.Branch, rev.Author, rev.Date, rev.Log);
				}
			
			this.SuspendLayout();
			this.Controls.Add(dgv);
			dgv.Dock = DockStyle.Fill;
			this.ResumeLayout();
		}
	}
}
