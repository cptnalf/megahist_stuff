
namespace TFSTree
{
	public class ProgressArgs : System.EventArgs
	{
		public int Delta { get; set; }
		public object Data { get; set; }
		
		public ProgressArgs(int delta, object obj)
		{
			this.Delta = delta;
			this.Data = obj;
		}
	}
	
	public interface IRevisionRepo
	{
		string FileName { get; }
		System.Collections.Generic.IEnumerable<string> BranchNames { get; }
		
		Revision rev(string id);
		System.Collections.Generic.Dictionary<string,Revision> revs(string branch, ulong limit);
		
		void load(string filename);
		void loadfolder(string filename);
		
		event System.EventHandler<ProgressArgs> OnProgress;
	}
}
