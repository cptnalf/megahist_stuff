
namespace megahistorylib
{
	/** base class for an argument.
	 */
	public class BaseArg
	{
		/**< 1 char option we're looking for, eg: -f */
		private char _opt;
		private string _optLong; /**< multi-char name for this option, eg: --foo-bar */
		private string _name;   /**< a helpful name for this option. */
		private string[] _help; /**< help text for this option. */
		
		///
		public char opt { get { return _opt; } }
		/// <summary>
		/// 
		/// </summary>
		public string optLong { get { return _optLong; } }
		/// <summary>
		/// 
		/// </summary>
		public string name { get { return _name; } }
		/// <summary>
		/// 
		/// </summary>
		public string[] help { get { return _help; } }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="n_opt"></param>
		/// <param name="n_long"></param>
		/// <param name="n_name"></param>
		/// <param name="n_help"></param>
		public BaseArg(char n_opt, string n_long, string n_name, string n_help)
		{ _opt = n_opt; _optLong = n_long; _name = n_name; _help = new string[] { n_help }; }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="n_opt"></param>
		/// <param name="n_long"></param>
		/// <param name="n_name"></param>
		/// <param name="n_help"></param>
		public BaseArg(char n_opt, string n_long, string n_name, string[] n_help)
		{
			_opt = n_opt;
			_optLong = n_long;
			_name = n_name;
			_help = n_help;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="n_long"></param>
		/// <param name="n_name"></param>
		/// <param name="n_help"></param>
		public BaseArg(string n_long, string n_name, string[] n_help)
			: this('\0', n_long, n_name, n_help)
		{ }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="n_opt"></param>
		/// <param name="n_long"></param>
		/// <param name="n_help"></param>
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
}
