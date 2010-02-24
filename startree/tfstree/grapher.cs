
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Glee.Drawing;
using Microsoft.Glee.GraphViewerGdi;

namespace TFSTree
{
	using Revision = StarTree.Host.Database.Revision;
	using Snapshot = StarTree.Host.Database.Snapshot;
	using RevisionSorterDesc = StarTree.Host.Database.RevisionSorterDesc;
	using DrwColor = System.Drawing.Color;
	using BranchContainer = treelib.AVLTree<string, treelib.StringSorterInsensitive>;
	using RevisionCont = treelib.AVLTree<string>;
	using RevisionIdx = treelib.AVLDict<string, StarTree.Host.Database.Revision>;
	
	internal struct RevisionColors
	{
		internal Microsoft.Glee.Drawing.Color fontColor;
		internal Microsoft.Glee.Drawing.Color fillColor;
	}
	
	internal class Grapher
	{
		private string _name;
		/// <summary>Container to assign the same colors to the same authors.</summary>
		private Dictionary<string, DrwColor> _colors = new Dictionary<string, DrwColor>();
		
		internal event System.EventHandler OnProgress;
		internal string Name { get { return _name; } set { _name = value; } }
		internal Dictionary<string, DrwColor> Colors
		{ get { return _colors; } set { _colors = value; } }
		
		internal Microsoft.Glee.Drawing.Color EdgeColor { get; set; }
		internal RevisionColors ForeignBranch { get; set; }
		
		internal Grapher()
		{
			EdgeColor = Microsoft.Glee.Drawing.Color.Black;
			
			RevisionColors fb;
			fb.fontColor = Microsoft.Glee.Drawing.Color.Black;
			fb.fillColor = Microsoft.Glee.Drawing.Color.LightGray;
			
			ForeignBranch = fb;
			
		}
		
		private delegate Revision GetRevisionFx_T(string parent);
		
		/// <summary>Creates the graph for the current branch.</summary>
		/// <param name="revisions">Revisions to create graph for.</param>
		/// <param name="database">database the revisions came from (to look up parents)</param>
		/// <returns>Graph.</returns>		
		internal Graph Create(Snapshot snapshot, string branch)
		{
			Graph graph = _buildGraph(_name);
			_colors.Clear();
			
			RevisionIdx.iterator it = snapshot.find(branch);
						
			for(; it != RevisionIdx.End(); ++it)
				{
					if (it.value().Parents.Count < 4)
						{
							_printParentsFull(graph, it.value(), 
																(parent) => { return snapshot.rev(parent); } );
						}
					else
						{
							_printParentsPartial(graph, it.value(), 
																	 (parent) => { return snapshot.rev(parent); } );
							/* so, let's collapse some of the history. */
						}
										
					if (OnProgress != null) { OnProgress(this, new System.EventArgs()); }
				}
			
			return graph;
		}
		
		private void _drawRev(Graph graph, Revision from, Revision to)
		{
			Edge edge = (Edge)graph.AddEdge(from.ID, to.ID);
			edge.Attr.Color = EdgeColor;
			
			Node node = (Node)graph.FindNode(from.ID);
			node.UserData = from;
			FormatNode(node, from);
			
			node = (Node)graph.FindNode(to.ID);
			node.UserData = to;
			FormatNode(node, to);
		}

		private void _printParentsFull(Graph graph, Revision rev, 
																	 GetRevisionFx_T getRevisions)
		{
			/* draw each revision in the target branch. */
			foreach (string parentID in rev.Parents)
				{
					Revision parRev = getRevisions(parentID);
					
					if (parRev != null)
						{
							if (parRev.Branch == rev.Branch) { _drawRev(graph, parRev, rev); }
							else
								{
									Edge edge = graph.AddEdge(parentID, rev.ID);
									edge.Attr.Color = EdgeColor;
									edge.Attr.AddStyle(Style.Dashed);

									Node node = graph.FindNode(parentID);
									node.UserData = parRev;
									if (node.UserData != null)
										{ FormatNodeFromDifferentBranch(node, parRev); }

									node = graph.FindNode(rev.ID);
									node.UserData = rev;
									FormatNode(node, rev);
								}
						}
				}
		}

