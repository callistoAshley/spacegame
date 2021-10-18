using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace spacegame.alisonscript
{
    public class RunningScript : IEnumerable
    {
        private List<Line> lines;

        public RunningScript(List<Line> lines)
        {
            this.lines = lines;
        }

        public IEnumerator GetEnumerator()
        {
            return new ScriptEnumerator(lines);
        }

        private class ScriptEnumerator : IEnumerator
        {
            public List<Line> lines; // enumerator data
            private int position = -1; // position in enumeration

            public ScriptEnumerator(List<Line> lines)
            {
                this.lines = lines;
            }

            // current object in enumeration
            public object Current
            {
                get
                {
                    return lines[position];
                }
            }

            public bool MoveNext()
            {
                // increment the position
                position++;
                return position < lines.Count;
            }

            public void Reset()
            {
                position = -1;
            }
        }
    }
}
