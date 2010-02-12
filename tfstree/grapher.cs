
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Glee.Drawing;
using Microsoft.Glee.GraphViewerGdi;

namespace TFSTree
{
	using DrwColor = System.Drawing.Color;
	using BranchContainer = treelib.AVLTree<string, treelib.StringSorterInsensitive>;
	using RevisionIdx = treelib.AVLDict<int, Revision, treelib.IntSorterDesc>;
	using BranchChangesets =
		treelib.AVLDict<string, treelib.AVLDict<int, Revision, treelib.IntSorterDesc>, treelib.StringSorterInsensitive>;

	internal class Grapher
	{
		internal delegate Revision GetRevision_T(string revID);
		
		private string _name;
		/// <summary>Container to assign the same colors to the same authors.</summary>
		private Dictionary<string, DrwColor> _colors = new Dictionary<string, DrwColor>();
		
		internal event System.EventHandler OnProgress;
		internal GetRevision_T GetRevisionFx { get; set; }
		internal string Name { get { return _name; } set { _name = value; } }
		internal Dictionary<string, DrwColor> Colors
		{ get { return _colors; } set { _colors = value; } }
		
		internal Grapher() { }
		
		internal Graph Create(string masterBranch, BranchChangesets branchChangesets, RevisionIdx revisions)
		{
			Graph graph = _buildGraph(masterBranch);
						
// 			for(BranchChangesets.iterator it = branchChangesets.begin();
// 			    it != branchChangesets.end();
// 					++it)
// 				{
// 					if (it.item() != masterBranch)
// 						{
// 							for(RevisionIdx.iterator rit = it.value().begin();
// 							    rit != it.value().end();
// 							    ++rit)
// 								{
// 									foreach(string parent in rit.value().Parents)
// 										{
// 											int revID = int.Parse(parent);
// 											RevisionIdx.iterator parRevIt = revisions.find(revID);
											
// 											if (parRevIt != revisions.end())
// 												{
// 													if (parRevIt.value().Branch == rit.value().Branch)
// 														{
// 															/* only draw parents which are in this branch. */
// 															_drawRev(graph, parRevIt.value(), rit.value());
// 														}
// 												}
// 										}
// 								}
// 						}
// 				}
			
			{
				BranchChangesets.iterator targetIt = branchChangesets.find(masterBranch);
				
				if (targetIt != branchChangesets.end())
					{
						for(RevisionIdx.iterator rit = targetIt.value().begin();
						    rit != targetIt.value().end();
						    ++rit)
							{
								GetRevisionFx_T getrevision = 
								  (parentID) => { 
									       int parID = int.Parse(parentID);
									       RevisionIdx.iterator parRevIt = revisions.find(parID);
									       return (parRevIt != revisions.end() ? parRevIt.value() : null);
									      };
								
								if (rit.value().Parents.Count > 4)
									{
										_printParentsPartial(graph, rit.value(), getrevision);
									}
								else 
									{ _printParentsFull(graph, rit.value(), getrevision); }
							}
					}
			}
			
			return graph;
		}
		
		delegate Revision GetRevisionFx_T(string parent);
		
