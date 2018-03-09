using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Library.Gui;

namespace Library.Gui.ExternalTools
{
    public partial class ExternalToolsDialog : Form
    {
        private const string NEW = "[New Tool]";
        private const string SEPARATOR = "-";

        private ExternalToolManager _manager;

        public ExternalToolsDialog(ExternalToolManager manager)
        {
            InitializeComponent();
            _manager = manager;
            TextBoxHelper.SetSelectAllOnGotFocus(this);
            Init();
        }

        private void Init()
        {
            SetListBoxDataSource();

            if (_manager.ExternalTools.Count == 0)
            {
                AddNewEntry();
            }

            ShowEntry();
        }

        private void ShowEntry()
        {
            if (CurrentEntry == null)
            {
                textBoxTitle.Text = string.Empty;
                textBoxCommand.Text = string.Empty;
                textBoxArguments.Text = string.Empty;
                textBoxInitialDirectory.Text = string.Empty;

                LockEntryGuiElements(true);
                return;
            }

            LockEntryGuiElements(false);

            textBoxTitle.Text = CurrentEntry.Title;
            textBoxCommand.Text = CurrentEntry.Command;
            textBoxArguments.Text = CurrentEntry.Arguments;
            textBoxInitialDirectory.Text = CurrentEntry.InitialDirectory;

            SetStatusMoveUpDown();
        }

        private void LockEntryGuiElements(bool lockIt)
        {
            buttonDelete.Enabled = !lockIt;
            buttonMoveUp.Enabled = !lockIt;
            buttonMoveDown.Enabled = !lockIt;

            textBoxTitle.Enabled = !lockIt;
            textBoxCommand.Enabled = !lockIt;
            textBoxArguments.Enabled = !lockIt;
            textBoxInitialDirectory.Enabled = !lockIt;
            textBoxPreview.Enabled = !lockIt;
            buttonBrowseCommand.Enabled = !lockIt;
            buttonSelectionArguments.Enabled = !lockIt;
            buttonSelectionInitDirectory.Enabled = !lockIt;

            if (lockIt)
            {
                buttonAdd.Focus();
            }
        }

        public ExternalToolEntry CurrentEntry
        {
            get
            {
                return (ExternalToolEntry)listBoxTool.SelectedItem;
            }
        }

