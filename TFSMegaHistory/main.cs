/*
 * 2009-03-26 15:21:01 -0700
 * stephen.alfors@igt.com
 */

/* this generates duplicates
 * i'm not sure if its because there really are duplicates in the merge history, or if 
 * the recursive queries are picking them up.
 * also, the rather innocent query i did turned into a frigg'n diasater.
 * 40 minutes later i have 720 different changesets (again multiples exist)
 * graphing the directed changes with dot, i get a png that's 7500x14000.
 *
 * reduced the call time to 8 minutes.
 */

using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Reflection;
using mh_ns = megahistorylib;
using Arg = saastdlib.Arg;
using ArgParser = saastdlib.ArgParser;
using FlagArg = saastdlib.FlagArg;


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
			}
		}
		
		static int Main(string[] args)
		{
			Values values =  new Values(0x4);
			ArgParser argParser = new ArgParser();
			
			argParser.add(new Arg('s', "server", "server name", "the tfs server to connect to", null));
			argParser.add(new PathVersionArg("src", "path[,version]", "the source of the changesets"));
			argParser.add(new VersionArg("from","version[,version]","the changeset range to look in."));
			argParser.add(new FlagArg("name-only", "add the path of the files to the changeset info", true));
			argParser.add(new FlagArg("name-status",
																"print the path and the change type in the changeset info", false));
			argParser.add(new saastdlib.ArgInt('d', "distance", 
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
			
			List<int> unknownArgs;
			bool argError = !argParser.parse_args(args, out unknownArgs);
			
			/* if we don't have any args, the user needs to fix that too. */
			if (!argError && (unknownArgs.Count > 0)) { argError = true; }
			
			if (argError)
				{
					/* get the version of the megahistory library. */
					Version version = null;
					Assembly asm = Assembly.GetAssembly(typeof(megahistorylib.VisitorBase));
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
					PathVersionArg.GetParts(args[unknownArgs[0]], out values.target, out values.targetVer);
				}
			
			values.server = (string)argParser.get_arg<Arg>('s');
			
			{
				PathVersionArg arg = argParser.get_arg<PathVersionArg>("src");
				if (arg.Data != null) { arg.get_parts(out values.srcPath, out values.srcVer); }
			}
			{
				VersionArg arg = argParser.get_arg<VersionArg>("from");
				if (arg.Data != null) { arg.get_parts(out values.fromVer, out values.toVer); }
			}
			
			if ((bool)argParser.get_arg<FlagArg>("name-only"))
				{ values.printWhat = HistoryViewer.Printwhat.NameOnly; }
			
			else if ((bool)argParser.get_arg<FlagArg>("name-status")) 
				{ values.printWhat = HistoryViewer.Printwhat.NameStatus; }
			
			values.maxDistance = (int)argParser.get_arg<saastdlib.ArgInt>("distance");
					
			HistoryViewer visitor = new HistoryViewer(values.printWhat);
			
			bool result = false;

			megahistorylib.MegaHistory megahistory = 
				new megahistorylib.MegaHistory(values.server, visitor, values.maxDistance);
			
			megahistory.query(values.srcPath, values.srcVer, int.MaxValue, values.fromVer, values.toVer, null);
			
			visitor.print(Console.Out);
			
			return 0;
		}	
	}
}
