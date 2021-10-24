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
        public List<Label> labels;
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

        public RunningScript(List<Line> lines)
        {
            this.lines = lines;
            labels = new List<Label>();

            // create labels
            foreach (
                Line line in from line in lines where // why do you call it oven when you of in the cold food of out hot eat the food
                line.contents.StartsWith("&") select line)
            {
                // use the same regex in Line to just get the name of the label without the args
                string labelName = new Regex("\".*?\"|\\s").Replace(line.contents, string.Empty);

                // then get the args 
                string[] args = Line.ArgsRegex(line);

                // then create the label and add it to the labels list
                labels.Add(new Label(labelName, line.index, args));
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

        public Label GetLabelByName(int start, string name)
        {
            for (int i = start; i < labels.Count; i++)
                if (labels[i].name == name)
                    return labels[i];
            throw new Exception($"a label with the name \"{name}\" could not be found from index position {start}");
        }
    }
}
