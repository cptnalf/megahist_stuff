
using System.Collections.Generic;

#if flarg

/*
megahistory
- query
  visit target changeset
	- query parent changesets (recursive call)


 */

get item for the path given

query history for item 
walk the history list
- walk the changes in the changeset
  - querymergedetails for the changed item, if it's a merge/branch

the existing way:

query history for the path
walk the history list
- if it's a merge changeset
  - query merges.
    walk returned list
			print the top changeset
			- attempt to disassemble 


namespace megahistory
{
	/**
	 *  this stores information about merges.
	 *  parents are sources of the changes.
	 *  children are the targets (parents are merged into new child changesets)
	 */
	public class MergeGraph
	{
		private QuickGraph.BidirectionalGraph<int, QuickGraph.SEdge<int>> _graph = 
			new QuickGraph.BidirectionalGraph<int,QuickGraph.SEdge<int>>();
		private Dictionary<int, bool> _partials = new Dictionary<int,bool>();
		
		public MergeGraph() { }
		
		public bool isPartial(int cngset)
		{
			bool result;
			if (_partials.TryGetValue(cngset, out result)) { result = false; }
			return result;
		}
		
		/** add a merge changeset edge.
		 *  
		 */
		public void add(ChangesetMerge mergeCS)
		{
			_partials.Add(mergeCS.SourceVersion, mergeCS.Partial);
			
			/* this creates a directed edge:
			 * from the parent (sourceversion) to the child (targetversion)
			 */
			QuickGraph.SEdge<int> edge = 
				new QuickGraph.SEdge<int>(mergeCS.SourceVersion, mergeCS.TargetVersion);
			_graph.AddEdge(edge);
		}
		
		public void add(int csSrc, int csMergedTo)
		{
			QuickGraph.SEdge<int> foo = new QuickGraph.SEdge<int>(csSrc, csMergedTo);
			
			_graph.AddEdge(foo);
		}
		
		public List<int> getChildren(int src)
		{
			List<int> changes = new List<int>();
			foreach(QuickGraph.SEdge<int> edge in _graph.InEdges(src)) { changes.Add(edge.Target); }
			return changes;
		}
		
		public List<int> getParents(int dest)
		{
			List<int> changes = new List<int>();
			
			foreach(QuickGraph.SEdge<int> edge in _graph.OutEdges(dest)) { changes.Add(edge.Source); }
			
			return changes;
		}
	}
}
#endif