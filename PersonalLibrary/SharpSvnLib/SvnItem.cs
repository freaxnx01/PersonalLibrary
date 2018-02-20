using System;
using System.Collections.Generic;
using System.Text;

using SharpSvn;

namespace SharpSvnLib
{
    public class SvnItem
    {
        public SvnUriTarget SvnTarget { get; set; }
        public long Revision { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
    }
}
