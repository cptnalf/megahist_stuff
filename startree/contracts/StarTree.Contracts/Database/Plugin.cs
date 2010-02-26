using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarTree.Contracts.Database
{
	[System.AddIn.Pipeline.AddInContract]
	public interface IPlugin : System.AddIn.Contract.IContract
	{
		DisplayNames names { get; }
		string currentName { get; }
		void open();
		void close();
		string[] branches();
		byte[] getBranch(string branch, long limit);
		Revision getRevision(string id);
		byte[] queryMerges(Revision rev);
	}
}
