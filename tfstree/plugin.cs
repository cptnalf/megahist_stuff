
namespace TFSTree
{
	internal class DBPlugin
	{
		internal Assembly Assembly { get; set; }
		internal Type DatabaseType { get; set; }
		
	}

	internal class Plugins
	{
		load_plugins(string directory)
		{
			string[] files = Directory.GetFiles(directory, "*.dll");
			
			foreach(string file in files)
				{
					try
						{
							DBPlugin plugin;
							Assembly assy = Assembly.LoadFile(file);
							Type dbType = null;
					
							Type[] types = assy.GetExportedTypes();
					
							foreach(Type t in types)
								{
									if (t.IsClass)
										{
											foreach(Type it in t.GetInterfaces())
												{
													if (it == typeof(Databases.IDBPlugin))
														{
															dbType = t;
														}
												}
										}
								}
					
							if (dbType != null)
								{
									plugin = new DBPlugin
										{
											Assembly = assy,
												DatabaseType = dbType,
												};
							
									plugins.Add(plugin);
								}
						}
					catch(Exception e)
						{
							/* ?? */
						}
				}
		}
	}
}

