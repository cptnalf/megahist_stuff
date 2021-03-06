﻿
namespace StarTree.Host.Adapter.Database
{
	[System.AddIn.Pipeline.HostAdapter]
	public class HostAdapter : StarTree.Host.Database.Plugin
	{
		private StarTree.Contracts.Database.IPlugin _plugin;
		private System.AddIn.Pipeline.ContractHandle _handle;
		
		public HostAdapter(StarTree.Contracts.Database.IPlugin plugin)
		{
			_plugin = plugin;
			_handle = new System.AddIn.Pipeline.ContractHandle(plugin);
		}

		public override string[] branches()
		{
			return _plugin.branches();
		}

		public override void close()
		{
			_plugin.close();
		}
		public override void open()
		{
			_plugin.open();
		}

		public override StarTree.Host.Database.DisplayNames names
		{
			get 
			{
				Converter.DisplayNames cdn = _plugin.names;
				return cdn;
			}
		}

		public override string currentName { get { return _plugin.currentName; } }

		public override StarTree.Host.Database.Snapshot getBranch(string branch, long limit)
		{
			StarTree.Host.Database.Snapshot snapshot = new StarTree.Host.Database.Snapshot(this);
			byte[] bytes = _plugin.getBranch(branch, limit);
			
			snapshot.load(bytes);
			
			return snapshot;
		}

		public override StarTree.Host.Database.Revision getRevision(string id)
		{
			Host.Database.Revision hr = null;
			Contracts.Database.Revision rev = _plugin.getRevision(id);
			
			if (rev != null)
				{
					Converter.Revision revcon = rev; 
					hr = revcon;
				}
			
			return hr;
		}
		
		public override StarTree.Host.Database.Snapshot queryMerges(StarTree.Host.Database.Revision rev)
		{
			Converter.Revision revCon = rev;
			StarTree.Host.Database.Snapshot sn = new StarTree.Host.Database.Snapshot(this);
			byte[] bytes = _plugin.queryMerges(revCon);
			sn.load(bytes);
			
			return sn;
		}
	}
}
