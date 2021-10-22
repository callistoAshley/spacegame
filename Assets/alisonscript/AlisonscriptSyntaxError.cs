using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spacegame.alisonscript
{
    public class AlisonscriptSyntaxError : Exception
    {
        private static string ErrorText(int line) => $"alisonscript syntax error on line {line}: ";

        public AlisonscriptSyntaxError() : base() { }
        public AlisonscriptSyntaxError(int line, string message) : base (ErrorText(line) + message) { }
        public AlisonscriptSyntaxError(int line, string message, Exception innerException) : base(ErrorText(line) + message, innerException) { }
    }
}
