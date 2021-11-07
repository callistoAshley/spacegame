using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace spacegame
{
    public class DebugUI : MonoBehaviour
    {
        public GameObject debugConsole;
        public Text debugText;

        // Update is called once per frame
        void Update()
        {
            // toggle on/off debug console
            if (Input.GetKeyDown(KeyCode.Tab))
                debugConsole.SetActive(!debugConsole.activeSelf);

            // set debug text
            debugText.text 
                = $"scene: {SceneManager.GetActiveScene().name ?? "<none, somehow>"}\n"
                + $"player world position: {(Controller.instance != null ? Controller.instance.gameObject.transform.position.ToString() : "<none>")}\n"
                + $"bgm: {(BGMPlayer.instance.aud.clip.name != null ? BGMPlayer.instance.aud.clip.name : "<none>")}\n"
                + $"interpreter line: {(alisonscript.Interpreter.runningScript != null ? alisonscript.Interpreter.runningScript.lines[alisonscript.Interpreter.runningScript.lineIndex] : "<none>")}\n"
                + $"cool robot: yup";
        }
    }
}