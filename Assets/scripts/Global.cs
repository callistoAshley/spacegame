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
        public static bool debugMode;

        // Start is called before the first frame update
        void Awake()
        {
            // singleton stuff
            DontDestroyOnLoad(gameObject);
            instance = this;

            Init.Initialization();
        }

        public void EnterDebugMode()
        {
            debugMode = true;
            GameState.SetBoolean("debug_mode", true);

            // create debug ui
            Instantiate(PrefabManager.instance.GetPrefab("debug ui"), UIManager.instance.transform);

            // register debug commands
            DebugConsole.Commands.RegisterCommands();
        }

        // the purpose of this coroutine is to give the init scene a moment to load so the loading text appears
        // as Resources.LoadAll is pretty slow
        // this is commented out for now because i don't load many resources but i might need it in the future
        /*
        IEnumerator Thing()
        {
            yield return new WaitForSeconds(0.1f);
            Init.Initialization();
        }*/
    }
}
