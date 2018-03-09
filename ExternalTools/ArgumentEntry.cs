using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Gui.ExternalTools
{
    public class ArgumentEntry : EntryBase
    {
        public ArgumentEntry(string name, string title, string value)
        {
            base.Name = name;
            base.Title = title;
            base.Value = value;
        }

        public ArgumentEntry(string title)
        {
            base.Title = title;
        }
    }
}
