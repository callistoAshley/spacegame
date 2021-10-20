using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace spacegame.alisonscript
{
    public class Functions : MonoBehaviour
    {
        public static Functions instance;
        public Dictionary<string, string> functions = new Dictionary<string, string>();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        public void Call(string functionName, Action callback, params string[] args)
        {
            foreach (string s in functions.Keys)
            {
                if (s == functionName)
                {
                    // call function coroutine
                    StartCoroutine(functions[s], new FunctionArgs(callback, args));
                    return;
                }
            }
            throw new Exception($"{functionName} didn't match any registered alisonscript functions");
        }

        [Function("log")]
        public IEnumerator Log(FunctionArgs args)
        {
            foreach (string s in args.args) 
                Debug.Log(s);
            args.callback.Invoke();
            
            yield break;
        }

        [Function("wait")] 
        public IEnumerator Wait(FunctionArgs args)
        {
            yield return new WaitForSeconds(int.Parse(args.args[0]));
            args.callback.Invoke();
        }

        public struct FunctionArgs
        {
            public Action callback;
            public string[] args;

            public FunctionArgs(Action callback, params string[] args)
            {
                this.callback = callback;
                this.args = args;
            }
        }
    }
}
