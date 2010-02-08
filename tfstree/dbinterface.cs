
using System.Collections.Generic;

namespace TFSTree
{
	public interface RevisionDB
	{
		List<string> GetBranches();
		Dictionary<string, Revision> GetLatestRevisionIDs(string branch, int limit);
		Revision GetRevision(string id);
	}
}
