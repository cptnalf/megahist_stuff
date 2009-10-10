
using System.Diagnostics;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace sourcecontrol.utils
{
	public class DiffTool
	{
		private string _cmd;
		private string _args;
		
		public DiffTool(string cmd, string args) { _cmd = cmd; _args=args; }
		
		public Process diff(string left, string leftName, string right, string rightName)
		{ 
			System.Text.StringBuilder args = new System.Text.StringBuilder(_args);
			
			/*
			int lf_idx = _args.IndexOf("%0");
			int ln_idx = _args.IndexOf("%1");
			int rf_idx = _args.IndexOf("%2");
			int rn_idx = _args.IndexOf("%3");
			
			 * like to have the indices ordered 
			 * the problem here is if some fool decided to have '%X' in their filename we're a bit screwed...
			 */
			
			args.Replace("%0", left);
			args.Replace("%1", leftName);
			args.Replace("%2", right);
			args.Replace("%3", rightName);
			
			return Process.Start(_cmd, args.ToString()); 
		}
	}
}

namespace tfs.utils
{
	public class PathCompare : System.ComponentModel.BackgroundWorker
	{
		private sourcecontrol.utils.DiffTool _diffTool = null;
		private VersionControlServer _vcs;
		private string _left;
		private string _right;
		
		public PathCompare(VersionControlServer vcs, sourcecontrol.utils.DiffTool diffTool, 
											 string left, string right)
		{
			_vcs = vcs;
			_diffTool = diffTool;
			_left = left;
			_right = right;
			this.WorkerReportsProgress = false;
			this.WorkerSupportsCancellation = false;
		}
		
		protected override void OnDoWork(System.ComponentModel.DoWorkEventArgs e)
		{
			string tmpFile1 = System.IO.Path.GetTempFileName();
			string tmpFile2 = System.IO.Path.GetTempFileName();
			_vcs.DownloadFile(_left, tmpFile1);
			_vcs.DownloadFile(_right, tmpFile2);
			
			Process diff = _diffTool.diff(tmpFile1, _left, tmpFile2, _right);
			
			diff.WaitForExit();
			
			System.IO.File.Delete(tmpFile1);
			System.IO.File.Delete(tmpFile2);
		}
	}
}