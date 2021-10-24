using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace spacegame.alisonscript
{
    // dumpster fire class
    public class Line
    {
        public string contents;
        public int index;

        public Line(string contents, int index)
        {
            this.contents = contents;
            this.index = index;
        }

        // i stole this regex lol https://stackoverflow.com/questions/49239218/get-string-between-character-using-regex-c-sharp
        // this is used in Label too and it's a bit lengthy so i made it a method
        public static string[] ArgsRegex(string input)
        {
            Regex searchForArgs = new Regex("(?<=\")(.+?)(?=\")");
            var argsCount = searchForArgs.Matches(input);

            List<string> args = new List<string>();
            for (int i = 0; i < argsCount.Count; i++)
                // only treat the argument as an argument if it's the first argument or its index in the array isn divisible by 2
                // this avoids treating the empty whitespaces between arguments as arguments,
                // e.g. ;log "hello" "dooby" logs "hello" and "dooby" instead of "hello" " " and "dooby"
                if (i == 0 || i % 2 == 0)
                    args.Add(argsCount[i].Value);

            return args.ToArray();
        }

        public void Process(RunningScript runningScript)
        {
            string line = contents;

            // remove indenting with regex to get the actual line
            Regex indenting = new Regex(@"\A\s*");
            line = indenting.Replace(line, string.Empty);

            // if the line is empty or starts with a #, treat it as a comment and return here
            if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
            {
                Interpreter.runningScript.IncrementIndex();
                return;
            }

            // if the line starts with a semicolon, call a function
            if (line.StartsWith(";"))
            {
                // this is absolutely horrible!
                string functionName = line.Substring(1); // get the function name by substringing the line to exclude the semicolon
                // then we just get rid of the args by cutting of the string at the first mention of a white space or quote
                functionName = new Regex("\".*?\"|\\s").Replace(functionName, string.Empty);

                if (functionName == string.Empty)
                    throw new AlisonscriptSyntaxError(Interpreter.runningScript.GetCurrentLine(), "function call expected");

                // get args from the function call
                string[] args = ArgsRegex(line);

                // if there were no args, throw an syntax error
                if (args.Length == 0)
                    throw new AlisonscriptSyntaxError(Interpreter.runningScript.GetCurrentLine(), "tried to call a function without arguments");

                // then call the function 
                if (Interpreter.runningScript is null) // how does this happen
                {
                    Debug.Log("i guess we're done?");
                    return;
                }
                Functions.instance.Call(functionName, () => Interpreter.runningScript.IncrementIndex(), args); // increment into lineIndex as callback
            }
            // if the line starts with an @ then it's referring to an object, so set it to a value
            else if (line.StartsWith("@"))
            {
                string objectName = string.Empty;
                objectName = new Regex("\".*?\"|\\s").Replace(objectName, string.Empty); // remove everything after the whitespace

                string[] args = ArgsRegex(line); // args

                if (args.Length == 0)
                    throw new AlisonscriptSyntaxError(Interpreter.runningScript.GetCurrentLine(), "value assignment expected");

                string objectValue = args[0];

                if (Interpreter.runningScript.objects.ContainsKey(objectName)) 
                    // if the script already has an object with the name objectName, set its value
                    Interpreter.runningScript.objects[objectName].value = objectValue;
                else 
                    // otherwise, create it and add it
                    Interpreter.runningScript.objects.Add(objectName, new Object(objectValue));

                Interpreter.runningScript.IncrementIndex();
            }
            // if the line starts with a & then it's referring to a label
            else if (line.StartsWith("&"))
            {
                line = line.Substring(1);
                // get label name
                string labelName = string.Empty;
                labelName = new Regex("\".*?\"|\\s").Replace(labelName, string.Empty);

                string[] args = ArgsRegex(line);

                // some labels have special behaviour, like if/end
                switch (labelName)
                {
                    // continue until the next end label if a condition is met
                    case "if":
                        // if the condition is true, continue
                        // otherwise, go to end
                        if (Interpreter.runningScript.objects[args[0]].value != args[1])
                        {
                            Interpreter.runningScript.lineIndex =
                                Interpreter.runningScript.GetLabelByName(Interpreter.runningScript.lineIndex,
                                // you can supply a third argument to if to indicate the label to jump to
                                // e.g. if "@cool_object" "hello" "end1" jumps to "end1" 
                                // because i don't feel like working out nested if statement mayhem
                                args.Length == 3 ? args[2] : "end").index;
                        }
                        break;

                    // inverse if - continue until the next end label if a condition is not met
                    case "!if":
                        // condition true
                        if (Interpreter.runningScript.objects[args[0]].value == args[1])
                        {
                            Interpreter.runningScript.lineIndex =
                                Interpreter.runningScript.GetLabelByName(Interpreter.runningScript.lineIndex,
                                args.Length == 3 ? args[2] : "end").index;
                        }
                        break;
                }

                Interpreter.runningScript.IncrementIndex();
            }
        }

        public static List<Line> FromStringArray(string[] array)
        {
            if (array.Length == 0)
                throw new Exception("array is empty");
            List<Line> lines = new List<Line>();

            // initialize a line from every string in the array and add it to the list
            for (int i = 0; i < array.Length; i++)
                lines.Add(new Line(array[i], i));

            // convert the list to an array and return
            return lines;
        }

        // overload string cast to just return the contents of the line for efficiency
        public static implicit operator string(Line line)
        {
            return line.contents;
        }
    }
}
