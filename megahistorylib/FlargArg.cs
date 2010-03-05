
namespace megahistorylib
{
	/** a flag argument. */
	public class FlagArg : BaseArg
	{
		private bool _on;
		
		/// <summary>
		/// 
		/// </summary>
		public bool On { get { return _on; } }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="n_opt"></param>
		public FlagArg(char n_opt) : this(n_opt, null, false) { }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="n_opt"></param>
		/// <param name="n_help"></param>
		/// <param name="def_state"></param>
		public FlagArg(char n_opt, string[] n_help, bool def_state)
			: base(n_opt, null, n_help) { _on = def_state; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="opt_long"></param>
		/// <param name="n_help"></param>
		/// <param name="def_state"></param>
		public FlagArg(string opt_long, string n_help, bool def_state)
			: base(opt_long, null, new string[] { n_help }) { _on = def_state; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="opt_long"></param>
		/// <param name="n_help"></param>
		/// <param name="def_state"></param>
		public FlagArg(string opt_long, string[] n_help, bool def_state)
			: base(opt_long, null, n_help)
		{ _on = def_state; }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="foo"></param>
		/// <returns></returns>
		public static implicit operator bool(FlagArg foo) { return foo.On; }
	}
}