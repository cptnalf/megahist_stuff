
using System;
using Microsoft.TeamFoundation.VersionControl.Client;
using saastdlib;

namespace MergeChecker
{
	using ItemMap = treelib.TreapDict<Item, treelib.AVLTree<int>, tfsinterface.sorters.ItemSorter>;
	using IntList = treelib.AVLTree<int>;
	using ItemList = treelib.AVLTree<Item, tfsinterface.sorters.ItemSorter>;
	using CSItemMap = treelib.TreapDict<int, treelib.AVLTree<Item, tfsinterface.sorters.ItemSorter>>;
		
	internal class merge_checker
	{
		/// <summary>
		/// print dependencies.
		/// </summary>
		/// <param name="wr">place to pretty-print them to</param>
		/// <param name="deps">the map of item->changeset list</param>
		/// <param name="keyChangesets">the changeset->item map for the changesets we are interested in</param>
		private static void _PrintDeps(System.IO.TextWriter wr, ItemMap deps, CSItemMap keyChangesets)
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
											foreach(int csID in imit.value()) { wr.WriteLine(csID); }
											wr.WriteLine();
										}
								}
						}
					
					wr.WriteLine();
				}
		}
		
		private static void _GenerateDeps(VersionControlServer vcs, ItemMap deps, IntList intTreap)
		{
			/* walk through the changeset list. */
			for(IntList.iterator it = intTreap.begin();
					it != intTreap.end();
					++it)
				{
					/* grab the changeset. */
					Changeset cs = vcs.GetChangeset(it.item());
					
					foreach(Change cng in cs.Changes)
						{
							/* now dump each item in the changeset into the dependency list. */
							ItemMap.iterator imit = deps.find(cng.Item);
							if (imit == deps.end())
								{
									/* new one. */
									IntList changesetlist = new IntList();
									changesetlist.insert(cs.ChangesetId);
									deps.insert(cng.Item, changesetlist);
								}
							else
								{
									/* hey, it already exists, so find it and dump the changeset in there. */
									IntList.iterator csit = imit.value().find(cs.ChangesetId);
									if (csit == imit.value().end())
										{ imit.value().insert(cs.ChangesetId); }
								}
						}
				}
		}
		
		/// <summary>
		/// read in a file which is the result of running:
		/// tf merge /recursive /candidate /noprompt &lt;src&gt; &lt;target&gt;
		/// something like this:
		///   12345   username   2009/01/01
		///   
		/// this function is only interested in the first number.
		/// </summary>
		/// <param name="rdr"></param>
		/// <returns></returns>
		private static IntList _ReadFile(System.IO.TextReader rdr)
		{
			System.Text.RegularExpressions.Regex lineRe = new System.Text.RegularExpressions.Regex("^[ \t]+([0-9]+)");
			string line;
			IntList cslist = new IntList();
			
			while( (line = rdr.ReadLine()) != null)
				{
					System.Text.RegularExpressions.Match m = lineRe.Match(line);
					
					if (m != null && m.Success)
						{
							string part = m.Groups[1].Value;
							cslist.insert(Int32.Parse(part));
						}
				}
			
			return cslist;
		}
		
		private static int Main(string[] args)
		{
			ArgParser ap = new ArgParser();
			
			ap.add(new Arg('s', "server", "tfs server name", "name of the tfs server.", null));
			ap.add(new Arg('c', "candidates", "candidates file name",
										 new string[] {
										 "list of stuff that hasn't moved from one branch to the other.",
										 " filename of '-' means stdin.",
										 " this can be obtained with a command like:",
										 " tf merge /recursive /candidate /noprompt <source> <destination>",
										 }, null));
			
			System.Collections.Generic.List<int> unknownArgs;
			bool argOK = ap.parse_args(args, out unknownArgs);
			
			if (!argOK || unknownArgs.Count < 1)
				{
					ap.print_help("", new string[] { "foo!"});
					return 1;
				}
			
			IntList mergeCandidates;
			IntList toMerge = new IntList();
			{
				System.IO.TextReader rdr = null;
				if (ap.get_arg<Arg>("candidates") != "-")
					{
						rdr = new System.IO.StreamReader(new System.IO.FileStream(ap.get_arg<Arg>("candidates"), 
							System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite));
					}
				else { rdr = Console.In; }
				
				mergeCandidates = _ReadFile(rdr);
			}
			
			/* process the unknown arguments.
			 * these should all be target changesets.
			 */
			foreach(int i in unknownArgs)
				{
					int csID = Int32.Parse(args[i]);
					
					IntList.iterator it = toMerge.find(csID);
					if (it == toMerge.end()) { toMerge.insert(csID); }
				}
			
			ItemMap deps = new ItemMap();
			VersionControlServer vcs = tfsinterface.SCMUtils.GetTFSServer(ap.get_arg<Arg>("server"));
			_GenerateDeps(vcs, deps, mergeCandidates);
			
			/* now pull information on the target changesets.
			 * 
			 * note:
			 * we really already have pulled this information,
			 * but it would be a little difficult to dual-purpose the '_GenerateDeps' function.
			 * it would also make it look really confusing.
			 */
			CSItemMap targetCSs = new CSItemMap();
			foreach(int csID in toMerge)
				{
					Changeset cs = vcs.GetChangeset(csID);
					ItemList il = new ItemList();
					
					foreach(Change cng in cs.Changes) { il.insert(cng.Item); }
					
					targetCSs.insert(csID, il);
				}
			
			Console.WriteLine("merge ordering:");
			_PrintDeps(Console.Out, deps, targetCSs);
			
			return 0;
		}
	}
}
