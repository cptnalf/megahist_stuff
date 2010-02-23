using System;
using System.Drawing;

namespace TFSTree
{
    /// <summary>Property page for general options.</summary>
    public partial class GeneralOptions : PropertyPage
    {
        /// <summary>Instantiates the property page for general options.</summary>
        public GeneralOptions()
        {
            InitializeComponent();

            checkBoxToLocalTime.Checked = (bool)Properties.Settings.Default["ToLocalTime"]; 
        }

        public override string Text
        {
            get
            {
                return "General";
            }
        }

        public override Image Image
        {
            get
            {
                return new Bitmap(GetType(), "Bitmaps.GeneralOptions.bmp");
            }
        }

        public override void SetActive()
        {
        }

        public override void Apply()
        {
            Properties.Settings.Default["ToLocalTime"] = checkBoxToLocalTime.Checked;
        }
    }
}
