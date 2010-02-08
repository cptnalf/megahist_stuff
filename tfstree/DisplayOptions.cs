using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Glee.Drawing;

namespace TFSTree
{
    /// <summary>Property page for display options.</summary>
    public partial class DisplayOptions : PropertyPage
    {
        /// <summary>Instantiates the property page for display options.</summary>
        public DisplayOptions()
        {
            InitializeComponent();

            viewer.RemoveToolbar();
            viewer.OutsideAreaBrush = System.Drawing.Brushes.White;

            Graph graph = new Graph("Options");
            graph.GraphAttr.NodeAttr.LineWidth = 2;
            graph.GraphAttr.NodeAttr.FontName = "Verdana, Arial, Helvetica, sans-serif";
            graph.GraphAttr.NodeAttr.Fontsize = 8;
            graph.GraphAttr.NodeAttr.XRad = 8;
            graph.GraphAttr.NodeAttr.YRad = 8;
            graph.GraphAttr.NodeAttr.Shape = Shape.Box;
            graph.GraphAttr.EdgeAttr.LineWidth = 2;

            for (int i = 1; i < 10; ++i)
                graph.AddEdge("Developer " + i, "Developer " + (i + 1));

            StringCollection bgColors = (StringCollection)Properties.Settings.Default["RevisionBgColors"];
            for (int i = 0; i < bgColors.Count && i < 10; ++i)
                FormatNode((Node)graph.FindNode("Developer " + (i + 1)), System.Drawing.Color.FromArgb(Int32.Parse(bgColors[i], System.Globalization.NumberStyles.HexNumber)));

            viewer.Graph = graph;
            viewer.ZoomF = 4;
            viewer.Focus();
            viewer.CenterToPoint(0, 0);
        }

        public override string Text
        {
            get
            {
                return "Appearance";
            }
        }

        public override Image Image
        {
            get
            {
                return new Bitmap(GetType(), "Bitmaps.DisplayOptions.bmp");
            }
        }

        public override void SetActive()
        {
        }

        public override void Apply()
        {
            StringCollection bgColors = new StringCollection();
            bgColors.Add(((Node)viewer.Graph.FindNode("Developer 1")).Attr.Fillcolor.ToString().Trim('"', '#'));
            bgColors.Add(((Node)viewer.Graph.FindNode("Developer 2")).Attr.Fillcolor.ToString().Trim('"', '#'));
            bgColors.Add(((Node)viewer.Graph.FindNode("Developer 3")).Attr.Fillcolor.ToString().Trim('"', '#'));
            bgColors.Add(((Node)viewer.Graph.FindNode("Developer 4")).Attr.Fillcolor.ToString().Trim('"', '#'));
            bgColors.Add(((Node)viewer.Graph.FindNode("Developer 5")).Attr.Fillcolor.ToString().Trim('"', '#'));
            bgColors.Add(((Node)viewer.Graph.FindNode("Developer 6")).Attr.Fillcolor.ToString().Trim('"', '#'));
            bgColors.Add(((Node)viewer.Graph.FindNode("Developer 7")).Attr.Fillcolor.ToString().Trim('"', '#'));
            bgColors.Add(((Node)viewer.Graph.FindNode("Developer 8")).Attr.Fillcolor.ToString().Trim('"', '#'));
            bgColors.Add(((Node)viewer.Graph.FindNode("Developer 9")).Attr.Fillcolor.ToString().Trim('"', '#'));
            bgColors.Add(((Node)viewer.Graph.FindNode("Developer 10")).Attr.Fillcolor.ToString().Trim('"', '#'));
            Properties.Settings.Default["RevisionBgColors"] = bgColors;
        }

        /// <summary>Event handler for double clicks in viewer.</summary>
        private void viewer_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            object clicked = viewer.GetObjectAt(e.X, e.Y);
            if (clicked is Node)
            {
                if (colorDialogAdd.ShowDialog() == DialogResult.OK)
                {
                    System.Drawing.Color color = colorDialogAdd.Color;
                    ((Node)clicked).Attr.Fillcolor = new Microsoft.Glee.Drawing.Color(color.R, color.G, color.B);
                    viewer.Refresh();
                }
            }
        }

        /// <summary>Event handler for Reset button.</summary>
        private void buttonReset_Click(object sender, EventArgs e)
        {
            FormatNode((Node)viewer.Graph.FindNode("Developer 1"), System.Drawing.Color.SteelBlue);
            FormatNode((Node)viewer.Graph.FindNode("Developer 2"), System.Drawing.Color.DarkSeaGreen);
            FormatNode((Node)viewer.Graph.FindNode("Developer 3"), System.Drawing.Color.Crimson);
            FormatNode((Node)viewer.Graph.FindNode("Developer 4"), System.Drawing.Color.DarkGoldenrod);
            FormatNode((Node)viewer.Graph.FindNode("Developer 5"), System.Drawing.Color.Violet);
            FormatNode((Node)viewer.Graph.FindNode("Developer 6"), System.Drawing.Color.Orange);
            FormatNode((Node)viewer.Graph.FindNode("Developer 7"), System.Drawing.Color.DarkKhaki);
            FormatNode((Node)viewer.Graph.FindNode("Developer 8"), System.Drawing.Color.LightSkyBlue);
            FormatNode((Node)viewer.Graph.FindNode("Developer 9"), System.Drawing.Color.GreenYellow);
            FormatNode((Node)viewer.Graph.FindNode("Developer 10"), System.Drawing.Color.Peru);
            viewer.Refresh();
        }

        /// <summary>Formats a node.</summary>
        /// <param name="node">Node to format</param>
        /// <param name="color">Color to fill node with.</param>
        private void FormatNode(Node node, System.Drawing.Color color)
        {
            node.Attr.LabelMargin = 5;
            node.Attr.Fontcolor = Microsoft.Glee.Drawing.Color.White;
            if (color != null)
                node.Attr.Fillcolor = new Microsoft.Glee.Drawing.Color(color.R, color.G, color.B);
        }
    }
}
