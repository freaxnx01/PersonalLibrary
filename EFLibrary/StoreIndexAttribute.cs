using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFLibrary
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
    public class StoreIndexAttribute : Attribute
    {
        public bool Unique { get; set; }

        public StoreIndexAttribute() {}
        public StoreIndexAttribute(bool unique)
        {
            Unique = unique;
        }
    }
}
