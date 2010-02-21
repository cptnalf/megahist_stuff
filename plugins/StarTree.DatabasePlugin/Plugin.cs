
namespace StarTree.Plugin.Database
{
	[System.AddIn.Pipeline.AddInBase]
	public abstract class Plugin
	{
		public abstract DisplayNames names { get; }
		public abstract string currentName { get; }
		public abstract void open();
		public abstract void close();
		public abstract string[] branches();
		public abstract Snapshot getBranch(string branch, long limit);
	}
}
