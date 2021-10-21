using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace spacegame
{
    // REMEMBER THIS IS ATTACHED TO THE CANVAS PREFAB
    public class UIManager : ScriptableObject
    {
        public static UIManager instance;
        // ui input queue

        public static void SetInstance()
        {
            if (instance is null)
                instance = CreateInstance<UIManager>();
            else
                throw new Exception("UIManager singleton instance already has a value");
        }

        public UI New(Vector2 position, Vector2 size)
        {
            // get prefab from PrefabManager and instantiate it as a child of the canvas
            GameObject prefab = PrefabManager.GetPrefab("ui");
            GameObject canvas = PrefabManager.GetPrefab("Canvas");
            GameObject g = Instantiate(prefab, position + (Vector2)canvas.transform.position, Quaternion.identity, canvas.transform);

            // get ui from gameobject and call the initialize method
            UI ui = g.GetComponent<UI>();
            ui.Initialize(size);
            return ui; // return the ui
        }
    }
}