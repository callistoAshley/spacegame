using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace spacegame
{
    public class Global : MonoBehaviour
    {
        public static Global instance;
        public static bool initialized;

        // Start is called before the first frame update
        void Awake()
        {
            if (initialized) return;
            initialized = true;

            DontDestroyOnLoad(gameObject);
            instance = this;
            alisonscript.Interpreter.RegisterFunctions();
            // alisonscript.Interpreter.Run("");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
