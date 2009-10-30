
using System.Collections.Generic;
using Microsoft.TeamFoundation.VersionControl.Client;
using System.Linq;

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
		
		public IEnumerable<Workspace> Workspaces { get { return _workspaces; } }
		
		public string getLocalPath(string workspaceName, string tfsPath)
		{
			string localPath = null;
			Workspace w = null;
			var foo =
				from ws in _workspaces
				where ws.DisplayName == workspaceName
				select ws;
			
			foreach(Workspace i in foo) { w = i; }
			
			localPath = w.TryGetLocalItemForServerItem(tfsPath);
			if (localPath == null ) { localPath = tfsPath; }
			
			return localPath;
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
