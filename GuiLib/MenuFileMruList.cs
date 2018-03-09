using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using Library;
using System.IO;
using System.Xml.Serialization;

namespace Library.Gui
{
    public class MenuFileMruList
    {
        public delegate void MruMenuItemClickEventHandler(string data);
        public event MruMenuItemClickEventHandler OnMruMenuItemClick;

        private ToolStripMenuItem _insertBeforeMenuItem;
        private List<string> _mruList;

        private List<ToolStripItem> _addedToolStripItems;

        private const int DEFAULT_MAX_NUMBER_OF_ENTRIES = 9;

        public string DataFile { get; set; }
        public int MaxNumberOfEntries { get; set; }

        public MenuFileMruList(ToolStripMenuItem insertBeforeMenuItem)
        {
            _insertBeforeMenuItem = insertBeforeMenuItem;
            _mruList = new List<string>();
            _addedToolStripItems = new List<ToolStripItem>();
            MaxNumberOfEntries = DEFAULT_MAX_NUMBER_OF_ENTRIES;
        }

        public void Add(string value)
        {
            RemoveCaseSensitive(value);
            //_mruList.Remove(value);
            
            _mruList.Insert(0, value);
            EnsureMaxNumberOfEntries();
            Render();
        }

        private void EnsureMaxNumberOfEntries()
        {
            if (MaxNumberOfEntries <= 0)
            {
                MaxNumberOfEntries = DEFAULT_MAX_NUMBER_OF_ENTRIES;
            }

            int diff = _mruList.Count - MaxNumberOfEntries;

            if (diff > 0)
            {
                for (int i = _mruList.Count - 1; i >= 0; i--)
                {
                    _mruList.Remove(_mruList[i]);

                    diff--;

                    if (diff == 0)
                    {
                        break;
                    }
                }
            }
        }

        public void AddRange(List<string> values)
        {
            _mruList.Clear();
            _mruList.AddRange(values);
            EnsureMaxNumberOfEntries();
            Render();
        }

        private void RemoveCaseSensitive(string value)
        {
            for (int i = _mruList.Count - 1; i >= 0; i--)
            {
                if (string.Compare(_mruList[i], value, true) == 0)
                {
                    _mruList.Remove(_mruList[i]);
                }                
            }
        }

        public void Remove(string value)
        {
            RemoveCaseSensitive(value);
            //_mruList.Remove(value);
            
            Render();
        }

        public void Clear()
        {
            _mruList.Clear();
            CleanUp();
        }

        private ToolStripMenuItem ParentMenuItem
        {
            get
            {
                if (_insertBeforeMenuItem != null)
                {
                    return (ToolStripMenuItem)_insertBeforeMenuItem.OwnerItem;
                }

                return null;
            }
        }

        public void Render()
        {
            CleanUp();

            if (_mruList == null || _mruList.Count == 0)
            {
                return;
            }

            int index = 0;
            foreach (ToolStripItem item in ParentMenuItem.DropDownItems)
            {
                if (item == _insertBeforeMenuItem)
                {
                    break;
                }

                index++;
            }

            if (index > 0)
            {
                if (!(ParentMenuItem.DropDownItems[index - 1] is ToolStripSeparator))
                {
                    ToolStripSeparator separatorStart = new ToolStripSeparator();
                    ParentMenuItem.DropDownItems.Insert(index, separatorStart);
                    _addedToolStripItems.Add(separatorStart);
                    index++;
                }
            }

            int mruCount = 0;

            foreach (string mruItem in _mruList)
            {
                mruCount++;

                string menuText = string.Format("&{0} {1}", mruCount, FileSystem.CompactPath(mruItem));
                ToolStripMenuItem mruMenuItem = new ToolStripMenuItem(menuText);

                mruMenuItem.Tag = mruItem;
                mruMenuItem.Click += new EventHandler(mruMenuItem_Click);
                ParentMenuItem.DropDownItems.Insert(index, mruMenuItem);
                _addedToolStripItems.Add(mruMenuItem);
                index++;
            }

            ToolStripSeparator separatorEnd = new ToolStripSeparator();
            ParentMenuItem.DropDownItems.Insert(index, separatorEnd);
            _addedToolStripItems.Add(separatorEnd);
        }

        private void CleanUp()
        {
            if (ParentMenuItem == null)
            {
                return;
            }

            foreach (ToolStripItem item in _addedToolStripItems)
            {
                item.Click -= mruMenuItem_Click;
                ParentMenuItem.DropDownItems.Remove(item);
            }

            _addedToolStripItems.Clear();
        }

        void mruMenuItem_Click(object sender, EventArgs e)
        {
            if (OnMruMenuItemClick != null)
            {
                if (((ToolStripMenuItem)sender).Tag != null)
                {
                    string data = ((ToolStripMenuItem)sender).Tag.ToString();
                    Add(data);
                    OnMruMenuItemClick(data);
                }
            }
        }

        public void Load()
        {
            if (!File.Exists(DataFile))
            {
                return;
            }

            XmlSerializer ser = new XmlSerializer(_mruList.GetType());

            using (FileStream fs = new FileStream(DataFile, FileMode.Open))
            {
                _mruList = (List<string>)ser.Deserialize(fs);
                fs.Close();
            }
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(DataFile))
            {
                return;
            }

            XmlSerializer ser = new XmlSerializer(_mruList.GetType());

            using (FileStream fs = new FileStream(DataFile, FileMode.Create))
            {
                ser.Serialize(fs, _mruList);
                fs.Close();
            }
        }
    }
}
