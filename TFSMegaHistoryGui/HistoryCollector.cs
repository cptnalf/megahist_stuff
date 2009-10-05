using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tfs_fullhistory
{
	class HistoryCollector : Visitor
	{
		public TreeNode Root = null;
		private Stack<TreeNode> _nodes = new Stack<TreeNode>();
		
		public HistoryCollector() { }
		
		protected override void _seen(int parentID, Visitor.PatchInfo p)
		{ }

		protected override void _visit(Visitor.PatchInfo p)
		{
			
			if (p.parent == 0 && Root == null) 
				{
					Root = new TreeNode(p.cs.ChangesetId.ToString());
					Root.ToolTipText = string.Format("{0} {1} {2}", p.cs.ChangesetId, p.cs.CreationDate, p.cs.Owner);
					Root.Tag = p;
					
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
					TreeNode node;
					
					node = new TreeNode(p.cs.ChangesetId.ToString());
					node.ToolTipText = string.Format("{0} {1} {2}", p.cs.ChangesetId, p.cs.CreationDate, p.cs.Owner);
					node.Tag = p;
					
					while(!done)
						{
							if (((Visitor.PatchInfo)_nodes.Peek().Tag).cs.ChangesetId == p.parent)
								{
									_nodes.Peek().Nodes.Add(node);
									_nodes.Push(node);
									done = true;
								}
							else
								{
									_nodes.Pop();
								}
						}
				}
		}
		
		public override void visit(int parentID, int changesetID, Exception e)
		{
			/* maybe log or tell the user something bad happened? */
		}
	}
}
