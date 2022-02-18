using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spacegame.alisonscript
{
    public sealed class EndKeyword : IKeyword
    {
        public string name => "end";
        public int minimumArgs => 0;

        public void OnCall(RunningScript script, Line line, string[] args)
        {
            // badoing
            if (script.encapsulationStack.Count == 0)
                throw new AlisonscriptSyntaxError(script.GetCurrentLine(), "stray end keyword");
            // pop the encapsulation stack and continue
            script.encapsulationStack.Pop();
            Interpreter.EvaluateNextLine();
        }
    }
}
