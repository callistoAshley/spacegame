// THIS IS HORRIBLE AND NEEDS TO BE REFACTORED
// BUT I DON'T FEEL LIKE IT! :D
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using UnityEditor;
using System.IO;
using System.Linq;

namespace spacegame.alisonscript
{
    public static class Interpreter
    {
        // reflection hurts my head man
        public static void RegisterFunctions()
        {
            // iterate through methods in Functions type via reflection
            foreach (MethodInfo m in typeof(Functions).GetMethods())
            {
                var v = m.GetCustomAttributes(typeof(FunctionAttribute));
                if (v.Count() > 0) // yeah!!! it's a function!!! let's go!!!!
                {
                    // get the name of the function from the function attribute and add it as a key to the functions dictionary
                    // haha this isn't a mess at all
                    FunctionAttribute f = (FunctionAttribute)Attribute.GetCustomAttribute(m, typeof(FunctionAttribute));
                    string functionName = f.name;
                    Functions.functions.Add(functionName, m.Name);
                    Debug.Log($"registered alisonscript function: {functionName} ({m.Name})");
                }
            }
        }

        public static async void Run(string script)
        {
            // read lines of file
            string[] file = File.ReadAllLines(Application.dataPath + "/lang/en/" + script + ".alisonscript");

            // create running script
            RunningScript runningScript = new RunningScript(Line.FromStringArray(file));

            foreach (Line line in runningScript)
            {
                await line.Process();
            }
        }
    }
}
