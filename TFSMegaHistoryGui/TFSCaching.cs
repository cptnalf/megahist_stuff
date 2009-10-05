
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System.Collections.Generic;

namespace tfs_fullhistory
{
	public class TFSWorkItemCache
	{
		private Dictionary<int,WorkItem> _cache = new Dictionary<int,WorkItem>();
		
		public TFSWorkItemCache() { }
		
		public WorkItem this[int id]
		{
			get
			{
				WorkItem wi = null;
				if (false == _cache.TryGetValue(id, out wi)) { wi = null; }
				return wi;
			}
		}
		
		public void add(Microsoft.TeamFoundation.VersionControl.Client.Changeset cngset)
		{
			for(int i=0; i < cngset.WorkItems.Length; ++i)
				{
					if (! _cache.ContainsKey(cngset.WorkItems[i].Id))
						{ _cache.Add(cngset.WorkItems[i].Id, cngset.WorkItems[i]); }
				}
		}
	}
}
