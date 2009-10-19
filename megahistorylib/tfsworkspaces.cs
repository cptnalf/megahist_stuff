
using System.Collections.Generic;
using	Microsoft.TeamFoundation.VersionControl.Client;

namespace megahistory
{
	public class TFSWorkspaces
	{
		private VersionControlServer _vcs = null;
		private Workspace[] _workspaces = null;
		
		public TFSWorkspaces(VersionControlServer vcs)
		: this(vcs, System.Environment.UserName, System.Environment.MachineName) { }
		
		public TFSWorkspaces(VersionControlServer vcs, string userName, string machineName)
		{
			_vcs = vcs;
			_workspaces = vcs.QueryWorkspaces(null, userName, machineName);
		}
		
		public List<string> getLocalPaths(string tfsPath)
		{
			List<string> localPaths = new List<string>();
			
			if (_workspaces != null)
				{
					string localPath = null;
					
					foreach(Workspace w in _workspaces)
						{
							localPath = w.TryGetLocalItemForServerItem(tfsPath);
							if (localPath != null) { localPaths.Add(localPath); }
						}
				}
			
			return localPaths;
		}
		
		public List<string> getServerPaths(string localPath)
		{
			List<string> serverPaths = new List<string>();
			
			if (_workspaces != null)
				{
					string serverPath = null;
					
					foreach(Workspace w in _workspaces)
						{
							serverPath = w.TryGetServerItemForLocalItem(localPath);
							if (serverPath != null) { serverPaths.Add(serverPath); }
						}
				}
			
			return serverPaths;
		}
	}
}