		private void _printParentsPartial(Graph graph, Revision rev, 
																			GetRevisionFx_T getRevisions)
		{
			/* so, let's collapse some of the history. */
			List<Revision> r = new List<Revision>();

			foreach (string parent in rev.Parents)
				{
					Revision parRev = getRevisions(parent);

					if (parRev != null)
						{
							if (parRev.Branch == rev.Branch) { _drawRev(graph, parRev, rev); }
							else { r.Add(parRev); }
						}
				}

			if (r.Count > 0)
				{
					string nodeID = string.Format("others-{0}", rev.ID);
					Edge edge = (Edge)graph.AddEdge(nodeID, rev.ID);

					edge.Attr.Color = EdgeColor;
					edge.Attr.AddStyle(Style.Dashed);

					Node node = (Node)graph.FindNode(nodeID);
					node.UserData = r;

					node.Attr.Fontcolor = Microsoft.Glee.Drawing.Color.Black;
					node.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.LightGray;
					node.Attr.LabelMargin = 5;

					System.Text.StringBuilder bldr = new System.Text.StringBuilder();
					
					foreach (Revision rli in r) { bldr.AppendLine(rli.ID); }
					
					node.Attr.Label = bldr.ToString();
					node.Attr.Shape = Shape.Box;

					node = (Node)graph.FindNode(rev.ID);
					node.UserData = rev;
					FormatNode(node, rev);
				}
		}

		private Graph _buildGraph(string name)
		{
			Graph graph = new Graph(name);
			
			graph.GraphAttr.NodeAttr.LineWidth = 2;
			graph.GraphAttr.NodeAttr.FontName = "Verdana, Arial, Helvetica, sans-serif";
			graph.GraphAttr.NodeAttr.Fontsize = 8;
			graph.GraphAttr.NodeAttr.XRad = 8;
			graph.GraphAttr.NodeAttr.YRad = 8;
			graph.GraphAttr.EdgeAttr.LineWidth = 2;
			
			return graph;
		}
		
		/// <summary>Gets the color for the specified author.</summary>
		/// <param name="author">Author to get the color for.</param>
		/// <returns>Color.</returns>
		private Microsoft.Glee.Drawing.Color GetColor(string author)
		{
			DrwColor color;
			
			if (!_colors.TryGetValue(author, out color))
				{
					StringCollection bgColors = 
						(StringCollection)Properties.Settings.Default["RevisionBgColors"];
					
					if (_colors.Count < bgColors.Count)
						{
							color = DrwColor.FromArgb(System.Int32.Parse(bgColors[_colors.Count], 
																													 System.Globalization.NumberStyles.HexNumber));
							_colors.Add(author, color);
						}
					else
						{ color = DrwColor.Gold; }
				}
			return new Microsoft.Glee.Drawing.Color(color.R, color.G, color.B);
		}
		
		/// <summary>Formats a node.</summary>
		/// <param name="node">Node to format.</param>
		/// <param name="rev">Revision which is represented by the node.</param>
		private void FormatNode(Node node, Revision rev)
		{
			node.Attr.Fontcolor = Microsoft.Glee.Drawing.Color.White;
			node.Attr.Fillcolor = GetColor(rev.Author);
			
			if (rev.Parents.Count > 1)
				{
					node.Attr.LabelMargin = 10;
					node.Attr.Label = rev.ID.Substring(0, System.Math.Min(20, rev.ID.Length));
					node.Attr.Shape = Shape.Parallelogram;
				}
			else
				{
					node.Attr.LabelMargin = 5;
					node.Attr.Label = string.Format("{0}\n{1}\n", rev.ID, rev.Author);
					/*.Substring(0, Math.Min(20, rev.ID.Length))*/
					
					if ((bool)Properties.Settings.Default["ToLocalTime"])
						{ node.Attr.Label += rev.Date.ToLocalTime().ToString(); }
					else
						{ node.Attr.Label += rev.Date.ToString(); }
					
					node.Attr.Shape = Shape.Box;
				}
		}
		
		/// <summary>Formats a node from a different branch.</summary>
		/// <param name="node">Node to format.</param>
		/// <param name="rev">Revision which is represented by the node.</param>
		private void FormatNodeFromDifferentBranch(Node node, Revision rev)
		{
			node.Attr.Fontcolor = ForeignBranch.fontColor;
			//Microsoft.Glee.Drawing.Color.White;
			
			node.Attr.Fillcolor = ForeignBranch.fillColor;
			node.Attr.LabelMargin = 5;
			
			node.Attr.Label = string.Format("{0}\n{1}\n{2}\n", rev.Branch, rev.ID, rev.Author);
			/*.Substring(0, Math.Min(20, rev.Branch.Length))*/
			
			if ((bool)Properties.Settings.Default["ToLocalTime"])
				{ node.Attr.Label += rev.Date.ToLocalTime().ToString(); }
			else
				{ node.Attr.Label += rev.Date.ToString(); }
			
			node.Attr.Shape = Shape.Box;
		}
		
	}
}
