
using System;

namespace megahistorylib
{
	/** a 'normal' argument
	 *  it looks for the option value, and retrieves the string.
	 */
	public class Arg : BaseArg
	{
		private string _data;
		/**< the option value. */
		public string Data { get { return _data; } }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="n_opt"></param>
		/// <param name="n_optLong"></param>
		/// <param name="n_name"></param>
		/// <param name="n_help"></param>
		/// <param name="def"></param>
		public Arg(char n_opt, string n_optLong, string n_name, string n_help, string def)
			: this(n_opt, n_optLong, n_name, new string[] { n_help }, def) { }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="n_opt"></param>
		/// <param name="n_optLong"></param>
		/// <param name="n_name"></param>
		/// <param name="n_help"></param>
		/// <param name="def"></param>
		public Arg(char n_opt, string n_optLong, string n_name, string[] n_help, string def)
			: base(n_opt, n_optLong, n_name, n_help) { _data = def; }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="n_longOpt"></param>
		/// <param name="n_name"></param>
		/// <param name="n_help"></param>
		public Arg(string n_longOpt, string n_name, string[] n_help)
			: base(n_longOpt, n_name, n_help)
			{ _data = null; }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="argc"></param>
		/// <param name="args"></param>
		/// <returns></returns>
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
}