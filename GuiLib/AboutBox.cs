using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Library;

namespace Library.Gui
{
    public partial class AboutBox : Form
    {
        private const string COPYRIGHT = "";
        private const string TEXT_VERSION = "Version";
        private const string TEXT_BUILT = "Erstellt";

        public string InfoApplication { get; set; }
        public string InfoVersion { get; set; }
        public string InfoBuilt { get; set; }
        public string InfoLicense { get; set; }

        public List<InfoItem> InfoItems;

        public AboutBox()
        {
            InitializeComponent();
            InfoItems = new List<InfoItem>();
        }

        private void AboutBox_Load(object sender, EventArgs e)
        {
            Render();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AboutBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void Render()
        {
            // Application
            if (InfoApplication!= null && InfoApplication.Length > 0)
            {
                labelApplication.Text = InfoApplication;
            }
            else
            {
                labelApplication.Text = AssemblyScanner.GetCustomAssemblyAttributeValue(Assembly.GetEntryAssembly(), typeof(AssemblyProductAttribute));
            }

            // Copyright
            labelCopyright.Text = string.Format(COPYRIGHT, DateTime.Now.Year.ToString());

            // Version
            string version = TEXT_VERSION + ' ';

            if (InfoVersion != null && InfoVersion.Length > 0)
            {
                version += InfoVersion;
            }
            else
            {
                version += Assembly.GetEntryAssembly().GetName().Version.ToString();
            }
            
            labelVersion.Text = version;

            // Built
            string infoBuiltText = string.Empty;

            if (string.IsNullOrEmpty(InfoBuilt))
            {
                infoBuiltText = AssemblyScanner.GetCustomAssemblyAttributeValue(Assembly.GetEntryAssembly(), typeof(AssemblyDescriptionAttribute));
            }
            else
            {
                infoBuiltText = InfoBuilt;
            }

            labelBuilt.Text = TEXT_BUILT + ' ' + infoBuiltText;
            labelBuilt.Visible = !string.IsNullOrEmpty(infoBuiltText);

            // Info - ListView
            PrepareListView();
            RenderListView();

            // LicenseInfo - RichTextBox
            RenderRichTextBox();

            if (string.IsNullOrEmpty(InfoLicense))
            {
                radioButtonViewInfo.Visible = false;
                radioButtonViewLicenseInfo.Visible = false;
                richTextBoxLicenceInfo.Visible = false;
                listViewInfo.Location = radioButtonViewInfo.Location;
                listViewInfo.Size = new Size(listViewInfo.Width, listViewInfo.Height + richTextBoxLicenceInfo.Location.Y - radioButtonViewInfo.Location.Y);
            }

            radioButtonViewInfo.Checked = true;
        }

        private void PrepareListView()
        {
            listViewInfo.Items.Clear();
            listViewInfo.FullRowSelect = true;
            listViewInfo.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            listViewInfo.View = System.Windows.Forms.View.Details;

            ColumnHeader columnHeader;

            columnHeader = new ColumnHeader();
            columnHeader.Text = "Was";
            listViewInfo.Columns.Add(columnHeader);

            columnHeader = new ColumnHeader();
            columnHeader.Text = "Wert";
            listViewInfo.Columns.Add(columnHeader);
        }

        private void RenderListView()
        {
            if (InfoItems.Count == 0)
            {
                InfoItems.Add(new AboutBox.InfoItem(".NET CLR", Environment.Version.ToString()));
                InfoItems.Add(new AboutBox.InfoItem("Base directory", AppDomain.CurrentDomain.BaseDirectory));
            }

            foreach (InfoItem item in InfoItems)
            {
                listViewInfo.Items.Add(new ListViewItem(new string[] { item.Name, item.Value }));
            }

            listViewInfo.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void RenderRichTextBox()
        {
            richTextBoxLicenceInfo.Text = InfoLicense;
        }

        public class InfoItem
        {
            public string Name { get; set; }
            public string Value { get; set; }

            public InfoItem(string name, string value)
            {
                Name = name;
                Value = value;
            }
        }

        private void radioButtonViewInfo_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonViewInfo.Checked)
            {
                listViewInfo.BringToFront();
            }
        }

        private void radioButtonViewLicenseInfo_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonViewLicenseInfo.Checked)
            {
                richTextBoxLicenceInfo.BringToFront();
            }
        }

        private void richTextBoxLicenceInfo_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Shell.StartDocument(e.LinkText);
        }
    }
}
