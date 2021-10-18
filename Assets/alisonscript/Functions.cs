using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace spacegame.alisonscript
{
    public static class Functions 
    {
        public static Dictionary<string, string> functions = new Dictionary<string, string>();

        public static async Task Call(string functionName, params string[] args)
        {
            foreach (string s in functions.Keys)
            {
                if (s == functionName)
                {
                    Debug.Log($"calling function: {functionName}");
                    // just create a task out of a lambda that starts the coroutine
                    // also use global's start coroutine method because i don't want feel like doing singleton stuff to make this inherit from MonoBehaviour
                    Task t = new Task(() => { Debug.Log("hey"); Global.instance.StartCoroutine(functions[s], args); });
                    await t;
                    Debug.Log("WHY"); 
                }
            }
            throw new Exception($"{functionName} didn't match any registered alisonscript functions");
        }

        [Function("log")]
        public static IEnumerator Log(params string[] args)
        {
            foreach (string s in args) Debug.Log(s);   
            yield break;
        }
    }
}
