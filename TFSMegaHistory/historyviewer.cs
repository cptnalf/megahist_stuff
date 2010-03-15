
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using megahistorylib;

using PrimaryIDsCont = treelib.AVLTree<int, treelib.sorters.IntDesc>;

/** print the changeset when we see it.
 */
class HistoryViewer
{
	public enum Printwhat : short
	{
		None,
		NameOnly,
		NameStatus
	}

	private Dictionary<string, int> _branch2Level = new Dictionary<string, int>();	
	private IMergeResults _results;
	private Printwhat _printWhat = Printwhat.None;
	
	public HistoryViewer(Printwhat printWhat, IMergeResults results)
	{
		_printWhat = printWhat;
		_results = results;
	}
	
	private void _print(System.IO.TextWriter wr, Revision rev)
	{
		wr.WriteLine(rev.Branch);
		wr.WriteLine("Changeset: {0}", rev.ID);
		
		wr.WriteLine("Parents: ");
		foreach(int p in rev.Parents) { wr.WriteLine("{0}", p); }
		
		wr.WriteLine("Author: {0}", rev.User);
		wr.WriteLine("Date: {0}", rev.CreationDate);
		
		/* @TODO pretty print the comment */
		wr.WriteLine(rev.Comment);
		wr.WriteLine();
		
		if (_printWhat != Printwhat.None)
			{
				for(int i=0; i < rev.ChangesCount; ++i)
					{
						switch (_printWhat)
							{
								case(Printwhat.NameOnly):
									{ wr.WriteLine("{0}", rev.Changes[i].Item.ServerItem); break; }
								case(Printwhat.NameStatus):
									{ 
										wr.WriteLine("{0:20} {1}", 
										             rev.Changes[i].ChangeType, rev.Changes[i].Item.ServerItem); 
										break;
									}
							}
					}
				
				wr.WriteLine();
			}
	}
	
	private void _print(System.IO.TextWriter wr, int csID)
	{
		/* print out a descending order changeset.
		 * first we print ourselves.
		 * then we print our parents, in descending order, recursively.
		 */
		
		Revision r = _results.getRevision(csID);
		
		if (r != null)
			{
				/* print out us. */
				_print(wr, r);
				
				/* then print out our parents, in descending order. */
				PrimaryIDsCont desc = new PrimaryIDsCont();
				PrimaryIDsCont mybranch =new PrimaryIDsCont();
				
				/* separate merge parents from regular history */
				foreach(int pID in r.Parents)
					{
						Revision pr = _results.getRevision(pID);
						
						if (pr != null)
							{
								/* separate changesets into 2 piles:
								 * my branch parents,
								 * not my branch parents.
								 */
								if (pr.Branch == r.Branch) { mybranch.insert(pID); }
								else { desc.insert(pID); }
							}
					}
				
				if (! desc.empty())
					{
						/* print all of my parents not in my branch.
						 * this would be changesets merged into my branch
						 * by this changeset.
						 */
						foreach(int pID in desc) { _print(wr, pID); }
					}
				
				if (!mybranch.empty())
					{
						/* now print all of my parents in my branch.
						 * this should be just one changeset.
						 */
						foreach(int pID in mybranch) { _print(wr, pID); }
					}
			}
		else
			{
				/* we failed to find the requested chagneset id.
				 * this means most likely that the parents were
				 * properly decomposed from the merge changeset, 
				 * but that none of the children were retrieved and processed.
				 */
			}
	}
	
	public void print(System.IO.TextWriter wr)
	{		
		int id = _results.firstID;
		
		if (id > 0) { _print(wr, id); }
		else { wr.WriteLine("No changesets found."); }
	}
}
