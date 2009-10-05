using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tfs_fullhistory
{
	public partial class ChangesetCtrl : UserControl
	{
		public ChangesetCtrl()
		{
			InitializeComponent();
			_comment.Dock = DockStyle.Fill;
			
			DataTable dt = new tables.Changes();
			_changesDGV.DataSource = dt;
			
			dt = new tables.WorkItems();
			_workItemsDGV.DataSource = dt;
		}
		
		public void reset()
		{
			DataTable tbl = _workItemsDGV.DataSource as DataTable;
			
			tbl.Clear();
			
			tbl = _changesDGV.DataSource as DataTable;
			tbl.Clear();
			
			_comment.Text = string.Empty;
			_id.Text = string.Empty;
			_owner.Text = string.Empty;
			_date.Text = string.Empty;
		}
		
		public string Comment { set { _comment.Text = value; } }
		public int ID { set { _id.Text = value.ToString(); } }
		public string Owner { set { _owner.Text = value; } }
		public DateTime CreationDate { set { _date.Text = value.ToString(); } }
		
		public tables.Changes Changes { set { _changesDGV.DataSource = value; } }
		public tables.WorkItems WorkItems { set { _workItemsDGV.DataSource = value; } }
	}
}
