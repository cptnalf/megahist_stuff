
using System.Reflection;
using System.Collections.Generic;
using System.IO;

namespace TFSTree
{
	using Type = System.Type;
	
	internal class Plugin
	{
		internal Assembly Assembly { get; set; }
		internal Type DatabaseType { get; set; }
		
		internal Databases.IDBPlugin dbInterface { get; private set; }
		
		internal void load()
		{
			ConstructorInfo constructor = DatabaseType.GetConstructor(Type.EmptyTypes);
			
			if (constructor != null)
				{
					dbInterface = constructor.Invoke(null) as Databases.IDBPlugin;
				}
		}
	}

	internal class Plugins
	{
		private List<Plugin> _plugins = new List<Plugin>();
		
		internal Plugins() { }
		
		internal List<Databases.IDBPlugin> getDBPlugins()
		{
			List<Databases.IDBPlugin> dbs = new List<Databases.IDBPlugin>();
			
			foreach(Plugin p in _plugins)
				{
					Databases.IDBPlugin obj = p.dbInterface;
					if (obj != null) { dbs.Add(obj); }
				}
			
			return dbs;
		}
		
		internal void load(string directory)
		{
			string fullpath = Path.GetFullPath(directory);
			string[] files = Directory.GetFiles(fullpath, "*.dll");
			
			foreach(string file in files)
				{
					try
						{
							Plugin plugin;
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
									plugin = new Plugin
										{
											Assembly = assy,
											DatabaseType = dbType,
										};
									
									plugin.load();
									
									_plugins.Add(plugin);
								}
						}
					catch(System.Exception e)
						{
							/* ?? */
						}
				}
		}
	}
}

