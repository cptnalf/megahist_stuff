
/*
 */
using System.Collections.Generic;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace TFSTree.Databases
{
	using IEnumerable = System.Collections.IEnumerable;
	using RevisionCont = treelib.AVLTree<Revision>;
	using BranchContainer = treelib.AVLTree<string, treelib.StringSorterInsensitive>;
	using ChangesetIdx = treelib.AVLDict<int,Revision,treelib.IntSorterDesc>;
	using BranchChangesets = 
		treelib.AVLDict<string,treelib.AVLDict<int,Revision,treelib.IntSorterDesc>,treelib.StringSorterInsensitive>;

	internal class TFSDB : IRevisionRepo, megahistory.IVisitor<Revision>
	{
		private ChangesetIdx _changesetIdx = new ChangesetIdx();
		private BranchContainer _branches;
		private BranchChangesets _branchChangesets = new BranchChangesets();
		private string _tfsServerName;
		private VersionControlServer _vcs;
		
		internal TFSDB() {	}
		
		public string FileName { get { return _tfsServerName; } }
		
		public System.Collections.Generic.IEnumerable<string> BranchNames
		{
			get
				{
					if (_branches == null) { _queryBranches(); }
					return _branches;
				}
		}
		
		public Revision rev(string id)
		{
			Revision rev = null;
			int csid = System.Int32.Parse(id);
			ChangesetIdx.iterator it = _changesetIdx.find(csid);
			if (it != _changesetIdx.end()) { rev = it.value(); }
			return rev;
		}
		
		public System.Collections.Generic.Dictionary<string,Revision> revs(string branch, ulong limit)
		{
			System.Collections.Generic.Dictionary<string,Revision> revisions = 
				new System.Collections.Generic.Dictionary<string,Revision>();
			
			BranchChangesets.iterator it = _branchChangesets.find(branch);
			if (it != _branchChangesets.end())
				{
					if (it.value().size() < limit) { _requery(branch, limit); }
					
					ulong count = (it.value().size() > limit) ? limit : it.value().size();
					ChangesetIdx.iterator csit = it.value().begin();
					
					for(ulong i=0; i < count; ++i, ++csit)
						{
							if (csit == it.value().end()) { break; }
							revisions.Add(csit.value().ID, csit.value());
						}
				}
			else
				{
					/* try to populate it... */
					_getRevisions(branch, limit);
				}
			
			return revisions;
		}
		
		public void load(string filename)
		{
			_vcs = megahistory.Utils.GetTFSServer(filename);
		}
		
		public void loadfolder(string filename) { }
		
		private void _queryBranches()
		{
			_branches = new BranchContainer();
			
			_branches.insert("$/IGT_0803/development/dev_adv/EGS/");
			_branches.insert("$/IGT_0803/development/dev_build/EGS/");
			_branches.insert("$/IGT_0803/development/dev_ABS/EGS/");
			_branches.insert("$/IGT_0803/development/dev_sb/EGS/");
			_branches.insert("$/IGT_0803/main/EGS/");
			_branches.insert("$/IGT_0803/release/EGS8.2/dev_sp/EGS/");
		}
		
		private void _requery(string branch, ulong limit)
		{
			/* this would grab more revisions, starting where we left off. */
		}
		
		public Revision visit(string branch, Changeset cs)
		{
			Revision rev = new Revision(cs.ChangesetId, branch,
																	cs.Owner, cs.CreationDate,
																	cs.Comment);

			ChangesetIdx.iterator csit = _changesetIdx.find(cs.ChangesetId);
			if (csit != _changesetIdx.end())
				{
					_changesetIdx.insert(cs.ChangesetId, rev);

					BranchChangesets.iterator it = _branchChangesets.find(branch);
					if (it != _branchChangesets.end())
						{ it.value().insert(cs.ChangesetId, rev); }
					else
						{
							ChangesetIdx revisionsIdx = new ChangesetIdx();
							revisionsIdx.insert(cs.ChangesetId, rev);
							_branchChangesets.insert(branch, revisionsIdx);
						}
				} 
			return rev;
		}
		
		public void addParent(Revision rev, int parentID)
		{
			rev.addParent(parentID.ToString());
		}
		
		private void _getRevisions(string branch, ulong limit)
		{
			megahistory.MergeHist<Revision> mergeHist = new megahistory.MergeHist<Revision>(_vcs, this);
			
			IEnumerable foo =
				_vcs.QueryHistory(branch, VersionSpec.Latest, 0, RecursionType.Full,
													null, null, null, /* user, from ver, to ver */
													(int)limit, 
													true, false, false); 
			/* inc changes, slot mode, inc download info. */
			
			foreach (object o in foo)
				{
					Changeset cs = o as Changeset;
					
					/* i need to find a path to use as a base item for this merge history query. 
					 *
					 * this could skew the results a little bit.
					 * unfortunately, to support renames, i need to get the previous name, 
					 * which might be in the changeset history (?)
					 * 
					 * however, this won't detect situations where the user checked-in
					 * changes on two separate branches.
					 */
					List<string> branchParts = megahistory.Utils.FindChangesetBranches(cs);
					
					for(int bpi = 0; bpi < branchParts.Count; ++bpi)
						{
							Item itm = _vcs.GetItem(branchParts[bpi], new ChangesetVersionSpec(cs.ChangesetId));
							
							mergeHist.queryMerge(cs, itm, 1);
						}
				}
		}		
	}
	
}