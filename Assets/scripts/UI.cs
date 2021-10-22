using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace spacegame
{
    public class UI : MonoBehaviour
    {
        public Text text;
        public Action inputProcessedCallback; // called when dequeued from the UIManager input queue

        public List<UI> alsoDestroy = new List<UI>(); // ui to destroy when destroying this one 

        public enum PrintTextOptions
        {
            // you get binary because i hate you that much <3
            CallbackBeforePrinting = 0b_0000_0001,
            CallbackAfterPrinting = 0b_0000_0010,
            CallbackAfterInput = 0b_0000_0100,
            DontCallback = 0b_0000_1000,
            DestroyUIAfterCallback = 0b_0001_0000,
            Instant = 0b_0010_0000,
        }

        public virtual void Initialize(Vector2 size)
        {
            text = GetComponentInChildren<Text>();

            // set box scale
            RectTransform rect = GetComponent<RectTransform>();
            
            rect.sizeDelta = size;

            // set text position and scale
            Image image = GetComponent<Image>();
            RectTransform textRect = text.GetComponent<RectTransform>();
            textRect.localPosition = new Vector2(0, 0); // don't remember why this is set, try removing it later
        }

        // also check out the stack overflow page i stole this code from: https://www.taste.com.au/recipes/collections/spaghetti-recipes
        public virtual IEnumerator PrintText(
            string text, // text input
            Action callback = null, // callback (in alisonscript this is usually just increment the line index)
            PrintTextOptions options = PrintTextOptions.CallbackAfterInput) // options
        {
            // automatically skip through the text if holding left ctrl
            if (Input.GetKey(KeyCode.LeftControl))
            {
                this.text.text = text;
                yield return new WaitForEndOfFrame(); // i want it to have like. you know. that oomph. you know. the oomph
                if (callback != null)
                    callback.Invoke();
                DisposeButNotReally();
                yield break;
            }

            if (options.HasFlag(PrintTextOptions.CallbackBeforePrinting))
                callback.Invoke();

            if (options.HasFlag(PrintTextOptions.Instant))
            {
                this.text.text = text;
            }
            else
            {
                // for all of the characters in the input string, append the character to the ui's text
                for (int i = 0; i < text.Length; i++)
                {
                    this.text.text += text[i];
                    yield return new WaitForSeconds(0.01f * Time.deltaTime);

                    // check for confirm or cancel key, and either is being pressed skip text
                    if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X))
                    {
                        this.text.text = text;
                        break;
                    }
                }
            }

            yield return new WaitForSeconds(0.1f);

            // this is gross
            if (options.HasFlag(PrintTextOptions.CallbackAfterPrinting))
            {
                if (callback != null)
                    callback.Invoke();
                if (options.HasFlag(PrintTextOptions.DestroyUIAfterCallback)) // destroy this ui if configured to
                    DisposeButNotReally();
            }
            if (options.HasFlag(PrintTextOptions.CallbackAfterInput))
            {
                // if a callback wasn't given and we're told to destroy the ui, destroy the ui
                if (callback is null && options.HasFlag(PrintTextOptions.DestroyUIAfterCallback))
                {
                    inputProcessedCallback = new Action(() => DisposeButNotReally());
                }
                // if a callback was given and we're told to destroy the ui, invoke the callback and destroy the ui
                else if (callback != null && options.HasFlag(PrintTextOptions.DestroyUIAfterCallback))
                {
                    inputProcessedCallback = new Action(() =>
                    {
                        callback.Invoke();
                        DisposeButNotReally();
                    });
                }
                else
                {
                    throw new Exception("genuinely can't think of a reason i'd do this so i'll just throw an exception and hope i don't find it later");
                }

                UIManager.instance.inputQueue.Enqueue(this);
            }
        }

        public void AddToInputQueue()
        {
            UIManager.instance.inputQueue.Enqueue(this);
        }

        public void DisposeButNotReally()
        {
            foreach (UI ui in alsoDestroy)
                ui.DisposeButNotReally();
            Destroy(gameObject);
        }
    }
}
