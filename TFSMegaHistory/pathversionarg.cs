
using Microsoft.TeamFoundation.VersionControl.Client;

namespace tfsmegahistory
{
	using Arg = saastdlib.Arg;
	
	/// <summary>
	/// this parses an arg of the following format:
	/// $/foo/bar,123
	/// </summary>
	/// <remarks>
	/// this is used to take in a qualified tfs path.
	/// </remarks>
	internal class PathVersionArg : Arg
	{
		internal PathVersionArg(char n_opt, string n_name, string n_help, string def)
			: base(n_opt, null, n_name, n_help, def)
		{}
		internal PathVersionArg(string n_longOpt, string n_name, string n_help)
			: base(n_longOpt, n_name, new string[] { n_help })
		{ }
		
		internal void get_parts(out string path, out VersionSpec ver)
		{ GetParts(this.Data, out path, out ver); }
		
		internal static void GetParts(string data, out string path, out VersionSpec ver)
		{
			string[] globs = data.Split(',');
			
			path = null;
			ver = null;
			
			if (globs.Length == 2) { ver = new ChangesetVersionSpec(globs[1]); }
			path = globs[0];
		}
	}
}
