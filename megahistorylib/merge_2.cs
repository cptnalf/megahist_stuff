
using Microsoft.TeamFoundation.VersionControl.Client;

namespace megahistorylib
{
	public class BranchHistory
	{
		public treelib.AVLTree<Changeset> decompose(VersionControlServer vcs, Item item)
		{
			treelib.AVLTree<Changeset> changesets = new treelib.AVLTree<Changeset>();
			ChangesetVersionSpec ver = new ChangesetVersionSpec(item.ChangesetId);
			RecursionType recurse = RecursionType.Full;
			
			if (item.ItemType == ItemType.File) { recurse = RecursionType.None; }
			
			ChangesetMergeDetails mergedetails = 
				vcs.QueryMergesWithDetails(null, null, 0, 
																	 item.ServerItem, ver, item.DeletionId,
																	 ver, ver, recurse);
			
			foreach(Changeset cs in mergedetails.Changesets)
				{
					changesets.insert(cs);
				}
			
			return changesets;
		}
	}
}
