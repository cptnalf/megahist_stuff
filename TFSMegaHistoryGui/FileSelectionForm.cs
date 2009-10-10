using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tfs_fullhistory
{
	public partial class FileSelectionForm : Form
	{
		private SortedDictionary<string,pair<int,int>> _paths = new SortedDictionary<string,pair<int,int>>();
		
		public SortedDictionary<string,pair<int,int>> Paths { set { _paths = value; } }
		public string SelectedPath
		{ 
			get
				{
					string str = string.Empty;
					
					if (objectListView1.SelectedIndex >= 0)
						{
							KeyValuePair<string,pair<int,int>> value = (KeyValuePair<string,pair<int,int>>)objectListView1.GetModelObject(objectListView1.SelectedIndex);
							str = value.Key;
						}
					
					return str;
				}
		}
		
		public FileSelectionForm()
		{
			InitializeComponent();
		}

		private void FileSelectionForm_Load(object sender, EventArgs e)
		{
			objectListView1.AddObjects(_paths);
			objectListView1.Refresh();
		}
		
	}
}
