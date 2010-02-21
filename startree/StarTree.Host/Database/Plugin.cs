using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarTree.Host.Database
{
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
