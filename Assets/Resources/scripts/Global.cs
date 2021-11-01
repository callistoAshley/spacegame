using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System;

namespace spacegame
{
    // Project Settings > Script Execution Order for all of this mayhem
    public class Global : MonoBehaviour
    {
        public static Global instance;
        public static bool initialized;

        public Dictionary<string, GameObject> commonObjects = new Dictionary<string, GameObject>();

        // Start is called before the first frame update
        void Awake()
        {
            // initialize other stuff
            if (initialized) return;
            initialized = true;

            Init();
        }
        /*
        private void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("scene loaded " + scene.name);
            commonObjects.Clear();
            instance = this;
        }*/

        private void Init()
        {
            // singleton stuff
            DontDestroyOnLoad(gameObject);
            instance = this;

            // alisonscript initialization
            alisonscript.Interpreter.RegisterFunctions();

            // then go to title screen
            SceneManager.LoadScene("title");
        }

        public GameObject GetCommonObject(string name)
        {
            foreach (string s in commonObjects.Keys)
                if (s == name)
                    return commonObjects[s];
            throw new System.Exception($"the common objects list does not have a key called {name}");
        }
    }
}
