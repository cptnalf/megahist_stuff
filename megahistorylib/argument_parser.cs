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
	public class ArgParser
	{
		private Dictionary<string,BaseArg> _str2arg = new Dictionary<string,BaseArg>();
		private Dictionary<char, BaseArg> _c2arg = new Dictionary<char,BaseArg>();
		private List<BaseArg> _args = new List<BaseArg>();
		
		public BaseArg this[int idx] { get { return _args[idx]; } }
		
		public ArgParser() { }
		
		public T get_arg<T>(int idx) where T : BaseArg { return (_args[idx]) as T; }
		
		public void add(BaseArg[] args)
		{
			foreach(BaseArg arg in args) { this.add(arg); }
		}
		
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
	
	/** base class for an argument.
	 */
	public class BaseArg
	{
		/**< 1 char option we're looking for, eg: -f */
		private char _opt;
		private string _optLong; /**< multi-char name for this option, eg: --foo-bar */
		private string _name;   /**< a helpful name for this option. */
		private string[] _help; /**< help text for this option. */
		
		public char opt       { get { return _opt; } }
		public string optLong { get { return _optLong; } }
		public string name    { get { return _name; } }
		public string[] help  { get { return _help; } }
		
		public BaseArg(char n_opt, string n_long, string n_name, string n_help)
		{ _opt = n_opt; _optLong = n_long; _name = n_name; _help = new string[] { n_help }; }

		public BaseArg(char n_opt, string n_long, string n_name, string[] n_help)
		{ 
			_opt = n_opt;
			_optLong = n_long;
			_name = n_name;
			_help = n_help;
		}
		
		public BaseArg(string n_long, string n_name, string[] n_help)
		: this('\0', n_long, n_name, n_help)
		{ }
		
		public BaseArg(char n_opt, string n_long, string[] n_help)
		 : this(n_opt, n_long, null, n_help)
		{ }
		
		/** called when the option is found.
		 *  this can then add another option or set internal data.
		 *  
		 *  @return false stops parsing
		 *          true continues parsing.
		 */
		public virtual bool set_opt(ref int i, string[] args) { return true; }
	}
	
	/** a flag argument. */
	public class FlagArg : BaseArg
	{
		private bool _on;
		public bool On { get { return _on; } }
		
		public FlagArg(char n_opt) : this(n_opt, null, false) { }
		public FlagArg(char n_opt, string[] n_help, bool def_state) 
			: base(n_opt, null, n_help) { _on = def_state; }
		
		public FlagArg(string opt_long, string n_help, bool def_state)
			:base(opt_long, null, new string[] { n_help }) { _on = def_state; }
		public FlagArg(string opt_long, string[] n_help, bool def_state)
			: base(opt_long, null, n_help)
		{ _on = def_state; }
		
		public static implicit operator bool(FlagArg foo) { return foo.On; }
	}
	
	/** a 'normal' argument
	 *  it looks for the option value, and retrieves the string.
	 */
	public class Arg : BaseArg
	{
		private string _data;
		/**< the option value. */
		public string Data { get { return _data; } }
		
		public Arg(char n_opt, string n_optLong, string n_name, string n_help, string def)
			: this(n_opt, n_optLong, n_name, new string[] { n_help }, def) { }
		public Arg(char n_opt, string n_optLong, string n_name, string[] n_help, string def)
			: base(n_opt, n_optLong, n_name, n_help) { _data = def; }
		
		public Arg(string n_longOpt, string n_name, string[] n_help)
			: base(n_longOpt, n_name, n_help)
			{ _data = null; }
		
		public override bool set_opt(ref int argc, string[] args)
		{
			bool ok = ((argc+1) < args.Length);
			if (ok) { _data = args[++argc]; }
			return ok;
		}
		
		/** report an error parsing the option value.
		 */
		protected void err(string arg)
		{
			Console.WriteLine("error: invalid type for option {0}; got {1}",
												name, arg);
		}
		
		/** convert this argument to it's value. */
		public static implicit operator string(Arg foo) { return foo.Data; }
	}
	
	public class ArgUShort : Arg
	{
		private ushort _ushort;
		public ushort UShort { get { return _ushort; } }
		
		public ArgUShort(char n_opt, string n_name, string n_help, ushort def)
			: this(n_opt, n_name, new string[] { n_help }, def) { }
		public ArgUShort(char n_opt, string n_name, string[] n_help, ushort def)
			: base(n_opt, null, n_name, n_help, null)
		{ _ushort = def; }
		
		public override bool set_opt(ref int argc, string[] args)
		{
			bool ok = base.set_opt(ref argc, args);
			if (ok)
				{
					bool res = UInt16.TryParse(Data, out _ushort);
					if (! res) { err(Data); ok = false; }
				}
			return ok;
		}
		
		public static implicit operator ushort(ArgUShort foo) { return foo.UShort; }
	}
}
