using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BranchDifferGui
{
	public partial class HistoryViewerForm : Form
	{
		private int _historyCount;
		private string _tfsPath;
		private Microsoft.TeamFoundation.VersionControl.Client.VersionControlServer _vcs;
		
		public void setInfos(Microsoft.TeamFoundation.VersionControl.Client.VersionControlServer vcs, string tfsPath, int historyCount)
		{
			_vcs = vcs;
			_tfsPath = tfsPath;
			_historyCount = historyCount;
		}
		
		public HistoryViewerForm()
		{
			InitializeComponent();
		}

		private void HistoryViewerForm_Load(object sender, EventArgs e)
		{
			System.Collections.IEnumerable changesets = 
			_vcs.QueryHistory(_tfsPath, Microsoft.TeamFoundation.VersionControl.Client.VersionSpec.Latest, 
			0, Microsoft.TeamFoundation.VersionControl.Client.RecursionType.None, null, 
			null, null, _historyCount, false, true, true);
			
			List<Microsoft.TeamFoundation.VersionControl.Client.Changeset> typedcs = new List<Microsoft.TeamFoundation.VersionControl.Client.Changeset>();
			
			foreach(object o in changesets)
			{
				if (o is Microsoft.TeamFoundation.VersionControl.Client.Changeset)
				{ typedcs.Add(o as Microsoft.TeamFoundation.VersionControl.Client.Changeset); }
			}
			
			objectListView1.ClearObjects();
			objectListView1.AddObjects(typedcs);
		}

		private void objectListView1_SelectionChanged(object sender, EventArgs e)
		{
			propertyGrid1.SelectedObject = null;
			
			if (objectListView1.SelectedIndex >=0)
			{
				propertyGrid1.SelectedObject = objectListView1.GetSelectedObject();
			}
		}
	}
}
