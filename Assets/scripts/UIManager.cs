using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace spacegame
{
    // REMEMBER THIS IS ATTACHED TO THE CANVAS PREFAB
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;
        public Queue<UI> inputQueue = new Queue<UI>();

        private bool pressedSelect => Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X);
        private bool pressedVertical => Input.GetAxis("Vertical") < 0 || Input.GetAxis("Vertical") > 0;

        private void Awake()
        {
            instance = this;
            StartCoroutine(ProcessInputQueue());
        }

        // pretty much 100% sure there's a better way to do this
        public IEnumerator ProcessInputQueue()
        {
            while (true)
            {
                StartOfLoop:
                // wait until we've either pressed select or the vertical keys
                yield return new WaitUntil(() => pressedSelect || pressedVertical);

                // only continue if the queue isn't empty
                if (inputQueue.Count == 0) goto StartOfLoop; // i tried using a continue statement here but it just broke out of the whole coroutine? so you get labels in c# get pranked

                // get the first object
                UI ui = inputQueue.Peek();

                if (pressedVertical && ui is UINavigateable)
                {
                    (ui as UINavigateable).Navigate(Input.GetAxis("Vertical") > 0);
                    yield return new WaitForSeconds(0.2f);
                }
                else if (pressedSelect)
                {
                    ui.inputProcessedCallback.Invoke(); // invoke the callback

                    // check if the ui has been destroyed, and if it has, dequeue it
                    if (ui.readyToDequeue) // unity doesn't actually dispose objects when you use destroy for no reason
                        inputQueue.Dequeue();
                }

                yield return new WaitForEndOfFrame();

                // loop after the end of the frame
            }
            throw new Exception("broke out of ui input queue loop");
        }

        public UI New(Vector2 position, Vector2 size) 
        {
            // get prefab from PrefabManager and instantiate it as a child of the canvas
            GameObject prefab = PrefabManager.instance.GetPrefab("ui");
            GameObject canvas = CommonObject.GetCommonObject("Canvas");
            // canvas MissingReferenceException
            GameObject g = Instantiate(prefab, position + (Vector2)canvas.transform.position, Quaternion.identity, canvas.transform);

            // get ui from gameobject and call the initialize method
            UI ui = g.GetComponent<UI>();
            ui.Initialize(size);
            return ui; // return the ui
        }

        public UINavigateable NewNavigateable(Vector2 position, Vector2 size, 
            UI.PrintTextOptions customPrintTextOptions = UI.PrintTextOptions.None)
        {
            // get prefab from PrefabManager and instantiate it as a child of the canvas
            GameObject prefab = PrefabManager.instance.GetPrefab("ui navigateable");
            GameObject canvas = CommonObject.GetCommonObject("Canvas");
            GameObject g = Instantiate(prefab, position + (Vector2)canvas.transform.position, Quaternion.identity, canvas.transform);

            // initialize
            UINavigateable ui = g.GetComponent<UINavigateable>();
            ui.customPrintTextOptions = customPrintTextOptions;
            ui.Initialize(size);

            return ui; // return the ui
        }
    }
}