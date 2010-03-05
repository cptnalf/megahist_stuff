
namespace megahistorylib
{
	/// <summary>
	/// 
	/// </summary>
	public class ArgUShort : Arg
	{
		private ushort _ushort;
		
		/// <summary>
		/// 
		/// </summary>
		public ushort UShort { get { return _ushort; } }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="n_opt"></param>
		/// <param name="n_name"></param>
		/// <param name="n_help"></param>
		/// <param name="def"></param>
		public ArgUShort(char n_opt, string n_name, string n_help, ushort def)
			: this(n_opt, n_name, new string[] { n_help }, def) { }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="n_opt"></param>
		/// <param name="n_name"></param>
		/// <param name="n_help"></param>
		/// <param name="def"></param>
		public ArgUShort(char n_opt, string n_name, string[] n_help, ushort def)
			: base(n_opt, null, n_name, n_help, null)
		{ _ushort = def; }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="argc"></param>
		/// <param name="args"></param>
		/// <returns></returns>
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
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="foo"></param>
		/// <returns></returns>
		public static implicit operator ushort(ArgUShort foo) { return foo.UShort; }
	}
}
