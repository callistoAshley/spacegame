using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace spacegame.alisonscript
{
    public class RunningScript 
    {
        public List<Line> lines;
        public Dictionary<string, Object> objects = new Dictionary<string, Object>();
        
        private int _lineIndex;
        public int lineIndex
        {
            get
            {
                return _lineIndex;
            }
            set
            {
                if (value > lines.Count - 1)
                {
                    // done!
                    Interpreter.runningScript = null;
                    return;
                }

                _lineIndex = value;
                lines[_lineIndex].Process(this);
            }
        }

        public void IncrementIndex()
        {
            Interpreter.runningScript.lineIndex++;
        }

        public RunningScript(List<Line> lines)
        {
            this.lines = lines;
        }
    }
}
