using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spacegame.alisonscript
{
    public class Cutscene : Object
    {
        public List<Line> lines;

        public Cutscene(List<Line> lines)
        {
            this.lines = lines;
        }
    }
}
