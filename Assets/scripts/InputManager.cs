#pragma warning disable CS0067 // shut up. huhuhptsu . shut pua. shut pu. shut up. shut the lhjaks. shut up. please.l shut i[
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
        public event KeyPressedEventHandler horizontalKeyHeld;
        public event KeyPressedEventHandler horizontalKeyDown;
        public event KeyPressedEventHandler horizontalKeyReleased;
        public event KeyPressedEventHandler fixedHorizontalKeyHeld;

        public event KeyPressedEventHandler verticalKeyHeld;
        public event KeyPressedEventHandler verticalKeyDown;
        public event KeyPressedEventHandler verticalKeyReleased;
        public event KeyPressedEventHandler fixedVerticalKeyHeld;

        public event KeyPressedEventHandler selectKeyHeld;
        public event KeyPressedEventHandler selectKeyDown;
        public event KeyPressedEventHandler selectKeyReleased;

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

        // i hate looking at this i am so sorry
        void Update()
        {
            if (Input.GetKey(left)) horizontalKeyHeld?.Invoke(this, new KeyPressedEventArgs(left));
            if (Input.GetKey(right)) horizontalKeyHeld?.Invoke(this, new KeyPressedEventArgs(right));
            if (Input.GetKeyDown(left)) horizontalKeyDown?.Invoke(this, new KeyPressedEventArgs(left));
            if (Input.GetKeyDown(right)) horizontalKeyDown?.Invoke(this, new KeyPressedEventArgs(right));
            if (Input.GetKeyUp(left)) horizontalKeyReleased?.Invoke(this, new KeyPressedEventArgs(left));
            if (Input.GetKeyUp(right)) horizontalKeyReleased?.Invoke(this, new KeyPressedEventArgs(right));

            if (Input.GetKey(up)) verticalKeyHeld?.Invoke(this, new KeyPressedEventArgs(up));
            if (Input.GetKey(down)) verticalKeyHeld?.Invoke(this, new KeyPressedEventArgs(down));
            if (Input.GetKeyDown(up)) verticalKeyDown?.Invoke(this, new KeyPressedEventArgs(up));
            if (Input.GetKeyDown(down)) verticalKeyDown?.Invoke(this, new KeyPressedEventArgs(down));
            if (Input.GetKeyUp(up)) verticalKeyReleased?.Invoke(this, new KeyPressedEventArgs(up));
            if (Input.GetKeyUp(down)) verticalKeyReleased?.Invoke(this, new KeyPressedEventArgs(down));

            if (Input.GetKey(select)) selectKeyHeld?.Invoke(this, new KeyPressedEventArgs(select));
            if (Input.GetKeyDown(select)) selectKeyDown?.Invoke(this, new KeyPressedEventArgs(select));
            if (Input.GetKeyUp(select)) selectKeyReleased?.Invoke(this, new KeyPressedEventArgs(select));
        }

        private void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.LeftArrow)) fixedHorizontalKeyHeld?.Invoke(this, new KeyPressedEventArgs(KeyCode.LeftArrow));
            if (Input.GetKey(KeyCode.RightArrow)) fixedHorizontalKeyHeld?.Invoke(this, new KeyPressedEventArgs(KeyCode.RightArrow));

            if (Input.GetKey(KeyCode.UpArrow)) fixedVerticalKeyHeld?.Invoke(this, new KeyPressedEventArgs(KeyCode.UpArrow));
            if (Input.GetKey(KeyCode.DownArrow)) fixedVerticalKeyHeld?.Invoke(this, new KeyPressedEventArgs(KeyCode.DownArrow));
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
#pragma warning restore CS0067