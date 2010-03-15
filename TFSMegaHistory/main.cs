/*
 * 2009-03-26 15:21:01 -0700
 * stephen.alfors@igt.com
 */


using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Reflection;
using mh_ns = megahistorylib;
using saastdlib;

namespace tfsmegahistory
{
	class main
	{
		/* ChangesetMerge.TargetVersion - the changeset which the merge took place
		 * ChangesetMerge.SourceVersion - the changeset containing the changes which we merged.
		 */
		internal struct Values
		{
			internal string server;
			internal string srcPath;
			internal VersionSpec srcVer;
			internal string target;
			internal VersionSpec targetVer;
			internal VersionSpec fromVer;
			internal VersionSpec toVer;
			internal VersionControlServer vcs;
			internal HistoryViewer.Printwhat printWhat;
			internal bool branchesToo;
			internal int maxDistance;
			internal int count;
			internal int threads;
			
			internal Values(byte b)
			{
				server = null;
				srcPath = null;
				srcVer = null;
				target = null;
				targetVer = null;
				fromVer = null;
				toVer = null;
				vcs = null;
				printWhat = HistoryViewer.Printwhat.None;
				branchesToo = false;
				maxDistance = -1;
				count = -1;
				threads = -1;
			}
		}
		
		static int Main(string[] args)
		{
			Values values =  new Values(0x4);
			ArgParser argParser = new ArgParser();
			
			argParser.add(new Arg('s', "server", "server name", "the tfs server to connect to", null));
			argParser.add(new tfsinterface.PathVersionArg("src", "path[,version]", "the source of the changesets"));
			argParser.add(new tfsinterface.VersionArg("from","version[,version]","the changeset range to look in."));
			argParser.add(new FlagArg("name-only", "add the path of the files to the changeset info", false));
			argParser.add(new FlagArg("name-status",
																"print the path and the change type in the changeset info", false));
			argParser.add(new ArgInt('d', "distance", 
				new string[] {
					"distance or number of merge queries to run.",
					"default=1",
					"e.g. distance=2",
					" source path=$/main/ version=12",
					" query #1 = $/main/ => decompose 12.",
					"  12 => $/development/group1/ version 10",
					" query #2 = $/development/group1/ => decompose 10.",
					"  10 => $/code_review/group1/ version 8",
					" process stops."
				}, 1));
			argParser.add(new ArgInt('j', "threads", 
			  new string[] {
			    "number of threads to run merge queries in",
			    " defaults to 8.",
			    " if set to 1, queries are done in order.",
			    " e.g. ",
			    "query changeset 12, query parents of changeset 12, starting at the first parent.",
			    " query the first parent, <insert recursion>"
			   }, 8));
			argParser.add(new ArgInt('c', "count",
															 new string[] {
																 "max number history items to query",
																 " defaults to 10.",
																 " if a range isn't specified, ",
																 "this will control how many history items that are decomposed.",
															 }, 10));
			
			List<int> unknownArgs;
			bool argError = false == argParser.parse_args(args, out unknownArgs);
			
			/* if we don't have any args, the user needs to fix that too. */
			if (! argError && (unknownArgs.Count < 1)) { argError = true; }
			
			if (argError)
				{
					/* get the version of the megahistory library. */
					Version version = null;
					Assembly asm = Assembly.GetAssembly(typeof(megahistorylib.MegaHistory));
					version = asm.GetName().Version;
					/* ******************** */
					string libVersion = null;
					
					object[] objs = asm.GetCustomAttributes(typeof(System.Reflection.AssemblyInformationalVersionAttribute),true);
					if (objs != null)
						{
							System.Reflection.AssemblyInformationalVersionAttribute attr = objs[0] as System.Reflection.AssemblyInformationalVersionAttribute;
							
							if (attr != null)
								{
									libVersion = string.Format("lib version git: {0}", attr.InformationalVersion);
								}
						}
					
					if (libVersion == null)
						{
							libVersion = 
							string.Format("lib version {0}.{1}.{2}.{3} (sp{4}/{5})", 
														version.Major, version.Minor, version.Build, version.Revision,
														version.MajorRevision, version.MinorRevision);
						}
					
					argParser.print_help("<target>[,<version>]",
															 new string[] 
							{
								libVersion,
								"target - the required path we're looking at",
								"version - an optional version of the target path",
								string.Empty,
								"queries tfs for the list of changesets which make up a merge",
								string.Empty,
								"eg: megahistory -s foo --src $/foo,45 --from 10,45 $/bar,43"
							}
															 );
					
					return 1;
				}
			
			if (unknownArgs.Count > 0)
				{
					/* anything not caught by the above items is considered a target path and changeset. */
					tfsinterface.PathVersionArg.GetParts(args[unknownArgs[0]], out values.target, out values.targetVer);
				}
			
			values.server = (string)argParser.get_arg<Arg>('s');
			
			{
				tfsinterface.PathVersionArg arg = argParser.get_arg<tfsinterface.PathVersionArg>("src");
				if (arg.Data != null) { arg.get_parts(out values.srcPath, out values.srcVer); }
			}
			{
				tfsinterface.VersionArg arg = argParser.get_arg<tfsinterface.VersionArg>("from");
				if (arg.Data != null) { arg.get_parts(out values.fromVer, out values.toVer); }
			}
			
			if ((bool)argParser.get_arg<FlagArg>("name-only"))
				{ values.printWhat = HistoryViewer.Printwhat.NameOnly; }
			
			else if ((bool)argParser.get_arg<FlagArg>("name-status")) 
				{ values.printWhat = HistoryViewer.Printwhat.NameStatus; }
			
			{
				saastdlib.ArgInt arg = argParser.get_arg<saastdlib.ArgInt>('d');
				values.maxDistance = arg;
			}
			
			values.threads = argParser.get_arg<saastdlib.ArgInt>('j');
			values.count = argParser.get_arg<saastdlib.ArgInt>('c');
			
			megahistorylib.MegaHistory megahistory = 
				new megahistorylib.MegaHistory(values.server, values.maxDistance);
			
			megahistorylib.MegaHistory.THREAD_COUNT = values.threads;
			
			/* this won't do a single changeset decomposition...? */
			megahistory.query(values.target, values.targetVer, values.count, values.fromVer, values.toVer, null);

			HistoryViewer visitor = new HistoryViewer(values.printWhat, megahistory.Results);
			visitor.print(Console.Out);
			
			return 0;
		}	
	}
}
