
using Microsoft.TeamFoundation.VersionControl.Client;

namespace tfsinterface
{
	/// <summary>
	/// an argument which can take a version pair
	/// 
	/// e.g.
	/// --from 12349,12352
	/// </summary>
	public class VersionArg : saastdlib.Arg
	{
		private VersionSpec _from;
		private VersionSpec _to;
		
		/// <summary>
		/// constructor.
		/// </summary>
		/// <param name="n_longOpt">long option value (--foo)</param>
		/// <param name="n_name">name of the argument to the option.</param>
		/// <param name="n_help">help text (single line)</param>
		public VersionArg(string n_longOpt, string n_name, string n_help)
			: base(n_longOpt, n_name, new string[] { n_help })
		{ }

		/// <summary>
		/// parse and set the option
		/// </summary>
		/// <param name="argc"></param>
		/// <param name="args"></param>
		/// <returns>true if we found the option.</returns>
		public override bool set_opt(ref int argc, string[] args)
		{
			bool result = base.set_opt(ref argc, args);
			
			if (result)
				{
					string[] globs = this.Data.Split(',');
					_to = null;

					if (globs.Length == 2) { _to = new ChangesetVersionSpec(globs[1]); }
					_from = new ChangesetVersionSpec(globs[0]);					
				}
			
			return result;
		}
		
		/// <summary>
		/// get both of the versions.
		/// </summary>
		/// <param name="fromVer"></param>
		/// <param name="toVer"></param>
		public void get_parts(out VersionSpec fromVer, out VersionSpec toVer)
		{
			fromVer = _from;
			toVer = _to;
		}
	}
}
