using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spacegame.alisonscript
{
    public class Label
    {
        public string name; 
        public int index; // index in the script
        public string[] args; 
        
        public Label(string name, int index, string[] args)
        {
            this.name = name;
            this.index = index;
            this.args = args;
        } 
    }
}
