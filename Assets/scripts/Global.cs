using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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

            // initialize other stuff
            UIManager.SetInstance();
            alisonscript.Interpreter.RegisterFunctions();

            // textbox
            UIManager.instance.New(new Vector2(-52, 169), new Vector2(700, 200));
            // speaker box
            UIManager.instance.New(new Vector2(-52, 328), new Vector2(500, 100));
            // alisonscript.Interpreter.Run("");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
