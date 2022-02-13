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
        public readonly List<Line> lines;
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
                    return;
                }

                _lineIndex = value;
            }
        }
        public int depth;

        public bool finished;

        public RunningScript(List<Line> lines)
        {
            this.lines = lines;
        }

        public bool ConditionalTrue(string objectName, string value)
        {
            // if "True" or "False" gets passed through as the object name, parse the object name to a bool and return it
            if (bool.TryParse(objectName, out bool result))
                return result;

            if (!objects.ContainsKey(objectName))
                throw new AlisonscriptSyntaxError(GetCurrentLine(), $"the current script does not have an object called {objectName}");

            // compare the object's value to the inputted value
            return objects[objectName].value == value;
        }

        // NEVER use this to get the actual line index, instead just get lineIndex
        // this is just a tidy way of getting the current line for syntax errors
        // e.g. throw new AlisonscriptSyntaxError(runningScript.GetCurrentLine(), "cometh hithereth, don the dunce hat");
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
            finished = true;
            Player.instance.canMove = true;
            Interpreter.interpreterRunning = false;
        }
        
        public Line GetLabelByName(string name)
        {
            IEnumerable<Line> labels = from line in lines where line.labelData.isLabel select line;
            
            foreach (Line line in labels)
                if (line.labelData.labelName == name)
                    return line;
            throw new Exception($"no such label with the name \"{name}\"");
        }

        public void JumpToNextOccurence(int start, string input)
        {
            for (int i = start + 1; i < lines.Count; i++)
            {
                // remove indenting
                Regex indenting = new Regex(@"\A\s*");
                string line = indenting.Replace(lines[i], string.Empty);

                if (line.StartsWith(input)
                    || (input == "when" && line.StartsWith("end"))) // also jump to end if the input is when to break out of conditionals
                {
                    lineIndex = i;

                    if (Interpreter.runningScript == null) // how does this happen part 2
                    {
                        Logger.WriteLine("running script is null " + lineIndex);
                        return;
                    }

                    // break out of conditional if the line isn't in one
                    //if (!Interpreter.runningScript.lines[lineIndex].inConditional)
                        //Interpreter.runningScript.inCond = false;
                    return;
                }
            }
            throw new Exception($"couldn't find an occurence of {input} from {start}");
        }

        public void AddObject(string objectName, string objectValue)
        {
            Logger.WriteLine($"adding object: {objectName} with value: {objectValue}");
            if (Interpreter.runningScript.objects.ContainsKey(objectName))
                // if the script already has an object with the name objectName, set its value
                Interpreter.runningScript.objects[objectName].value = objectValue;
            else
                // otherwise, create it and add it
                Interpreter.runningScript.objects.Add(objectName, new Object(objectValue));
        }
    }
}
