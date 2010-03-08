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
	using IntPair = saastdlib.pair<int,int>;
	using PathsDict = SortedDictionary<string, saastdlib.pair<int, int>>;
	
	public partial class FileSelectionForm : Form
	{
		private PathsDict _paths = new PathsDict();
		
		public PathsDict Paths { set { _paths = value; } }
		public string SelectedPath
		{ 
			get
				{
					string str = string.Empty;
					
					if (objectListView1.SelectedIndex >= 0)
						{
							KeyValuePair<string,IntPair> value = (KeyValuePair<string,IntPair>)objectListView1.GetModelObject(objectListView1.SelectedIndex);
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
