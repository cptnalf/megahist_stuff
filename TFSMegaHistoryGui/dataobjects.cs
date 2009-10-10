
using System;

namespace tfs_fullhistory
{
	namespace DataObjects
	{
		public class Commit : Visitor.PatchInfo
		{
			private List<Commit> _commits = new List<Commit>();
			
			public List<Microsoft.TeamFoundation.VersionControl.Client.Changeset> parts
			{ get { return _commits; } }
			
			public int size { get { return _commits.Count; } }
			
			public Commit(Microsoft.TeamFoundation.VersionControl.Client.Changeset cs)
			{
				_cs = cs;
			}
			
			public void add(Microsoft.TeamFoundation.VersionControl.Client.Changeset cs)
			{ _commits.Add(cs); }
										
		}
	}
}
