
namespace megahistorylib
{
	internal static class Logger
	{
		internal static log4net.ILog logger = log4net.LogManager.GetLogger("megahistorylib.logger");
		
		internal static void LoadLogger()
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
				catch(Exception) { }
#endif
				
				logger.Debug("init'ed logger!");
			}
		}
	}
}
