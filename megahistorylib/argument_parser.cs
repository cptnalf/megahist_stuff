/** file: argument_parser.cs
 *  chiefengineer@voyager
 *  (c) 2009 saa
 */

using System;
using System.Collections.Generic;
using System.Reflection;

/**
 *  this supports parsing standard posix-like short options.
 */
namespace megahistorylib
{
	/// <summary>
	/// 
	/// </summary>
	public class ArgParser
	{
		private Dictionary<string,BaseArg> _str2arg = new Dictionary<string,BaseArg>();
		private Dictionary<char, BaseArg> _c2arg = new Dictionary<char,BaseArg>();
		private List<BaseArg> _args = new List<BaseArg>();
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="idx"></param>
		/// <returns></returns>
		public BaseArg this[int idx] { get { return _args[idx]; } }
		
		/// <summary>
		/// 
		/// </summary>
		public ArgParser() { }
		
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="idx"></param>
		/// <returns></returns>
		public T get_arg<T>(int idx) where T : BaseArg { return (_args[idx]) as T; }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		public void add(BaseArg[] args)
		{
			foreach(BaseArg arg in args) { this.add(arg); }
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="arg"></param>
		public void add(BaseArg arg)
		{
			_args.Add(arg);
			if (arg.optLong != null) { _str2arg.Add(arg.optLong, arg); }
			if (arg.opt != '\0') { _c2arg.Add(arg.opt, arg); }
		}

		/** parse the command line arguments.
		 *  @return true = no problems
		 *         false = errors or help requested (errors have already been reported)
		 */
		public bool parse_args(string[] argv, out List<int> unknownArgs)
		{
			bool result = true;
			unknownArgs = new List<int>();
			
			for(int i=0; i < argv.Length; ++i)
				{
					if (argv[i][0] == '-' && argv[i].Length > 1)
						{
							BaseArg arg = null;
							
							if (argv[i][1] == 'h') { result = false; break; }
							else
								{
									string key;
									
									if (argv[i][1] == '-')
										{
											key = argv[i].Substring(2);
											
											if (key == "help") { result = false; break; }
											else
												{
													_str2arg.TryGetValue(key, out arg);
												}
										}
									else
										{
											char c = argv[i][1];
											_c2arg.TryGetValue(c, out arg);
										}

									if (arg != null)
										{
											result = arg.set_opt(ref i, argv);
											if (!result) break;
										}
									else
										{
											Console.WriteLine("unknown option {0}.", argv[i]);
											result = false;
											break;
										}
								}
						}
					else { unknownArgs.Add(i); }
				}
			
			return result;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="opts"></param>
		/// <param name="prog_text"></param>
		public void print_help(string opts, string[] prog_text)
		{
			/* Lookup the assembly info attributes. 
			 * assembly version
			 * description
			 * copyright
			 * location
			 * name
			 */
			Assembly assembly = Assembly.GetCallingAssembly();
			AssemblyName name = assembly.GetName();
			object[] attrs = assembly.GetCustomAttributes(true);
			AssemblyDescriptionAttribute desc = null;
			AssemblyCopyrightAttribute copyright = null;
			
			attrs = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), true);
			if (attrs.Length >= 1) { desc = (AssemblyDescriptionAttribute)attrs[0]; }
			
			attrs = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), true);
			if (attrs.Length >= 1) { copyright = (AssemblyCopyrightAttribute)attrs[0]; }
			
			/* now print out our collected information. */
			Console.WriteLine("{0} {1}", name.Name, copyright.Copyright);
			Console.WriteLine("version {0}", name.Version);
			Console.WriteLine(desc.Description);
			Console.WriteLine("{0} {1} {2}", name.Name, "[options]", opts);
			
			/* anything extra that they want to pass along */
			for(int i=0; i < prog_text.Length; ++i) { Console.WriteLine(prog_text[i]); }
			
			Console.WriteLine();
			Console.WriteLine("Options:");
			
			for(int i=0; i <_args.Count; ++i)
				{
					if (_args[i].opt != '\0') 
						{ 
							if (string.IsNullOrEmpty(_args[i].name))
								{ Console.WriteLine("  -{0}", _args[i].opt); }
							else 
								{ Console.WriteLine("  -{0} <{1}>", _args[i].opt, _args[i].name); }
						}
					if (! string.IsNullOrEmpty(_args[i].optLong))
						{
							if (string.IsNullOrEmpty(_args[i].name))
								{ Console.WriteLine("  --{0}", _args[i].optLong); }
							else
								{ Console.WriteLine("  --{0} <{1}>", _args[i].optLong, _args[i].name); }
						}
					
					for(int j=0; j < _args[i].help.Length; ++j)
						{ Console.WriteLine("\t{0}", _args[i].help[j]); }
				}
		}
	}
}
