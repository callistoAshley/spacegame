using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace spacegame.alisonscript
{
    public sealed class FunctionKeyword : IKeyword
    {
        // the constructor doesn't have to do anything, it's just so that it can be initialized and put into the keywords list
        public FunctionKeyword() { }

        public string name => ";";

        public void OnCall(RunningScript script, Line line)
        {
            string functionName = line.contents.Substring(1); // get the function name by substringing the line to exclude the semicolon
            // then we just get rid of the args by cutting of the string at the first mention of a white space or quote
            functionName = new Regex("\".*?\"|\\s").Replace(functionName, string.Empty);

            // get the args
            string[] args = Interpreter.ArgsRegex(line.contents);

            // call the function
            Functions.instance.Call(functionName, Interpreter.EvaluateNextLine, args);
        }
    }
}
