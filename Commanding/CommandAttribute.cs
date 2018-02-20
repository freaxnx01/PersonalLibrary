using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commanding
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        public string Name { get; set; }

        public CommandAttribute()
        {

        }

        public CommandAttribute(string name)
        {
            Name = name;
        }
    }
}
