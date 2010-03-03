using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace workitem_viewer
{
	public partial class WorkItemViewerForm : Form
	{
		public WorkItemViewerForm()
		{
			InitializeComponent();
		}
		
		private void WorkItemViewerForm_Load(object sender, EventArgs e)
		{
			
		}
		
		private void _getQueriesCompleted()
		{
			if (this.InvokeRequired)
				{
					//this.Invoke(new tfs_interface.actions.CompletedDelegate(_getqueriesCompleted), act, e);
					return;
				}
			
			//if (e == null)
				{
					//tfs_interface.actions.GetQueries gq = act as tfs_interface.actions.GetQueries;
					//toolStripSplitButton1.DropDownItems.Clear();
					
					//foreach(string key in gq.queries.Keys)
					//  {
					//    ToolStripMenuItem keymi = new ToolStripMenuItem(key);
							
					//    foreach(Microsoft.TeamFoundation.WorkItemTracking.Client.StoredQuery q in gq.queries[key])
					//      {
					//        /*
					//          if (!grps.ContainsKey(q.Project.Name))
					//          {
					//          grps.Add(q.Project.Name, new ListViewGroup(q.Project.Name));
					//          listView1.Groups.Add(grps[q.Project.Name]);
					//          }
										
					//          ListViewItem lvi = listView1.Items.Add(q.Name);
					//          lvi.Group = grps[q.Project.Name];
					//        */
					//        ToolStripMenuItem tsmi = new ToolStripMenuItem(q.Name);
					//        tsmi.CheckOnClick = true;
					//        tsmi.Click += _queryMenuItemClicked;
					//        tsmi.ToolTipText = q.Description;
					//        keymi.DropDownItems.Add(tsmi);
					//      }
					//    toolStripSplitButton1.DropDownItems.Add(keymi);
					//  }
				}

		}
	}
}
