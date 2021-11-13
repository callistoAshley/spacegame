using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace spacegame
{
    public class InputManager : MonoBehaviour
    {
        public delegate void KeyPressedEventHandler(object sender, KeyPressedEventArgs e);

        // key pressed events
        public event KeyPressedEventHandler horizontalKeyPressed;
        public event KeyPressedEventHandler verticalKeyPressed;
        public event KeyPressedEventHandler selectKeyPressed;

        // key codes
        public KeyCode left = KeyCode.LeftArrow;
        public KeyCode right = KeyCode.RightArrow;
        public KeyCode up = KeyCode.UpArrow;
        public KeyCode down = KeyCode.DownArrow;
        public KeyCode select = KeyCode.Z;

        // singleton instance
        public static InputManager instance;

        private void Awake()
        {
            instance = this;
        }

        void Update()
        {
            // witty comment about how my code is bad
            if (Input.GetKey(KeyCode.LeftArrow)) horizontalKeyPressed?.Invoke(this, new KeyPressedEventArgs(KeyCode.A));
            if (Input.GetKey(KeyCode.RightArrow)) horizontalKeyPressed?.Invoke(this, new KeyPressedEventArgs(KeyCode.D));
            if (Input.GetKey(KeyCode.UpArrow)) verticalKeyPressed?.Invoke(this, new KeyPressedEventArgs(KeyCode.UpArrow));
            if (Input.GetKey(KeyCode.DownArrow)) verticalKeyPressed?.Invoke(this, new KeyPressedEventArgs(KeyCode.DownArrow));
            if (Input.GetKey(KeyCode.Z)) verticalKeyPressed?.Invoke(this, new KeyPressedEventArgs(KeyCode.Z));
        }

        public class KeyPressedEventArgs : EventArgs
        {
            public KeyCode key;

            public KeyPressedEventArgs(KeyCode key)
            {
                this.key = key;
            }
        }
    }
}