using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    [Serializable]
    public class AssemblyInfo
    {
        public string FullName { get; set; }
        public string Location { get; set; }
        public string CodeBase { get; set; }
        public string ImageRuntimeVersion { get; set; }
        public bool GlobalAssemblyCache { get; set; }
    }
}
