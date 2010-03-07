
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using megahistorylib;

using PrimaryIDsCont = treelib.AVLTree<int, megahistorylib.IntDescSorter>;

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
		wr.WriteLine("Parents: ");
		foreach(int p in rev.Parents) { wr.WriteLine("{0}", p); }
		
		wr.WriteLine(rev.Branch);
		wr.WriteLine("Changeset: {0}", rev.ID);
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
		/* so when we print a parent out, we have to actually
		 * switch up the printing...
		 * we print the parent's parent's first, then us.
		 */
		Revision r = _results.getRevision(csID);
		
		foreach(int pID in r.Parents) { _print(wr, csID); }
		
		_print(wr, r);
	}
	
	public void print(System.IO.TextWriter wr)
	{
		treelib.support.iterator_base<int> ptr = _results.primaryIDStart();
		treelib.support.iterator_base<int> end = _results.primaryIDEnd();
		
		if (ptr != end)
			{
				for(; ptr != end; ptr.inc())
					{
						/* these are in descending order.
						 * so first you print the top level changeset,
						 * then recursively print the rest.
						 */
						Revision r = _results.getRevision(ptr.item());
						_print(wr, r);
						
						foreach(int id in r.Parents)
							{
								_print(wr, id);
							}
					}
			}
		else { wr.WriteLine("No changesets found."); }
	}
}