        private void ExternalToolsDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                CloseDialog();
            }
        }

        private void AddNewEntry()
        {
            _manager.ExternalTools.Add(new ExternalToolEntry() { Title = NEW });

            if (listBoxTool.SelectedIndex != listBoxTool.Items.Count - 1)
            {
                listBoxTool.SelectedIndex = listBoxTool.Items.Count - 1;
            }
            else
            {
                ShowEntry();
            }
            
            textBoxCommand.Focus();
        }

        private void DeleteEntry()
        {
            int selIndex = listBoxTool.SelectedIndex;
            _manager.ExternalTools.Remove((ExternalToolEntry)listBoxTool.SelectedItem);

            if (selIndex < listBoxTool.Items.Count)
            {
                listBoxTool.SelectedIndex = selIndex;
            }
        }

        private void MoveEntryUp()
        {
            if (CurrentEntry == null)
            {
                return;
            }

            MoveEntry(-1);
        }

        private void MoveEntryDown()
        {
            if (CurrentEntry == null)
            {
                return;
            }
            
            MoveEntry(1);
        }

        private void MoveEntry(int offset)
        {
            ExternalToolEntry entry = CurrentEntry;
            int index = listBoxTool.SelectedIndex;
            _manager.ExternalTools.RemoveAt(index);
            int newIndex = index + offset;
            _manager.ExternalTools.Insert(newIndex, entry);
            listBoxTool.SelectedIndex = newIndex;
            listBoxTool.Focus();
        }

        private void CloseDialog()
        {
            Close();
        }

        private void SetCommandText(string value)
        {
            if (textBoxTitle.Text == NEW || textBoxTitle.Text.Length == 0)
            {
                string newTitle = Path.GetFileNameWithoutExtension(value);
                if (!string.IsNullOrEmpty(newTitle))
	            {
                    newTitle = newTitle.Substring(0, 1).ToUpper() + newTitle.Substring(1);
                    textBoxTitle.Text = newTitle;
	            }
            }

            textBoxCommand.Text = value;
        }

        private void ComposePreview()
        {
            textBoxPreview.Text = _manager.ComposeCommandToExecute(CurrentEntry);
        }

        private void AddTextToTextBox(string value, TextBox textBox)
        {
            if (textBox.Name == textBoxInitialDirectory.Name)
            {
                textBox.Text = value;
            }
            else
            {
                textBox.SelectedText = value;
            }
            
            ComposePreview();
        }

        #region Context menu item event handlers
        void argumentMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                AddTextToTextBox(((ToolStripMenuItem)sender).Tag.ToString(), textBoxArguments);
            }
        }

        void initDirMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                AddTextToTextBox(((ToolStripMenuItem)sender).Tag.ToString(), textBoxInitialDirectory);
            }
        }
        #endregion

        #region Button event handlers
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddNewEntry();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DeleteEntry();
        }

        private void buttonMoveUp_Click(object sender, EventArgs e)
        {
            MoveEntryUp();
        }

        private void buttonMoveDown_Click(object sender, EventArgs e)
        {
            MoveEntryDown();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            CloseDialog();
        }

        private void buttonBrowseCommand_Click(object sender, EventArgs e)
        {
            BrowseForCommand();
        }

        private void buttonSelectionArguments_Click(object sender, EventArgs e)
        {
            BrowseForArguments();
        }

        private void buttonSelectionInitDirectory_Click(object sender, EventArgs e)
        {
            BrowseForInitDirectory();
        }
        #endregion

        #region Text box event handlers
        private void textBoxCommand_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
            {
                BrowseForCommand();
            }
        }

        private void textBoxArguments_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
            {
                BrowseForArguments();
            }
        }

        private void textBoxInitialDirectory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
            {
                BrowseForInitDirectory();
            }
        }

        private void SetListBoxDataSource()
        {
            listBoxTool.DisplayMember = "Title";
            listBoxTool.DataSource = _manager.ExternalTools;
        }

        private void textBoxTitle_TextChanged(object sender, EventArgs e)
        {
            if (CurrentEntry != null)
            {
                CurrentEntry.Title = ((TextBox)sender).Text;
                ((CurrencyManager)listBoxTool.BindingContext[listBoxTool.DataSource]).Refresh();
            }
        }

        private void textBoxCommand_TextChanged(object sender, EventArgs e)
        {
            if (CurrentEntry != null)
            {
                CurrentEntry.Command = ((TextBox)sender).Text;
            }

            ComposePreview();
        }

        private void textBoxArguments_TextChanged(object sender, EventArgs e)
        {
            if (CurrentEntry != null)
            {
                CurrentEntry.Arguments = ((TextBox)sender).Text;
            }

            ComposePreview();
        }

        private void textBoxInitialDirectory_TextChanged(object sender, EventArgs e)
        {
            if (CurrentEntry != null)
            {
                CurrentEntry.InitialDirectory = ((TextBox)sender).Text;
            }
        }
        #endregion

        #region BrowserFor methods
        private void BrowseForCommand()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "All Executables (*.exe, *.com, *.pif, *.bat, *.cmd)|*.exe;*.com;*.pif;*.bat;*.cmd";
                ofd.FilterIndex = 1;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    SetCommandText(ofd.FileName);
                }
            }
        }

        private void BrowseForArguments()
        {
            ContextMenuStrip cms = new ContextMenuStrip();

            foreach (ArgumentEntry argument in _manager.Arguments)
            {
                if (argument.Title == SEPARATOR)
                {
                    cms.Items.Add(new ToolStripSeparator());
                }
                else
                {
                    ToolStripMenuItem argumentMenuItem = new ToolStripMenuItem(argument.Title);
                    argumentMenuItem.Tag = argument.VariableName;
                    argumentMenuItem.Click += new EventHandler(argumentMenuItem_Click);
                    cms.Items.Add(argumentMenuItem);
                }
            }

            Point ptAbsolute = PointToScreen(buttonSelectionArguments.Location);
            Point pt = new Point(ptAbsolute.X + buttonSelectionArguments.Width, ptAbsolute.Y);
            cms.Show(pt);
        }

        private void BrowseForInitDirectory()
        {
            ContextMenuStrip cms = new ContextMenuStrip();

            foreach (InitDirEntry initDir in _manager.InitialDirectories)
            {
                if (initDir.Title == SEPARATOR)
                {
                    cms.Items.Add(new ToolStripSeparator());
                }
                else
                {
                    ToolStripMenuItem initDirMenuItem = new ToolStripMenuItem(initDir.Title);
                    initDirMenuItem.Tag = initDir.VariableName;
                    initDirMenuItem.Click += new EventHandler(initDirMenuItem_Click);
                    cms.Items.Add(initDirMenuItem);
                }
            }

            Point ptAbsolute = PointToScreen(buttonSelectionInitDirectory.Location);
            Point pt = new Point(ptAbsolute.X + buttonSelectionInitDirectory.Width, ptAbsolute.Y);
            cms.Show(pt);
        }
        #endregion

        private void buttonExecute_Click(object sender, EventArgs e)
        {
            _manager.ExecuteCommand(CurrentEntry);
        }

        private void listBoxTool_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowEntry();
        }

        private void SetStatusMoveUpDown()
        {
            buttonMoveUp.Enabled = false;
            buttonMoveDown.Enabled = false;

            if (listBoxTool.Items.Count > 1)
            {
                buttonMoveUp.Enabled = true;
                buttonMoveDown.Enabled = true;

                if (listBoxTool.SelectedIndex == 0)
                {
                    buttonMoveUp.Enabled = false;
                }

                if (listBoxTool.SelectedIndex == listBoxTool.Items.Count - 1)
                {
                    buttonMoveDown.Enabled = false;
                }
            }
        }

        private void textBoxPreview_TextChanged(object sender, EventArgs e)
        {
            buttonExecute.Enabled = textBoxPreview.Text.Length > 0;
        }
    }
}
