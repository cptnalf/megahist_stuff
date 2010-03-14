
namespace StarTree.Plugin.Git
{
	internal class RunCmd
	{
		System.Diagnostics.Process _p;
		
		internal RunCmd(string exe, string workingDir)
		{
			_p = new System.Diagnostics.Process();
			_p.StartInfo = new System.Diagnostics.ProcessStartInfo();
			
			_p.StartInfo.FileName = exe;
			_p.StartInfo.WorkingDirectory = workingDir;
			_p.StartInfo.CreateNoWindow = true;
			_p.StartInfo.UseShellExecute = false;
			_p.StartInfo.RedirectStandardOutput = true;
			
		}

		internal void run(string args)
		{
			_p.StartInfo.Arguments = args;
			_p.Start();
		}
		
		internal System.IO.StreamReader rdr() { return _p.StandardOutput; }
		
		internal void waitForExit() { _p.WaitForExit(); }
	}
}
