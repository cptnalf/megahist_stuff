using System;
using System.Drawing;
using System.Windows.Forms;

namespace TFSTree
{
    /// <summary>Property page.</summary>
    public partial class PropertyPage : UserControl
    {
        /// <summary>Instantiates a property page.</summary>
        public PropertyPage()
        {
            InitializeComponent();
        }

        /// <summary>Gets the description of the property page.</summary>
        public override string Text
        {
            get
            {
                return GetType().Name;
            }
        }

        /// <summary>Gets the image which is associated with the property page.</summary>
        public virtual Image Image
        {
            get
            {
                return null;
            }
        }

        /// <summary>Called when a property page becomes active.</summary>
        /// <remarks>Override this method in derived classes to update a property page when it becomes active.</remarks>
        public virtual void SetActive()
        {
        }

        /// <summary>Called when the user clicks on OK to close a dialog window and save options.</summary>
        /// <remarks>Overwrite this method in derived classes to save options of a property page.</remarks>
        public virtual void Apply()
        {
        }
    }
}
