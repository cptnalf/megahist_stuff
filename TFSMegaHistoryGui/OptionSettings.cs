using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tfs_fullhistory
{
	public class OptionSettings
	{
		[System.ComponentModel.Category("Global")]
		[System.ComponentModel.Description("The name of the TFS Server to connect to.")]
		public string TFSServerName { get; set; }
	}
}
