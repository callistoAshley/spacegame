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

        // Start is called before the first frame update
        void Awake()
        {
            // singleton stuff
            DontDestroyOnLoad(gameObject);
            instance = this;
            SceneManager.sceneLoaded += SceneLoaded;
            
            Init.Initialization();
        }

        // this runs before common object
        private void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            CommonObject.commonObjects.Clear();
        }
    }
}
