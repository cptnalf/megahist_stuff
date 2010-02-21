
using Microsoft.Glee.Drawing;

namespace StarTree.Utils
{	
	using Revision = StarTree.Host.Database.Revision;
	
	internal static class SnapshotSaver
	{
		internal static void Save(StarTree.Host.Database.Snapshot snapshot, Graph graph)
		{
			foreach(object val in graph.NodeMap.Values)
				{
					DrawingObject obj = val as DrawingObject;
					if (obj != null)
						{
							Revision r = obj.UserData as Revision;
							if (r != null) { snapshot.add(r); }
						}
				}
		}
	}
}
