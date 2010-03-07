
using Microsoft.TeamFoundation.VersionControl.Client;

namespace tfsmegahistory
{
	internal class VersionArg : saastdlib.Arg
	{
		internal VersionArg(string n_longOpt, string n_name, string n_help)
			: base(n_longOpt, n_name, new string[] { n_help })
		{ }
		
		internal void get_parts(out VersionSpec fromVer, out VersionSpec toVer)
		{
			string[] globs = this.Data.Split(',');
			toVer = null;
			
			if (globs.Length == 2) { toVer = new ChangesetVersionSpec(globs[1]); }
			fromVer = new ChangesetVersionSpec(globs[0]);
		}
	}
}
