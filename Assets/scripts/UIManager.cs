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
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X));

                // if the queue isn't empty
                if (inputQueue.Count != 0)
                {
                    UI ui = inputQueue.Peek(); // get the first object
                    ui.inputProcessedCallback.Invoke(); // invoke the callback

                    yield return new WaitForEndOfFrame();

                    // check if the ui has been destroyed, and if it has, dequeue it
                    if (ui == null) // apparently there's something wrong with is null??? i thought it was tomato tomahto but there's defo something i don't understand
                        inputQueue.Dequeue();
                }
            }
        }

        public UI New(Vector2 position, Vector2 size) 
        {
            // get prefab from PrefabManager and instantiate it as a child of the canvas
            GameObject prefab = PrefabManager.instance.GetPrefab("ui");
            GameObject canvas = Global.GetCommonObject("Canvas");
            GameObject g = Instantiate(prefab, position + (Vector2)canvas.transform.position, Quaternion.identity, canvas.transform);

            // get ui from gameobject and call the initialize method
            UI ui = g.GetComponent<UI>();
            ui.Initialize(size);
            return ui; // return the ui
        }
    }
}