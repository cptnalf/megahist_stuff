using System;
using System.Collections.Generic;

using megahistory;

namespace tfs_fullhistory
{
	class HistoryCollector : Visitor
	{
		public PatchInfo Root = null;
		private Stack<PatchInfo> _nodes = new Stack<PatchInfo>();
		private System.ComponentModel.BackgroundWorker _worker = null;
		
		public System.ComponentModel.BackgroundWorker Worker
		{ get { return _worker; } set { _worker = value; } }
		
		public HistoryCollector() { }
		
		public override void reset()
		{
			if (Root != null)
				{
					_worker.ReportProgress(99, Root);
				}
			
			base.reset();
			Root = null;
			_nodes.Clear();
		}
		
		protected override void _seen(int parentID, Visitor.PatchInfo p)
		{ }

		protected override void _visit(Visitor.PatchInfo p)
		{
			if (p.parent == 0 && Root == null) 
				{
					Root = p;					
					_nodes.Push(Root);
				}
			else
				{
					/*
					 * so, we look to see if our parent is the top of the stack.
					 * if so, we add ourselves, then push ourselves.
					 * if not, we need to find our parent on the stack.
					 *         so pop one off, and check again.
					 *
					 */
					
					bool done = false;
					
					while(!done)
						{
							if ((_nodes.Peek()).id == p.parent)
								{
									(_nodes.Peek()).add(p);
									_nodes.Push(p);
									done = true;
								}
							else { _nodes.Pop(); }
						}
				}
		}
		
		public override void visit(int parentID, int changesetID, Exception e)
		{
			/* maybe log or tell the user something bad happened? */
		}
	}
}
