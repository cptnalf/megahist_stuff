using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TFSTree
{
    /// <summary>Etched line.</summary>
    public partial class EtchedLine : UserControl
    {
        /// <summary>Etched line's light color.</summary>
        Color lightColor = SystemColors.ControlLight;

        /// <summary>Etched line's dark color.</summary>
        Color darkColor = SystemColors.ControlDark;

        /// <summary>Creates an etched line.</summary>
        public EtchedLine()
        {
            InitializeComponent();
        }

        /// <summary>Event handler to draw etched line.</summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rec = ClientRectangle;

            Pen lightPen = new Pen(lightColor, 1f);
            Pen darkPen = new Pen(darkColor, 1f);

            if (Dock == DockStyle.Top)
            {
                int y0 = rec.Top;
                int y1 = rec.Top + 1;

                g.DrawLine(darkPen, rec.Left, y0, rec.Right, y0);
                g.DrawLine(lightPen, rec.Left, y1, rec.Right, y1);
            }
            else if (Dock == DockStyle.Bottom)
            {
                int y0 = rec.Bottom - 2;
                int y1 = rec.Bottom - 1;

                g.DrawLine(darkPen, rec.Left, y0, rec.Right, y0);
                g.DrawLine(lightPen, rec.Left, y1, rec.Right, y1);
            }

            base.OnPaint(e);
        }

        /// <summary>Sets or gets light color.</summary>
        [Category("Appearance")]
        public Color LightColor
        {
            set { lightColor = value; }
            get { return lightColor; }
        }

        /// <summary>Sets or gets dark color.</summary>
        [Category("Appearance")]
        public Color DarkColor
        {
            set { darkColor = value; }
            get { return darkColor; }
        }
    }
}
