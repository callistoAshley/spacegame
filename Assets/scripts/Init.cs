using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace spacegame
{
    public static class Init
    {
        public static bool initialized;

        public static void Initialization()
        {
            if (initialized) return;
            initialized = true;

            // alisonscript initialization
            alisonscript.Interpreter.RegisterFunctions();

            // load resources
            BGMPlayer.instance.bgm = Resources.LoadAll<AudioClip>("audio/bgm");
            PrefabManager.instance.prefabs = Resources.LoadAll<GameObject>("prefabs");

            // then go to title screen
            SceneManager.LoadScene("title");
        }
    }

}