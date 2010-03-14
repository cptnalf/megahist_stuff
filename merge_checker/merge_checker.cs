
using System;
using Microsoft.TeamFoundation.VersionControl.Client;
using System.Reflection;
using saastdlib;

[assembly: AssemblyTitle("merge checker")]
[assembly: AssemblyDescription("checks merge dependencies.")]
[assembly: AssemblyConfiguration("debug")]
[assembly: AssemblyCompany("flarg")]
[assembly: AssemblyProduct("merge checker")]
[assembly: AssemblyCopyright("flarg")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyVersion("1.0.0.0")]


namespace MergeChecker
{
	using ItemMap = treelib.TreapDict<Item, treelib.AVLTree<int>, ItemSorter>;
	using IntList = treelib.AVLTree<int>;
	using ItemList = treelib.AVLTree<Item,ItemSorter>;
	using CSItemMap = treelib.TreapDict<int, treelib.AVLTree<Item,ItemSorter>>;
	
	
	public class ItemSorter : System.Collections.Generic.IComparer<Item>
	{
		public ItemSorter() { }
		
		public int Compare(Item one, Item two)
		{
			return one.ItemId.CompareTo(two.ItemId);
		}
	}
	
	internal class merge_checker
	{
		private static VersionControlServer vcs;
		
		private static void _ProcessDeps(System.IO.TextWriter wr, ItemMap deps, CSItemMap keyChangesets)
		{
			/* print out only our changesets, 
			 * details:
			 *  the dependices they have on other changesets,
			 *  what file is the dependency.
			 */
			
			for(CSItemMap.iterator it = keyChangesets.begin();
					it != keyChangesets.end();
					++it)
				{
					wr.WriteLine("changeset {0}", it.item());
					
					foreach(Item item in it.value())
						{
							ItemMap.iterator imit = deps.find(item);
							if (imit != deps.end())
								{
									if (imit.value().size() > 1)
										{
											wr.WriteLine(imit.item().ServerItem);
											foreach(int csID in imit.value())
												{
													wr.WriteLine(csID);
												}
											wr.WriteLine();
										}
								}
						}
					
					wr.WriteLine();
				}
		}
		
		private static void _GenerateDeps(ItemMap deps, IntList intTreap)
		{
			for(IntList.iterator it = intTreap.begin();
					it != intTreap.end();
					++it)
				{
					Changeset cs = vcs.GetChangeset(it.item());
					
					foreach(Change cng in cs.Changes)
						{
							ItemMap.iterator imit = deps.find(cng.Item);
							if (imit == deps.end())
								{
									IntList changesetlist = new IntList();
									
									changesetlist.insert(cs.ChangesetId);
									
									deps.insert(cng.Item, changesetlist);
								}
							else
								{
									IntList.iterator csit = imit.value().find(cs.ChangesetId);
									if (csit == imit.value().end())
										{ imit.value().insert(cs.ChangesetId); }
								}
						}
				}
		}
		
		private static IntList _ReadFile(System.IO.TextReader rdr)
		{
			System.Text.RegularExpressions.Regex lineRe = new System.Text.RegularExpressions.Regex("^[ \t]+([0-9]+)");
			string line;
			IntList cslist = new IntList();
			
			while( (line = rdr.ReadLine()) != null)
				{
					System.Text.RegularExpressions.Match m = lineRe.Match(line);
					
					if (m != null)
						{
							string part = m.Groups[1].Value;
							if (!string.IsNullOrEmpty(part) )
								{
									cslist.insert(Int32.Parse(part));
								}
						}
				}
			
			return cslist;
		}
		
		private static int Main(string[] args)
		{
			ArgParser ap = new ArgParser();
			
			ap.add(new Arg('s', "server", "tfs server name", "name of the tfs server.", null));
			ap.add(new Arg('c', "candidates", "candidates file name",
										 "list of stuff that hasn't moved from one branch to the other.", null));
			
			System.Collections.Generic.List<int> unknownArgs;
			bool argOK = ap.parse_args(args, out unknownArgs);
			
			if (!argOK || unknownArgs.Count < 1)
				{
					ap.print_help("", new string[] { "foo!"});
					return 1;
				}
			
			vcs = tfsinterface.SCMUtils.GetTFSServer(ap.get_arg<Arg>("server"));
			IntList mergeCandidates;
			IntList toMerge = new IntList();
			
			{
				System.IO.TextReader rdr = 
					new System.IO.StreamReader(new System.IO.FileStream(ap.get_arg<Arg>("candidates"), 
					System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite));
				mergeCandidates = _ReadFile(rdr);
			}
			
			foreach(int i in unknownArgs)
				{
					int csID = Int32.Parse(args[i]);
					
					IntList.iterator it = toMerge.find(csID);
					if (it == toMerge.end())
						{
							toMerge.insert(csID);
						}
				}
			
			ItemMap deps = new ItemMap();
			_GenerateDeps(deps, mergeCandidates);
			
			CSItemMap targetCSs = new CSItemMap();
			foreach(int csID in toMerge)
				{
					Changeset cs = vcs.GetChangeset(csID);
					ItemList il = new ItemList();
					
					foreach(Change cng in cs.Changes)
						{
							il.insert(cng.Item);
						}
					
					targetCSs.insert(csID, il);
				}
			
			Console.WriteLine("merge ordering:");

			_ProcessDeps(Console.Out, deps, targetCSs);
			
			return 0;
		}
	}
}
