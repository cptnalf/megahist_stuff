
using System.Reflection;
using System.Collections.Generic;
using System.AddIn.Hosting;
using System.Collections.ObjectModel;

namespace TFSTree
{
	using Plugin = StarTree.Host.Database.Plugin;
	using Type = System.Type;
	
	internal class PluginLoader
	{
		private List<Plugin> _plugins = new List<Plugin>();
		
		internal PluginLoader() { }
		
		internal List<Plugin> getDBPlugins() { return _plugins; }
		
		internal void unload()
		{
			_plugins.Clear();
			_plugins = null;
			_plugins = new List<Plugin>();
		}
		
		internal void load(string directory)
		{
			string fullpath = System.IO.Path.GetFullPath(directory);
			
			/* rebuild the plugin cache. */
			AddInStore.Rebuild(fullpath);
			
			/* grab the list o' addins. */
			Collection<AddInToken> addins = AddInStore.FindAddIns(typeof(StarTree.Host.Database.Plugin), fullpath);
			
			foreach(AddInToken token in addins)
				{
					Plugin p = token.Activate<Plugin>( AddInSecurityLevel.Host);
					
					if (p != null) { _plugins.Add(p); }
				}
		}
	}
}

