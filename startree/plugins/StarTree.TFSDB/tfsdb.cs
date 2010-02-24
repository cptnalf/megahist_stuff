
using System.Collections.Generic;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace StarTree.Plugin.TFSDB
{
	using Revision = StarTree.Plugin.Database.Revision;
	using Snapshot = StarTree.Plugin.Database.Snapshot;
		
	/// <summary>
	/// 
	/// </summary>
	internal partial class TFSDB
	{
		internal static int THREAD_COUNT = 8;
		internal static log4net.ILog logger = log4net.LogManager.GetLogger("tfsdb_logger");
		
		internal static string MakeID(int id) { return string.Format("{0:d6}", id); }
		
		/* 1 = just populate the requested branch's revisions
		 * 2 = populate the requested branch + the parent's of the requested branch.
		 */
		private static readonly int RECURSIVE_QUERY_COUNT = 2;
		
		internal void LoadLogger()
		{
			if (! logger.Logger.Repository.Configured)
				{
					System.Reflection.Assembly asm = 
						System.Reflection.Assembly.GetExecutingAssembly();
					System.IO.FileStream fs;
					
					try{
						fs = new System.IO.FileStream(asm.Location+".config", 
																					System.IO.FileMode.Open, 
																					System.IO.FileAccess.Read, 
																					System.IO.FileShare.ReadWrite);
						log4net.Config.XmlConfigurator.Configure(fs);
					}
#if DEBUG
					catch(System.Exception e)
						{
							int q = 1;
							q -= 12;
							if (e.HelpLink == null ) { q +=12; }
						}
#else
					catch(System.Exception) { }
#endif
				}
		}
		
		internal static string[] DefaultBranches()
		{
			/* replace this with a dynamic tfs query.
			 * maybe cache that too.
			 */
			string[] staticBranches = {
				"$/IGT_0803/development/dev_adv/EGS/",
				"$/IGT_0803/development/dev_adv_cr/EGS/",
				"$/IGT_0803/development/dev_build/EGS/",
				"$/IGT_0803/development/dev_ABS/EGS/",
				"$/IGT_0803/development/dev_sb/EGS/",
				"$/IGT_0803/main/EGS/",
				"$/IGT_0803/release/EGS8.2/dev_sp/EGS/",
				};
			
			return staticBranches;
		}
	}		
}

