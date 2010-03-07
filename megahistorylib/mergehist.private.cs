
using System.Collections.Generic;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace megahistorylib
{
	using SortedPaths_T = treelib.AVLTree<string, treelib.StringSorterInsensitive>;
	using ChangesetDict_T =
		treelib.AVLDict<int, treelib.AVLTree<string, treelib.StringSorterInsensitive>>;
	
	public partial class MegaHistory
	{
		private IVisitor _visitor;
		private VersionControlServer _vcs;
		private bool _threaded = true;
		private int _baseDistance = 3;
		
		private ulong _qc = 0;
		private saastdlib.Timer _qt = new saastdlib.Timer();
		
		private ulong _gic = 0;
		private saastdlib.Timer _git = new saastdlib.Timer();

		private ulong _gcc = 0;
		private saastdlib.Timer _gct = new saastdlib.Timer();
		
		private Item _getItem(string targetPath, int csID, int deletionID, bool downloadInfo)
		{
			VersionSpec targetVer = new ChangesetVersionSpec(csID);
			return _getItem(targetPath, targetVer, deletionID, downloadInfo);
		}
		
		private Item _getItem(string targetPath, VersionSpec targetVer, int deletionID, bool downloadInfo)
		{
			saastdlib.Timer t = new saastdlib.Timer();
			
			t.start();
			Item itm = _vcs.GetItem(targetPath, targetVer, deletionID, downloadInfo);
			t.stop();
			
			lock(_git)
				{
					++_gic;
					_git.TotalT += t.DeltaT;
				}
			
			return itm;
		}
		
		private Item _getItem(string targetPath) { return _vcs.GetItem(targetPath); }

		private void _queueOrVisit(QueryProcessor qp, Changeset cs, int distance, string branch)
		{
			/* ok, ok, we really don't have it... 
			* didn't find it, so do the first level query. 
			*/
			if (!_handleVisit(qp, cs, distance))
				{
					/* manufacture a visit since this changeset
					* has no merge actions to get branches from
					*/
					if (branch == null)
						{
							/* determine a branch from the changesets's changes 
							 * we're not queuing it, so we look for _all_ branches.
							 * (eg, 2nd level :[ ) 
							 */
							List<string> branches =
							tfsinterface.Utils.FindChangesetBranches(cs, (cng) => true);
							
							foreach (string b in branches) { _visitor.construct(b, cs); }
						}
					else { _visitor.construct(branch, cs); }
				}
		}
		
		private ChangesetDict_T _processDetails(int csID, ChangesetMergeDetails mergedetails)
		{
			ChangesetDict_T visitedItems = new ChangesetDict_T();
			/* this should always be 1 when the targetpath is a file. */
			foreach (ItemMerge m in mergedetails.MergedItems)
				{
					if (m.TargetVersionFrom != csID)
						{
							Logger.logger.ErrorFormat("merged to a different changeset? {0}=>{1} vs {2}",
																				m.SourceVersionFrom, m.TargetVersionFrom,
																				csID);
						}

					/* pull a list of changesets we want to visit from the merged items we get. */
					ChangesetDict_T.iterator dictIt = visitedItems.find(m.SourceVersionFrom);

					if (dictIt == visitedItems.end())
						{
							SortedPaths_T itemList = new SortedPaths_T();

							/* add the new item. */
							itemList.insert(m.SourceServerItem);
							visitedItems.insert(m.SourceVersionFrom, itemList);
						}
					else { dictIt.value().insert(m.SourceServerItem); }
				}
			return visitedItems;
		}

		private bool _handleVisit(QueryProcessor qp, Changeset cs, int depth)
		{
			/* grab the branches for this changeset,
			 * looking for merge branches, because we want to decompose if possible
			 */
			/* @Note
			 * worst case here is that you do a checkin to a branch
			 * of 100k items (say an add or edit).
			 * in that case the process will probably end up trying to find branches
			 * twice :/
			 */
			List<string> branchParts = tfsinterface.Utils.FindChangesetBranches(cs);

			/* @Note
			 * i need to find a path to use as a base item for this merge history query. 
			 *
			 * this could skew the results a little bit.
			 * unfortunately, to support renames, i need to get the previous name, 
			 * which might be in the changeset history (?)
			 * 
			 * however, this won't detect situations where the user checked-in
			 * changes on two separate branches.
			 */
			for (int bpi = 0; bpi < branchParts.Count; ++bpi)
				{
					Item itm = _vcs.GetItem(branchParts[bpi],
																	new ChangesetVersionSpec(cs.ChangesetId));

					Logger.logger.DebugFormat("hv[{0},{1}]", itm.ServerItem, cs.ChangesetId);
					qp.push(cs.ChangesetId, itm, depth);
				}

			return branchParts.Count > 0;
		}
	}
}

