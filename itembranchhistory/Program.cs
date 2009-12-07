using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace itembranchhistory
{
	using megahistorylib;
	
	class Program
	{
		static int Main(string[] args)
		{
			ArgParser argparser = new ArgParser();
			argparser.add(
										new Arg('s', "server-name", "tfs server to connect to", 
														new string[] { "the name of the server" }, null) 
										);
			
			List<int> unknownArgs;
			bool argError = ! argparser.parse_args(args, out unknownArgs);
			
			if (argError 
					|| unknownArgs.Count < 0)
				{
					argparser.print_help("[item path]", 
															 new string[]
							{
								"item path - tfs-server path of an item to check merge history for.",
								null,
								"test an item-based querying solution.",
								"e.g. itembranchhistory -s foo $/foo/bar.cs",
							});
					
					return 1;
				}
			
			string server = (string)argparser.get_arg<Arg>(0);
			string path = args[unknownArgs[0]];
			
			Microsoft.TeamFoundation.VersionControl.Client.VersionControlServer vcs =
				megahistory.Utils.GetTFSServer(server);
			
			BranchHistory bh = new BranchHistory();
			Microsoft.TeamFoundation.VersionControl.Client.VersionSpec ver =
				Microsoft.TeamFoundation.VersionControl.Client.VersionSpec.Latest;
			Microsoft.TeamFoundation.VersionControl.Client.Item item =
				vcs.GetItem(path, ver, 0, false);
			
			treelib.AVLTree<Microsoft.TeamFoundation.VersionControl.Client.Changeset> changes = 
				bh.decompose(vcs, item);
			
			if (changes.empty())
				{
					Console.WriteLine("no changesets found.");
				}
			
			for(treelib.AVLTree<Microsoft.TeamFoundation.VersionControl.Client.Changeset>.iterator it =
						changes.begin();
					it != changes.end();
					++it)
				{
					Microsoft.TeamFoundation.VersionControl.Client.Change cng = it.item().Changes[0];
					
					Console.WriteLine("{0} {1}",
														cng.ChangeType.ToString(),
														cng.Item.ServerItem);
				}
			
			return 0;
		}
	}
}
