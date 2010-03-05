
using System.Collections.Generic;
using Microsoft.TeamFoundation.VersionControl.Client;
using System.Linq;

namespace megahistorylib
{
	/// <summary>
	/// 
	/// </summary>
	public class TFSWorkspaces
	{
		private VersionControlServer _vcs = null;
		private Workspace[] _workspaces = null;
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="vcs"></param>
		public TFSWorkspaces(VersionControlServer vcs)
		: this(vcs, System.Environment.UserName, System.Environment.MachineName) { }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="vcs"></param>
		/// <param name="userName"></param>
		/// <param name="machineName"></param>
		public TFSWorkspaces(VersionControlServer vcs, string userName, string machineName)
		{
			_vcs = vcs;
			_workspaces = vcs.QueryWorkspaces(null, userName, machineName);
		}
		
		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<Workspace> Workspaces { get { return _workspaces; } }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="workspaceName"></param>
		/// <param name="tfsPath"></param>
		/// <returns></returns>
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
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="tfsPath"></param>
		/// <returns></returns>
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
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="localPath"></param>
		/// <returns></returns>
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
