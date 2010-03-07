
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using megahistorylib;

using PrimaryIDsCont = treelib.AVLTree<int, megahistorylib.IntDescSorter>;

/** print the changeset when we see it.
 */
class HistoryViewer : VisitorBase
{
	public enum Printwhat : short
	{
		None,
		NameOnly,
		NameStatus
	}
	
	private Printwhat _printWhat = Printwhat.None;
	
	public HistoryViewer(Printwhat printWhat) { _printWhat = printWhat; }
	
	private void _print(System.IO.TextWriter wr, CSWrapper cs)
	{
		wr.WriteLine("Parents: ");
		foreach(int p in cs.Parents) { wr.WriteLine("{0}", p); }
		
		wr.WriteLine(cs.Branch);
		wr.WriteLine("Changeset: {0}", cs.ID);
		wr.WriteLine("Author: {0}", cs.User);
		wr.WriteLine("Date: {0}", cs.CreationDate);
		
		/* @TODO pretty print the comment */
		wr.WriteLine(cs.Comment);
		wr.WriteLine();
		
		if (_printWhat != Printwhat.None)
			{
				for(int i=0; i < cs.ChangesCount; ++i)
					{
						switch (_printWhat)
							{
								case(Printwhat.NameOnly):
									{ wr.WriteLine("{0}", cs[i].Item.ServerItem); break; }
								case(Printwhat.NameStatus):
									{ wr.WriteLine("{0:20} {1}", cs[i].ChangeType, cs[i].Item.ServerItem); break; }
							}
					}
				
				wr.WriteLine();
			}
	}
	
	public void print(System.IO.TextWriter wr)
	{
		for(PrimaryIDsCont.iterator it = _primaryIDs.begin();
				it != _primaryIDs.end();
				++it)
			{
				CSWrapper w = this.find(it.item());
				_print(wr, w);
			}
		
		if (_primaryIDs.empty()) { wr.WriteLine("No changesets found."); }
	}
}
