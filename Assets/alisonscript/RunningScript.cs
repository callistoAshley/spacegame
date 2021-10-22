using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using UnityEngine;

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
                    Finished();
                    Interpreter.runningScript = null;
                    return;
                }

                _lineIndex = value;
                lines[_lineIndex].Process(this);
            }
        }

        // NEVER use this get the actual line index, instead just get lineIndex
        // this is just a tidy way of getting the current line for syntax errors
        public int GetCurrentLine()
        {
            return lineIndex + 1;
        }

        public void IncrementIndex()
        {
            Interpreter.runningScript.lineIndex++;
        }

        public void Finished()
        {
            Controller.instance.canMove = true;
        }

        public RunningScript(List<Line> lines)
        {
            this.lines = lines;
        }
    }
}
