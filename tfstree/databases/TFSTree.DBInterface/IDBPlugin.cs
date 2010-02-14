
namespace TFSTree.Databases
{
	public interface IDBPlugin
	{
		string internalName { get; }
		string Name { get; }
		IRevisionRepo open();
		
		void init();
		void close();
	}
}
