
namespace TFSTree
{
	public interface IRevisionRepo
	{
		string FileName { get; }
		System.Collections.Generic.IEnumerable<string> BranchNames { get; }
		
		Revision rev(string id);
		System.Collections.Generic.Dictionary<string,Revision> revs(string branch, ulong limit);
		
		void load(string filename);
		void loadfolder(string filename);
		
	}
}
