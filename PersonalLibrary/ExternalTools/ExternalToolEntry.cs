using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Library.Gui.ExternalTools
{
    public class ExternalToolEntry
    {
        public string Title { get; set; }
        public string Command { get; set; }
        public string Arguments { get; set; }
        public string InitialDirectory { get; set; }

        public ExternalToolEntry()
        {
            Title = string.Empty;
            Command = string.Empty;
            Arguments = string.Empty;
            InitialDirectory = string.Empty;
        }
    }
}
