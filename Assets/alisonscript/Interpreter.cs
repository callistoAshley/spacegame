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
        private static RunningScript _runningScript;
        public static RunningScript runningScript
        {
            get
            {
                return _runningScript;
            }
            set
            {
                if (!(_runningScript is null) && !(value is null))
                    throw new Exception("shouldn't set the running script to a value while a script is running");
                _runningScript = value;
            }
        }

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
                    Functions.instance.functions.Add(functionName, m.Name);
                    Debug.Log($"registered alisonscript function: {functionName} ({m.Name})");
                }
            }
        }

        public static void Run(string script)
        {
            if (_runningScript != null)
                throw new Exception("cannot run a new alisonscript script while the runningScript instance has a value (a script is already running)");

            if (!File.Exists(Application.dataPath + "/lang/en/" + script + ".alisonscript"))
                throw new Exception($"couldn't find alisonscript file \"/lang/en/{script}\"");

            // read lines of file
            string[] file = File.ReadAllLines(Application.dataPath + "/lang/en/" + script + ".alisonscript");

            // create running script
            runningScript = new RunningScript(Line.FromStringArray(file));

            // process first line
            runningScript.lines[0].Process(runningScript); 
        }
    }
}
