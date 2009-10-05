

namespace tfs_fullhistory
{
	namespace tables
	{
		public class Changes : System.Data.DataTable
		{
			public Changes() :base("changes")
			{
				System.Data.DataColumn col = new System.Data.DataColumn("_filename", typeof(string));
				col.ColumnName = "FileName";
				col.ReadOnly = true;
				this.Columns.Add(col);
				
				col = new System.Data.DataColumn("_changeType", typeof(string));
				col.ColumnName = "Change Type";
				col.ReadOnly = true;
				this.Columns.Add(col);
				
				col = new System.Data.DataColumn("_tfsPath", typeof(string));
				col.ColumnName = "TFSPath";
				col.ReadOnly = true;
				this.Columns.Add(col);
			}
			
			public void add(Microsoft.TeamFoundation.VersionControl.Client.Change cng)
			{
				string filename = string.Empty;
				int idx = cng.Item.ServerItem.LastIndexOf('/');
				
				if (idx != -1) { filename = cng.Item.ServerItem.Substring(idx+1); }
			
				this.Rows.Add(filename, cng.ChangeType.ToString(), cng.Item.ServerItem);
			}
			public void add(string fn, string type, string path) { this.Rows.Add(fn, type, path); }
		}
		
		public class WorkItems : System.Data.DataTable
		{
			public WorkItems() : base("workitems")
			{
				System.Data.DataColumn col;
				col = new System.Data.DataColumn("_tfsID", typeof(int));
				col.ColumnName = "ID";
				col.ReadOnly = true;
				this.Columns.Add(col);

				col = new System.Data.DataColumn("_type", typeof(string));
				col.ColumnName = "Type";
				col.ReadOnly = true;
				this.Columns.Add(col);

				col = new System.Data.DataColumn("_itemStatus", typeof(string));
				col.ColumnName = "Status";
				col.ReadOnly = true;
				this.Columns.Add(col);

				col = new System.Data.DataColumn("_title", typeof(string));
				col.ColumnName = "Title";
				col.ReadOnly = true;
				this.Columns.Add(col);
			}
			
			public void addWorkItem(Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItem wi)
			{ this.Rows.Add(wi.Id, wi.Type.Name, wi.State.ToString(), wi.Title); }
		}
	}

	namespace BackgroundJobs
	{
		public class GetWorkItemData : System.ComponentModel.BackgroundWorker
		{
			private tables.WorkItems _workItems = new tfs_fullhistory.tables.WorkItems();
			private Microsoft.TeamFoundation.VersionControl.Client.Changeset _cs;
			private ChangesetCtrl _changesetCtrl;
			
			public GetWorkItemData(ChangesetCtrl changesetCtrl,
								   Microsoft.TeamFoundation.VersionControl.Client.Changeset cs)
			{
				_cs = cs;
				_changesetCtrl = changesetCtrl;
				
				this.WorkerReportsProgress = true;
				this.WorkerSupportsCancellation = false;
			}

			protected override void OnRunWorkerCompleted(System.ComponentModel.RunWorkerCompletedEventArgs e)
			{
				base.OnRunWorkerCompleted(e);
				
				if (_changesetCtrl.InvokeRequired) { _changesetCtrl.Invoke(new System.ComponentModel.RunWorkerCompletedEventHandler(_workerCompleted), this, e); }
				else { _workerCompleted(this, e); }
			}
			
			private void _workerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
			{
				_changesetCtrl.WorkItems = _workItems;
			}
			
			protected override void OnDoWork(System.ComponentModel.DoWorkEventArgs e)
			{
				base.OnDoWork(e);
				
				
				foreach(Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItem wi in _cs.WorkItems)
					{
						_workItems.addWorkItem(wi);
					}
			}
		}
	}
}
