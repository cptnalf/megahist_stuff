
namespace StarTree.Plugin.Adapter.Database
{
	[System.AddIn.Pipeline.AddInAdapter]
	public class PluginAdapter : System.AddIn.Pipeline.ContractBase, Contracts.Database.IPlugin
	{
		private StarTree.Plugin.Database.Plugin _plugin;
		
		public PluginAdapter(StarTree.Plugin.Database.Plugin plugin)
		{
			_plugin = plugin;
		}
#region IPlugin Members
		
		public StarTree.Contracts.Database.DisplayNames names
		{
			get
				{
					StarTree.Plugin.Adapter.Converters.DisplayNames dn = _plugin.names;
					return dn;
				}
		}
		
		public string currentName { get { return _plugin.currentName; } }
		
		public void open()
		{
			_plugin.open();
		}

		public void close()
		{
			_plugin.close();
		}

		public string[] branches()
		{
			return _plugin.branches();
		}

		public byte[] getBranch(string branch, long limit)
		{
			StarTree.Plugin.Database.Snapshot snapshot = _plugin.getBranch(branch, limit);
			
			return snapshot.serialize();
		}

#endregion
	}
}
