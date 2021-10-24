using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spacegame.alisonscript
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FunctionAttribute : Attribute
    {
        public string name;
        public int minimumArgs;

        public FunctionAttribute(string name) : this(name, 0) { }

        public FunctionAttribute(string name, int minimumArgs)
        {
            this.name = name;
            this.minimumArgs = minimumArgs;
        }
    }
}