		private void _printParentsFull(Graph graph, Revision rev, GetRevisionFx_T getRevisions)
		{
			/* draw each revision in the target branch. */
			foreach(string parentID in rev.Parents)
				{
					Revision parRev = getRevisions(parentID);
					
					if (parRev != null)
						{
							if (parRev.Branch == rev.Branch)
								{
									_drawRev(graph, rev, parRev);
								}
							else
								{
									Edge edge = graph.AddEdge(parentID, rev.ID);
									edge.Attr.Color = Microsoft.Glee.Drawing.Color.Black;
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
		
		private void _printParentsPartial(Graph graph, Revision rev, GetRevisionFx_T getRevisions)
		{
			/* so, let's collapse some of the history. */
			List<Revision> r = new List<Revision>();
			
			foreach(string parent in rev.Parents)
				{
					Revision parRev = getRevisions(parent);
					
					if (parRev != null)
						{
							if (parRev.Branch == rev.Branch)
								{ _drawRev(graph, rev, parRev); }
							else
								{ r.Add(parRev); }
						}
				}
			
			if (r.Count > 0)
				{
					string nodeID = string.Format("others-{0}", rev.ID);
					Edge edge = (Edge)graph.AddEdge(nodeID, rev.ID);
					
					edge.Attr.Color = Microsoft.Glee.Drawing.Color.Black;
					edge.Attr.AddStyle(Style.Dashed);
					
					Node node = (Node)graph.FindNode(nodeID);
					node.UserData = r;
					
					node.Attr.Fontcolor = Microsoft.Glee.Drawing.Color.Black;
					node.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.LightGray;
					node.Attr.LabelMargin = 5;
					
					System.Text.StringBuilder bldr = new System.Text.StringBuilder();
					foreach(Revision rli in r)
						{
							bldr.AppendLine(rli.ID);
						}
					node.Attr.Label = bldr.ToString();
					node.Attr.Shape = Shape.Box;
					
					node = (Node)graph.FindNode(rev.ID);
					node.UserData = rev;
					FormatNode(node, rev);
				}
		}
		
		/// <summary>Creates the graph for the current branch.</summary>
		/// <param name="revs">Revisions to create graph for.</param>
		/// <returns>Graph.</returns>
		internal Graph Create(Dictionary<string, Revision> revs)
		{
			Graph graph = _buildGraph(_name);
			
			foreach (KeyValuePair<string, Revision> rev in revs)
				{
					foreach (string parent in rev.Value.Parents)
						{
							if (revs.ContainsKey(parent))
								{
									Edge edge = (Edge)graph.AddEdge(parent, rev.Key);
									edge.Attr.Color = Microsoft.Glee.Drawing.Color.Black;
									
									Node node = (Node)graph.FindNode(parent);
									node.UserData = revs[parent];
									FormatNode(node, revs[parent]);
									
									node = (Node)graph.FindNode(rev.Key);
									node.UserData = rev.Value;
									FormatNode(node, rev.Value);
								}
							else
								{
									Revision parentRev = this.GetRevisionFx(parent);
									
									if (parentRev != null && parentRev.Branch == rev.Value.Branch)
										{
											Edge edge = (Edge)graph.AddEdge(parent, rev.Key);
											edge.Attr.Color = Microsoft.Glee.Drawing.Color.Black;
											
											Node node = (Node)graph.FindNode(parent);
											node.UserData = parentRev;
											FormatNode(node, parentRev);
											
											node = (Node)graph.FindNode(rev.Key);
											node.UserData = rev.Value;
											FormatNode(node, rev.Value);
										}
									else
										{
											Edge edge = (Edge)graph.AddEdge(parent, rev.Key);
											edge.Attr.Color = Microsoft.Glee.Drawing.Color.Black;
											edge.Attr.AddStyle(Style.Dashed);
											
											Node node = (Node)graph.FindNode(parent);
											node.UserData = this.GetRevisionFx(parent);
											if (node.UserData != null)
												{
													FormatNodeFromDifferentBranch(node, (Revision)node.UserData);
												}
											
											node = (Node)graph.FindNode(rev.Key);
											node.UserData = rev.Value;
											FormatNode(node, rev.Value);
										}
								}
						}
					
					if (OnProgress != null) { OnProgress(this, new System.EventArgs()); }
				}
			
			return graph;
		}
		
		internal Graph CreateWSG(Dictionary<string,Revision> revs)
		{
			Graph graph = _buildGraph(_name);
			
			foreach(KeyValuePair<string, Revision> rev in revs)
				{
					if (rev.Value.Parents.Count < 4)
						{
							_printParentsFull(graph, rev.Value, (parent) => { return this.GetRevisionFx(parent); } );
						}
					else
						{
							_printParentsPartial(graph, rev.Value, (parent) => { return this.GetRevisionFx(parent); } );
							/* so, let's collapse some of the history. */
						}
										
					if (OnProgress != null) { OnProgress(this, new System.EventArgs()); }
				}
			
			return graph;
		}
		
		private void _drawRev(Graph graph, Revision from, Revision to)
		{
			Edge edge = (Edge)graph.AddEdge(from.ID, to.ID);
			edge.Attr.Color = Microsoft.Glee.Drawing.Color.Black;
			
			Node node = (Node)graph.FindNode(from.ID);
			node.UserData = from;
			FormatNode(node, from);
			
			node = (Node)graph.FindNode(to.ID);
			node.UserData = to;
			FormatNode(node, to);
		}
		
		/*
			draw a revision
		 */
		private void _drawRev(Dictionary<string,Graph> graphs, string revID)
		{
			Revision rev = this.GetRevisionFx(revID);
			
			if (rev != null)
				{
					Graph g = null;
					if (graphs.ContainsKey(rev.Branch))
						{ g = graphs[rev.Branch]; }
					else 
						{
							g = _buildGraph(rev.Branch); 
							graphs.Add(rev.Branch, g);
						}
					
					foreach(string parent in rev.Parents)
						{
							Node node = g.FindNode(parent);
							
							if (node != null)
								{
									g.AddEdge(rev.ID, parent);
								}
						}
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
			node.Attr.Fontcolor = Microsoft.Glee.Drawing.Color.Black;
			//Microsoft.Glee.Drawing.Color.White;
			
			node.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.LightGray;
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
