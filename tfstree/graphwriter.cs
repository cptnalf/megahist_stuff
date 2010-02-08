using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using Microsoft.Glee.Drawing;
using Microsoft.Glee.GraphViewerGdi;
using System.Linq;

namespace TFSTree
{
	using Graph = Microsoft.Glee.Drawing.Graph;
	
	class IDComparer : IComparer<Revision>
	{
		public int Compare(Revision one, Revision two) { return one.ID.CompareTo(two.ID); }
	}
	
	class GraphWriter
	{
		/// <summary>Container to assign the same colors to the same authors.</summary>
		Dictionary<string, System.Drawing.Color> colors = new Dictionary<string, System.Drawing.Color>();
		
		/*
		 * merge changesets must point to the changes which make them up
		 * so:
		 *
		 * A     B    C
		 * 10 <- 6 <- 2
		 *  |    \ \  \
		 *  \ <- 5  <-1
		 *
		 * CS  Parent
		 * 10, 6
		 * 10, 5
		 *  6, 5
		 *  6, 2
		 *  6, 1
		 *  2, 1
		 */
		internal Graph createGraph(List<Revision> revisions)
		{
			Graph graph = new Graph("foo");
			graph.GraphAttr.NodeAttr.LineWidth = 2;
			graph.GraphAttr.NodeAttr.FontName = "Verdana, Arial, Helvetica, sans-serif";
			graph.GraphAttr.NodeAttr.Fontsize = 8;
			graph.GraphAttr.NodeAttr.XRad = 8;
			graph.GraphAttr.NodeAttr.YRad = 8;
			graph.GraphAttr.EdgeAttr.LineWidth = 2;
			
			IDComparer idcmp = new IDComparer();
			revisions.Sort(idcmp);
			
			foreach(Revision rev in revisions)
				{
					foreach(string parent in rev.Parents)
						{
							Revision fuck = new Revision(parent, null);
							Revision parRev;
							int idx = revisions.BinarySearch(fuck, idcmp);
							if (idx >= 0 && idx < revisions.Count)
								{
									parRev = revisions[idx];
									Edge edge = (Edge) graph.AddEdge(parRev.ID, rev.ID);
									edge.Attr.Color = Microsoft.Glee.Drawing.Color.Black;
									Node node = (Node)graph.FindNode(parRev.ID);
									node.UserData = parRev;
									FormatNode(node, parRev);
									node = (Node)graph.FindNode(rev.ID);
									node.UserData = rev;
									FormatNode(node, rev);
								}
						}
				}
			
			return graph;
		}
		
		/// <summary>Creates the graph for the current branch.</summary>
		/// <param name="revs">Revisions to create graph for.</param>
		/// <returns>Graph.</returns>
		private Microsoft.Glee.Drawing.Graph CreateGraph(Dictionary<string, Revision> revs)
		{
			/*
			toolStripProgressBar.Visible = true;
			toolStripProgressBar.Value = 0;
			toolStripProgressBar.Maximum = revs.Count;
			*/			
			
			Graph graph = new Graph("foo");
			graph.GraphAttr.NodeAttr.LineWidth = 2;
			graph.GraphAttr.NodeAttr.FontName = "Verdana, Arial, Helvetica, sans-serif";
			graph.GraphAttr.NodeAttr.Fontsize = 8;
			graph.GraphAttr.NodeAttr.XRad = 8;
			graph.GraphAttr.NodeAttr.YRad = 8;
			graph.GraphAttr.EdgeAttr.LineWidth = 2;
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
									Revision parentRev = null; ///database.GetRevision(parent);
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
											node.UserData = null; ///database.GetRevision(parent);
											FormatNodeFromDifferentBranch(node, (Revision)node.UserData);
											node = (Node)graph.FindNode(rev.Key);
											node.UserData = rev.Value;
											FormatNode(node, rev.Value);
										}
								}
						}
					//toolStripProgressBar.PerformStep();
				}
			
			//toolStripProgressBar.Value = toolStripProgressBar.Maximum;
			return graph;
		}
		
		/// <summary>Gets the color for the specified author.</summary>
		/// <param name="author">Author to get the color for.</param>
		/// <returns>Color.</returns>
		private Microsoft.Glee.Drawing.Color GetColor(string author)
		{
			System.Drawing.Color color;
			if (!colors.TryGetValue(author, out color))
				{
					StringCollection bgColors = (StringCollection)Properties.Settings.Default["RevisionBgColors"];
					if (colors.Count < bgColors.Count)
						{
							color = System.Drawing.Color.FromArgb(Int32.Parse(bgColors[colors.Count], System.Globalization.NumberStyles.HexNumber));
							colors.Add(author, color);
						}
					else
						{
							color = System.Drawing.Color.Gold;
						}
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
					node.Attr.Label = rev.ID.Substring(0, Math.Min(20, rev.ID.Length));
					node.Attr.Shape = Shape.Parallelogram;
				}
			else
				{
					node.Attr.LabelMargin = 5;
					node.Attr.Label = string.Format("{0}\n{1}\n", 
																					rev.ID /*.Substring(0, Math.Min(20, rev.ID.Length))*/,
																					rev.Author);
					if ((bool)Properties.Settings.Default["ToLocalTime"])
						node.Attr.Label += rev.Date.ToLocalTime().ToString();
					else
						node.Attr.Label += rev.Date.ToString();
					node.Attr.Shape = Shape.Box;
				}
		}
		
		/// <summary>Formats a node from a different branch.</summary>
		/// <param name="node">Node to format.</param>
		/// <param name="rev">Revision which is represented by the node.</param>
		private void FormatNodeFromDifferentBranch(Node node, Revision rev)
		{
			node.Attr.Fontcolor = Microsoft.Glee.Drawing.Color.Black;//Microsoft.Glee.Drawing.Color.White;
			node.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.LightGray;
			node.Attr.LabelMargin = 5;
			node.Attr.Label = rev.Branch + "\n" +rev.ID /*.Substring(0, Math.Min(20, rev.Branch.Length))*/ + "\n" + rev.Author + "\n";
			
			if ((bool)Properties.Settings.Default["ToLocalTime"])
				node.Attr.Label += rev.Date.ToLocalTime().ToString();
			else
				node.Attr.Label += rev.Date.ToString();
			node.Attr.Shape = Shape.Box;
		}
	}
}
