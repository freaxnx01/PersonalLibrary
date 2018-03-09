using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Gui.ExternalTools
{
    public class EntryBase
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }

        public string VariableName
        {
            get
            {
                if (!string.IsNullOrEmpty(Name))
                {
                    return string.Format("$({0})", Name);
                }

                return string.Empty;
            }
        }
    }
}
