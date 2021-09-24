using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spacegame.alisonscript
{
    public class Line
    {
        public string contents;

        public Line(string contents)
        {
            this.contents = contents;
        }

        public static Line[] FromStringArray(string[] array)
        {
            List<Line> lines = new List<Line>();

            // initialize a line from every string in the array and add it to the list
            foreach (string s in array)
                lines.Add(new Line(s));

            // convert the list to an array and return
            return lines.ToArray();
        }
    }
}
