using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Windows.Forms;

namespace Library.Gui.ExternalTools
{
    public class ExternalToolManager
    {
        public BindingList<ExternalToolEntry> ExternalTools { get; set; }
        public List<ArgumentEntry> Arguments { get; set; }
        public List<InitDirEntry> InitialDirectories { get; set; }
        public string SettingsFile { get; set; }

        public delegate void RequestUpToDateValuesEventHandler(object sender, RequestUpToDateValuesEventArgs args);
        public event RequestUpToDateValuesEventHandler OnRequestUpToDateValues;

        private List<ToolStripItem> _addedToolStripItems;
        private ToolStripMenuItem _externalToolsDialogMenuItem;

        public ExternalToolManager(string settingsFile, ToolStripMenuItem externalToolsDialogMenuItem)
        {
            ExternalTools = new BindingList<ExternalToolEntry>();
            SettingsFile = settingsFile;
            LoadSettings();

            Arguments = new List<ArgumentEntry>();
            InitialDirectories = new List<InitDirEntry>();

            _externalToolsDialogMenuItem = externalToolsDialogMenuItem;
            _addedToolStripItems = new List<ToolStripItem>();
            RenderMenu();
        }

        public void ShowDialog()
        {
            RaiseGetUpToDateValuesEvent();

            using (ExternalToolsDialog dlg = new ExternalToolsDialog(this))
            {
                dlg.ShowDialog();
            }

            SaveSettings();
            RenderMenu();
        }

        private string GetArgumentExpression(string arguments)
        {
            if (string.IsNullOrEmpty(arguments))
            {
                return string.Empty;
            }

            string result = arguments;

            foreach (ArgumentEntry argument in Arguments)
            {
                if (!string.IsNullOrEmpty(argument.Value))
                {
                    result = result.Replace(argument.VariableName, argument.Value);
                }
            }

            return result;
        }

        private string GetInitialDirectoryExpression(string initialDirectory)
        {
            if (string.IsNullOrEmpty(initialDirectory))
            {
                return string.Empty;
            }

            string result = initialDirectory;

            foreach (InitDirEntry initdir in InitialDirectories)
            {
                if (!string.IsNullOrEmpty(initdir.Value))
                {
                    result = result.Replace(initdir.VariableName, initdir.Value);
                }
            }

            return result;
        }

        internal string ComposeCommandToExecute(ExternalToolEntry entry)
        {
            if (entry == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(entry.Command);

            if (!string.IsNullOrEmpty(entry.Arguments))
            {
                sb.Append(" ");
                sb.Append(GetArgumentExpression(entry.Arguments));
            }

            return sb.ToString();
        }

        public void ExecuteCommand(ExternalToolEntry entry)
        {
            RaiseGetUpToDateValuesEvent();

            Process proc = new Process();
            proc.StartInfo.FileName = entry.Command;

            string args = GetArgumentExpression(entry.Arguments);

            if (!string.IsNullOrEmpty(args))
            {
                proc.StartInfo.Arguments = args;
            }

            string initDirExpr = GetInitialDirectoryExpression(entry.InitialDirectory);
            if (!string.IsNullOrEmpty(initDirExpr) && Directory.Exists(initDirExpr))
            {
                proc.StartInfo.WorkingDirectory = initDirExpr;
            }

            proc.Start();
        }

        #region Render menu
        private void RenderMenu()
        {
            if (ExternalTools == null)
            {
                return;
            }

            if (_externalToolsDialogMenuItem == null)
            {
                return;
            }

            CleanUpMenu();

            ToolStripMenuItem parentMenuItem = (ToolStripMenuItem)_externalToolsDialogMenuItem.OwnerItem;

            int index = parentMenuItem.DropDownItems.IndexOf(_externalToolsDialogMenuItem);

            for (int i = ExternalTools.Count - 1; i >= 0; i--)
            {
                ExternalToolEntry externalTool = ExternalTools[i];

                string menuText = externalTool.Title;
                if (!string.IsNullOrEmpty(menuText))
                {
                    ToolStripMenuItem externalToolMenuItem = new ToolStripMenuItem(menuText);
                    externalToolMenuItem.Tag = externalTool;
                    externalToolMenuItem.Click += new EventHandler(externalToolMenuItem_Click);
                    parentMenuItem.DropDownItems.Insert(index, externalToolMenuItem);
                    _addedToolStripItems.Add(externalToolMenuItem);
                }
            }
        }

        private void CleanUpMenu()
        {
            if (_addedToolStripItems == null || _addedToolStripItems.Count == 0)
            {
                return;
            }

            foreach (ToolStripMenuItem menuItem in _addedToolStripItems)
            {
                menuItem.Click -= externalToolMenuItem_Click;

                if (menuItem.OwnerItem != null && menuItem.OwnerItem is ToolStripMenuItem)
                {
                    ((ToolStripMenuItem)menuItem.OwnerItem).DropDownItems.Remove(menuItem);
                }
            }

            _addedToolStripItems.Clear();
        }
        #endregion

        void externalToolMenuItem_Click(object sender, EventArgs e)
        {
            RaiseGetUpToDateValuesEvent();

            object tag = ((ToolStripMenuItem)sender).Tag;

            if (tag != null && tag is ExternalToolEntry)
            {
                ExecuteCommand((ExternalToolEntry)tag);
            }
        }

        private void RaiseGetUpToDateValuesEvent()
        {
            if (OnRequestUpToDateValues != null)
            {
                RequestUpToDateValuesEventArgs args = new RequestUpToDateValuesEventArgs();

                args.UpToDateArguments = Arguments;
                args.UpToDateInitialDirectories = InitialDirectories;

                OnRequestUpToDateValues(this, args);

                Arguments = args.UpToDateArguments;
                InitialDirectories = args.UpToDateInitialDirectories;
            }
        }

        #region Load and save settings
        public void LoadSettings()
        {
            if (!File.Exists(SettingsFile))
            {
                return;
            }

            XmlSerializer ser = new XmlSerializer(typeof(BindingList<ExternalToolEntry>));

            using (FileStream fs = new FileStream(SettingsFile, FileMode.Open))
            {
                ExternalTools = (BindingList<ExternalToolEntry>)ser.Deserialize(fs);
            }
        }

        public void SaveSettings()
        {
            XmlSerializer ser = new XmlSerializer(typeof(BindingList<ExternalToolEntry>));

            using (FileStream fs = new FileStream(SettingsFile, FileMode.Create))
            {
                ser.Serialize(fs, ExternalTools);
            }
        }
        #endregion
    }

    public class RequestUpToDateValuesEventArgs : EventArgs
    {
        public List<ArgumentEntry> UpToDateArguments { get; set; }
        public List<InitDirEntry> UpToDateInitialDirectories { get; set; }
    }
}
