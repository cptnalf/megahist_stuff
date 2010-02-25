
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
		public abstract Revision getRevision(string id);
		
		/// <summary>
		/// this has to do one of two things:
		/// 1) return ALL of the changesets to a depth of 2 (the branch + 1 more level)
		/// 2) allow for callbacks for 1 revision
		/// 
		/// (perhaps the snapshot caches data?)
		/// </summary>
		/// <param name="branch"></param>
		/// <param name="limit"></param>
		/// <returns></returns>
		public abstract Snapshot getBranch(string branch, long limit);
		
		/// <summary>
		/// this will decompose the specified revision into possible list of changesets.
		/// </summary>
		public abstract Snapshot queryMerges(Revision rev);
	}
}
