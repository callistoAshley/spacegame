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

        // y'all like unnecessarily long and verbose variable names
        public enum PrintTextCallbackPosition
        {
            BeforePrinting,
            AfterPrinting,
            AfterInput,
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
            bool instant, // whether the text should appear instantaneously
            Action callback = null, // callback (in alisonscript this is usually just increment the line index)
            PrintTextCallbackPosition callbackPosition = PrintTextCallbackPosition.AfterInput, // where in the method the callback should be invoked
            bool destroyUiAfterCallback = true) // whether the ui should be destroyed after the callback is invoked
        {
            // add this ui to the input queue
            UIManager.instance.inputQueue.Enqueue(this);

            if (callbackPosition == PrintTextCallbackPosition.BeforePrinting)
                callback.Invoke();

            if (instant)
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

            // i made this a switch to end the unending hell of if statements but honestly i have no idea whether it looks worse
            switch (callbackPosition)
            {
                // if we're told to invoke the callback 
                case PrintTextCallbackPosition.AfterPrinting:
                    callback.Invoke();
                    if (destroyUiAfterCallback) // destroy this ui if configured to
                        DisposeButNotReally();
                    yield break; // break out of the coroutine here

                case PrintTextCallbackPosition.AfterInput:
                    // if a callback wasn't given and we're told to destroy the ui, destroy the ui
                    if (callback is null && destroyUiAfterCallback)
                    {
                        inputProcessedCallback = new Action(() => DisposeButNotReally());
                    }
                    // if a callback was given and we're told to destroy the ui, invoke the callback and destroy the ui
                    else if (callback != null && destroyUiAfterCallback)
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
                    break;
            }
        }

        public void DisposeButNotReally()
        {
            foreach (UI ui in alsoDestroy) 
                if (ui != null)
                    ui.DisposeButNotReally();
            if (this != null) // how does this happen
                Destroy(gameObject);
        }
    }
}
