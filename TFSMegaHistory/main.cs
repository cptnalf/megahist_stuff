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
using mh_ns = megahistory;
using Arg = megahistorylib.Arg;
using ArgParser = megahistorylib.ArgParser;
using FlagArg = megahistorylib.FlagArg;

/* for log4net. */
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

class main
{
	/* ChangesetMerge.TargetVersion - the changeset which the merge took place
	 * ChangesetMerge.SourceVersion - the changeset containing the changes which we merged.
	 */
	
	internal class PathVersionArg : Arg
	{
		internal PathVersionArg(char n_opt, string n_name, string n_help, string def)
			: base(n_opt, null, n_name, n_help, def)
		{}
		internal PathVersionArg(string n_longOpt, string n_name, string n_help)
			: base(n_longOpt, n_name, new string[] { n_help })
		{ }
		
		internal void get_parts(out string path, out VersionSpec ver)
		{ GetParts(this.Data, out path, out ver); }
		
		internal static void GetParts(string data, out string path, out VersionSpec ver)
		{
			string[] globs = data.Split(',');
			
			path = null;
			ver = null;
			
			if (globs.Length == 2) { ver = new ChangesetVersionSpec(globs[1]); }
			path = globs[0];
		}
	}
	
	internal class VersionArg : megahistorylib.Arg
	{
		internal VersionArg(string n_longOpt, string n_name, string n_help)
			: base(n_longOpt, n_name, new string[] { n_help })
		{ }
		
		internal void get_parts(out VersionSpec fromVer, out VersionSpec toVer)
		{
			string[] globs = this.Data.Split(',');
			toVer = null;
			
			if (globs.Length == 2) { toVer = new ChangesetVersionSpec(globs[1]); }
			fromVer = new ChangesetVersionSpec(globs[0]);
		}
	}
	
	internal struct Values
	{
		internal bool noRecurse;
		internal string server;
		internal string srcPath;
		internal VersionSpec srcVer;
		internal string target;
		internal VersionSpec targetVer;
		internal VersionSpec fromVer;
		internal VersionSpec toVer;
		internal VersionControlServer vcs;
		internal HistoryViewer.Printwhat printWhat;
		internal bool allowBranchRevisiting;
		internal bool forceDecomposition;
		internal bool branchesToo;
		
		internal Values(byte b)
		{
			noRecurse = false;
			server = null;
			srcPath = null;
			srcVer = null;
			target = null;
			targetVer = null;
			fromVer = null;
			toVer = null;
			vcs = null;
			printWhat = HistoryViewer.Printwhat.None;
			allowBranchRevisiting = false;
			forceDecomposition = false;
			branchesToo = false;
		}
	}
	
	static int Main(string[] args)
	{
		Values values =  new Values(0x4);
		ArgParser argParser = new ArgParser();
		
		argParser.add(new Arg('s', "server", "server name", "the tfs server to connect to", null));
		argParser.add(new PathVersionArg("src", "path[,version]", "the source of the changesets"));
		argParser.add(new VersionArg("from","version[,version]","the changeset range to look in."));
		argParser.add(new FlagArg("no-recurse", new string[]
				{
					"do not recursively query merge history.",
					"this will execute only one QueryMerges",
				},
												 false));
		argParser.add(new FlagArg("name-only", "add the path of the files to the changeset info", true));
		argParser.add(new FlagArg("name-status",
															"print the path and the change type in the changeset info", false));
		argParser.add(new FlagArg("allow-branch-revisiting", 
															new string[] 
				{
					"this option allows the query algorithm to revisit branches.",
					"only turn on this branch if really know what you are doing.",
					"Turning this option on could cause a stack overflow.",
					"(infinite loop of recusrive decomposition)",
				},
															false));
		
		argParser.add(new FlagArg("force-decomposition", new string[]
				{
					"if the changeset in the initial query just contains ",
					"ChangeType.Merge changes, this option will allow the",
					" querying to continue. ",
					"By default the algorithm will stop decomposing the changeset."
				},
															false));
		argParser.add(new FlagArg("branches-too", new string[]
				{
					"this changes the function used to determine when to decompose",
					" a changeset to include ChangeType.Branch changes as well.",
				},
															false));
		
		List<int> unknownArgs;
		bool argError = !argParser.parse_args(args, out unknownArgs);
		
		/* if we don't have any args, the user needs to fix that too. */
		if (!argError && (unknownArgs.Count > 0)) { argError = true; }
		
		if (argError)
			{
				/* get the version of the megahistory library. */
				Version version = null;
				Assembly asm = Assembly.GetAssembly(typeof(megahistory.MegaHistory));
				version = asm.GetName().Version;
				/* ******************** */
				
				string libVersion = 
					string.Format("lib version {0}.{1}.{2}.{3} (sp{4}/{5})", 
												version.Major, version.Minor, version.Build, version.Revision,
												version.MajorRevision, version.MinorRevision);
				
				argParser.print_help("<target>[,<version>]",
														 new string[] 
						{
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
		
		values.server = (string)argParser.get_arg<Arg>(0);
		
		{
			PathVersionArg arg = argParser.get_arg<PathVersionArg>(1);
			if (arg.Data != null) { arg.get_parts(out values.srcPath, out values.srcVer); }
		}
		{
			VersionArg arg = argParser.get_arg<VersionArg>(2);
			if (arg.Data != null) { arg.get_parts(out values.fromVer, out values.toVer); }
		}
		
		values.noRecurse = (bool)argParser.get_arg<FlagArg>(3);
		if ((bool)argParser.get_arg<FlagArg>(4))
			{ values.printWhat = HistoryViewer.Printwhat.NameOnly; }
		
		else if ((bool)argParser.get_arg<FlagArg>(5)) 
			{ values.printWhat = HistoryViewer.Printwhat.NameStatus; }
		
		values.allowBranchRevisiting = argParser.get_arg<FlagArg>(6);
		values.forceDecomposition = argParser.get_arg<FlagArg>(7);
		values.branchesToo = argParser.get_arg<FlagArg>(8);
		
		values.vcs = mh_ns.Utils.GetTFSServer(values.server);
		
		mh_ns.MegaHistory.Options mhopts = new mh_ns.MegaHistory.Options();
		
		mhopts.NoRecurse = values.noRecurse;
		mhopts.AllowBranchRevisiting = values.allowBranchRevisiting;
		mhopts.ForceDecomposition = values.forceDecomposition;
		
		mh_ns.Visitor visitor = new HistoryViewer(values.printWhat);
		mh_ns.MegaHistory megahistory = new mh_ns.MegaHistory(mhopts, values.vcs, visitor);
		
		if (values.branchesToo)
			{
				mh_ns.MegaHistory.IsChangeToConsider = 
					delegate(Change cng)
					{
						return (
										(cng.ChangeType != ChangeType.Merge)  /* ignore only merges. */
										&&
										(
										 /* look for branched items, or merged items */
										 ((cng.ChangeType & ChangeType.Branch) == ChangeType.Branch) ||
										 ((cng.ChangeType & ChangeType.Merge) == ChangeType.Merge)
										 )
										);
					};
			}
		
		bool result = megahistory.visit(values.srcPath, values.srcVer, 
																		values.target, values.targetVer, 
																		values.fromVer, values.toVer, RecursionType.Full);
		
		if (!result)
			{
				Console.WriteLine("no changesets found.");
			}
		
		return 0;
	}	
}
