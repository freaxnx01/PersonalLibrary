using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Gui.ExternalTools
{
    public class InitDirEntry : EntryBase
    {
        public InitDirEntry(string name, string title, string value)
        {
            base.Name = name;
            base.Title = title;
            base.Value = value;
        }

        public InitDirEntry(string title)
        {
            base.Title = title;
        }
    }
}
