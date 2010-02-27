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
	public partial class OptionsForm : Form
	{
		private OptionSettings _foo;
		private Properties.Settings _settings;
		
		public OptionsForm()
		{
			_settings = new tfs_fullhistory.Properties.Settings();
			InitializeComponent();
			_foo = new OptionSettings();
			_foo.TFSServerName = _settings.TFSServer;
			
			propertyGrid1.SelectedObject = _foo;
		}

		private void _save_Click(object sender, EventArgs e)
		{
			if (_foo.TFSServerName != _settings.TFSServer)
				{
					_settings["TFSServer"] = _foo.TFSServerName;
					_settings.Save();
					Properties.Settings.Default.Reload();
				}
		}
	}
}
