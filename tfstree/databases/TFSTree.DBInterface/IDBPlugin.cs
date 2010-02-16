
namespace TFSTree.Databases
{
	public interface IDBPlugin
	{
		string internalName { get; }
		string Name { get; }
		
		string IDName { get; }
		string ParentName { get; }
		string AuthorName { get; }
		string LogName    { get; }
		
		IRevisionRepo open();
		
		void init();
		void close();
	}
}
