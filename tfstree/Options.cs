using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TFSTree
{
    /// <summary>Options dialog window.</summary>
    public partial class Options : Form
    {
        /// <summary>Property pages.</summary>
        List<PropertyPage> pages = new List<PropertyPage>();

        /// <summary>Currently active property page.</summary>
        PropertyPage activePage = null;

        /// <summary>Instantiates the options dialog window.</summary>
        public Options()
        {
            InitializeComponent();
        }

        /// <summary>Gets the property pages.</summary>
        public IList<PropertyPage> Pages
        {
            get
            {
                return pages;
            }
        }

        /// <summary>Event handler for OK button.</summary>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            foreach (PropertyPage page in pages)
                page.Apply();
            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>Event handler for Cancel button.</summary>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>Event handler for Load event.</summary>
        private void Options_Load(object sender, EventArgs e)
        {
            Bitmap defaultBitmap = new Bitmap(GetType(), "Bitmaps.NullOptions.bmp");
            imageListGroups.Images.Add(defaultBitmap);

            Size maxSize = panelPages.Size;

            foreach (PropertyPage page in pages)
            {
                panelPages.Controls.Add(page);
                AddListItemForPage(page);

                if (page.Width > maxSize.Width)
                    maxSize.Width = page.Width;
                if (page.Height > maxSize.Height)
                    maxSize.Height = page.Height;

                page.Dock = DockStyle.Fill;
                page.Visible = false;
            }

            Size newSize = new Size();
            newSize.Width = maxSize.Width + (Width - panelPages.Width) + panelPages.Padding.Horizontal;
            newSize.Height = maxSize.Height + (Height - panelPages.Height) + panelPages.Padding.Vertical;
            Size = newSize;
            CenterToParent();

            if (listViewGroups.Items.Count != 0)
                listViewGroups.Items[0].Selected = true;
        }

        /// <summary>Adds items to the overview of property pages for every new property page.</summary>
        /// <param name="page">New property page to be added to the options dialog window.</param>
        private void AddListItemForPage(PropertyPage page)
        {
            int imageIndex = 0;

            Image image = page.Image;
            if (image != null)
            {
                imageListGroups.Images.Add(image);
                imageIndex = imageListGroups.Images.Count - 1;
            }

            ListViewItem item = new ListViewItem(page.Text, imageIndex);
            item.Tag = page;
            listViewGroups.Items.Add(item);
        }

        /// <summary>Event handler to display a new property page.</summary>
        private void listViewGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (activePage != null)
                activePage.Visible = false;

            if (listViewGroups.SelectedItems.Count != 0)
            {
                ListViewItem item = listViewGroups.SelectedItems[0];
                PropertyPage page = (PropertyPage)item.Tag;
                activePage = page;
            }

            if (activePage != null)
            {
                activePage.Visible = true;
                activePage.SetActive();
            }
        }
    }
}
