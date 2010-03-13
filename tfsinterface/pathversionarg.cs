
using Microsoft.TeamFoundation.VersionControl.Client;

namespace tfsinterface
{
	using Arg = saastdlib.Arg;
	
	/// <summary>
	/// this parses an arg of the following format:
	/// $/foo/bar,123
	/// e.g.
	/// --tfspath $/foo/bar,123
	/// -p foo/bar,123
	/// </summary>
	/// <remarks>
	/// this is used to take in a qualified tfs path.
	/// </remarks>
	public class PathVersionArg : Arg
	{
		private string _path;
		private VersionSpec _ver;
		
		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="n_opt">single-char option</param>
		/// <param name="n_name">argument name</param>
		/// <param name="n_help">single-line of help text.</param>
		/// <param name="def">default value (not used?)</param>
		public PathVersionArg(char n_opt, string n_name, string n_help, string def)
			: base(n_opt, null, n_name, n_help, def)
		{}
		
		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="n_longOpt">long option.</param>
		/// <param name="n_name">argument name</param>
		/// <param name="n_help">single-line of help text</param>
		public PathVersionArg(string n_longOpt, string n_name, string n_help)
			: base(n_longOpt, n_name, new string[] { n_help })
		{ }

		/// <summary>
		/// find the option, then set it.
		/// </summary>
		/// <param name="argc"></param>
		/// <param name="args"></param>
		/// <returns>true if the option was found.</returns>
		public override bool set_opt(ref int argc, string[] args)
		{
			bool result = base.set_opt(ref argc, args);
			
			if (result) { GetParts(this.Data, out _path, out _ver); }
			
			return result;
		}
		
		/// <summary>
		/// get the individual parts of the option.
		/// </summary>
		/// <param name="path">the path part.</param>
		/// <param name="ver">the version part</param>
		public void get_parts(out string path, out VersionSpec ver)
		{ 
			path = _path;
			ver = _ver;
		}
		
		/// <summary>
		/// parse a string of the format [path],[version]
		/// e.g.
		/// foo/var,1234
		/// </summary>
		/// <remarks>
		/// currently [version] can only be an integer, translating to a ChangesetVersionSpec.
		/// </remarks>
		/// <param name="data"></param>
		/// <param name="path"></param>
		/// <param name="ver"></param>
		public static void GetParts(string data, out string path, out VersionSpec ver)
		{
			string[] globs = data.Split(',');
			
			path = null;
			ver = null;
			
			if (globs.Length == 2) { ver = new ChangesetVersionSpec(globs[1]); }
			path = globs[0];
		}
		
		/// <summary>
		/// convert the given PathVersion arg into a pair: path,version
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		public static implicit operator saastdlib.pair<string,VersionSpec>(PathVersionArg arg)
		{ return new saastdlib.pair<string,VersionSpec>(arg._path, arg._ver); }
	}
}
