using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Text.RegularExpressions;

namespace spacegame.alisonscript
{
    public class Functions : MonoBehaviour
    {
        public static Functions instance;
        public Dictionary<string, string> functions = new Dictionary<string, string>();

        private void Awake()
        {
            //DontDestroyOnLoad(gameObject);
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

        [Function("spk")]
        public IEnumerator Speak(FunctionArgs args)
        {
            string fullText = args.args[0];
            UI speakerBox = null;

            // start by getting the speaker, which we do with a regex
            Regex searchForSpeaker = new Regex(@"(?<=\|)(.+?)(?=\|)");
            if (searchForSpeaker.IsMatch(fullText)) // nice! we have a speaker! let's go!
            {
                // create speaker box
                speakerBox = UIManager.instance.New(new Vector2(-152, 328), new Vector2(500, 100));

                // print the speaker onto the speaker box
                speakerBox.PrintText(searchForSpeaker.Match(fullText).Value, true, 
                    callbackPosition: UI.PrintTextCallbackPosition.AfterPrinting); // since we don't have a callback just do this

                // then replace the contents of the speaker with an empty string
                // this regex includes the bars, eg "|guy| what a lovely day!!!" matches "|guy|"
                Regex speaker = new Regex(@"\|.*?\|");
                speaker.Replace(fullText, string.Empty);

                // i normally like adding an extra whitespace after the speaker so it's more readable so this checks for that and removes it
                // you can disable this check by including "-ndws" (no delete white space) as an argument

                if (fullText[0] == ' ' && !args.args.Contains("-ndws"))
                    fullText = fullText.Substring(1);
            }

            // create textbox
            UI textbox = UIManager.instance.New(new Vector2(-52, 169), new Vector2(700, 200));
            if (!(speakerBox is null)) // destroy the speaker box when we destroy the textbox
                textbox.alsoDestroy.Add(speakerBox);
            textbox.StartCoroutine(textbox.PrintText(fullText, false, args.callback, UI.PrintTextCallbackPosition.AfterInput, true));
            
            yield break;
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
