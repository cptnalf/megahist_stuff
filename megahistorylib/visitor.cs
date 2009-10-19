
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;

namespace megahistory
{

	/** what happens when we see a changeset?
	 */
	public abstract class Visitor
	{
		/** private version of a 'Changeset' object */
		public class PatchInfo
		{
			private int _parent;
			private Changeset _cs;
			private List<string> _treeBranches;
			private List<PatchInfo> _commits = new List<PatchInfo>();
		
			public List<PatchInfo> parts
			{ get { return _commits; } }
		
			public int partCount { get { return _commits.Count; } }
		
			public int parent { get { return _parent; } set { _parent = value; } }
			public Changeset cs { get { return _cs; } set { _cs = value; } }
			public List<string> treeBranches
			{ get { return _treeBranches; } set { _treeBranches = value; } }
		
			public int id { get { return _cs.ChangesetId; } }
			public string user { get { return _cs.Owner; } }
			public DateTime creationDate { get { return _cs.CreationDate; } }
			public string comment { get { return _cs.Comment; } }
		
			public PatchInfo(Changeset c) : this(0, c, null) { }
		
			public PatchInfo(int p, Changeset c, List<string> tb)
			{
				parent = p;
				cs = c;
				treeBranches = tb;
			}
		
			public void add(PatchInfo patch)
			{ _commits.Add(patch); }
		
			public void print(System.IO.TextWriter writer) { print(0, writer); }
		
			public void print(int parentID, System.IO.TextWriter writer)
			{
				int p = 0;
			
				if (parentID != 0) { p = parentID; }
				else { p = parent; }
			
				if (p != 0) { writer.WriteLine("Parent: {0}", p); }
				writer.WriteLine("Changeset: {0}", cs.ChangesetId);
				writer.WriteLine("Author: {0}", cs.Owner);
				writer.WriteLine("Date: {0}", cs.CreationDate);
			
				if (treeBranches != null)
					{ for(int i=0; i < treeBranches.Count; ++i) { writer.WriteLine(treeBranches[i]); } }
			
				/* @TODO pretty print the comment */
				writer.WriteLine(cs.Comment);
				writer.WriteLine();
			}
		};
	
		protected Dictionary<int,PatchInfo> _cache = new Dictionary<int,PatchInfo>();
		protected List<string> _branches = new List<string>();
	
		public int visitedCount() { return _cache.Count; }
		public PatchInfo this[int changesetid]
		{
			get
				{
					PatchInfo p;
					_cache.TryGetValue(changesetid, out p);
					return p;
				}
		}
		
		/** reset the visited statistics.
		 */
		public virtual void reset()
		{
			_cache.Clear();
			_branches.Clear();
		}
		
		/** have we already visited this changeset?
		 */
		public bool visited(int changesetid) { return _cache.ContainsKey(changesetid); }
		/** have we already visited this branch? */
		public bool visited(string branch) { return _branches.BinarySearch(branch) >= 0; }
	
		public void visit(int parentID, Changeset cs) { visit(parentID, cs, null); }
		public virtual void visit(int parentID, Changeset cs, List<string> branches)
		{
			PatchInfo p;
		
			if (_cache.TryGetValue(cs.ChangesetId, out p)) 
				{
					_seen(parentID, p);
				}
			else
				{
					/* only print stuff we haven't already seen. */
					List<string> treeBranches = branches;
					if (treeBranches == null) { treeBranches = Utils.FindChangesetBranches(cs); }
					p = new PatchInfo(parentID, cs, treeBranches);
				
					_cache.Add(p.cs.ChangesetId, p);
				
					for(int i=0; i<treeBranches.Count; ++i)
						{
							/* insertion could be sped up using BinarySearch, but this keeps things simple. */
							if (_branches.BinarySearch(treeBranches[i]) < 0)
								{
									_branches.Add(treeBranches[i]);
									_branches.Sort();
								}
						}
				
					_visit(p);
				}
		}
	
		protected abstract void _visit(PatchInfo p);
		protected abstract void _seen(int parentID, PatchInfo p);
	
		/* for errors. */
		public abstract void visit(int parentID, int changesetID, Exception e);
	}

	/*
	 * so for a nice changeset tree like :
	 *    cm
	 *    /\
	 *  cm  c1
	 *  /\
	 * c  cm
	 *    /\
	 *   c  c1
	 * 
	 */
}
