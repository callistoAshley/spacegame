#pragma warning disable CS0067 // shut up. huhuhptsu . shut pua. shut pu. shut up. shut the lhjaks. shut up. please.l shut i[
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace spacegame
{
    public class InputManager : MonoBehaviour
    {
        // definitions
        public delegate void KeyPressedEventHandler(KeyPressedEventArgs e);
        public enum KeyPressedType
        {
            Down,
            Held,
            Up
        } 

        // key pressed events

        // key codes
        public static KeyCode left = KeyCode.LeftArrow;
        public static KeyCode right = KeyCode.RightArrow;
        public static KeyCode up = KeyCode.UpArrow;
        public static KeyCode down = KeyCode.DownArrow;
        public static KeyCode select = KeyCode.Z;

        // singleton instance
        public static InputManager instance;

        // TODO: comment this

        // array of key events that occur in non-fixed update
        private KeyPressedEvent[] events = new KeyPressedEvent[]
        {
            // horizontal keys
            new KeyPressedEvent(left, KeyPressedType.Down, new DelegateHolder("horizontalKeyDown")),
            new KeyPressedEvent(right, KeyPressedType.Down, new DelegateHolder("horizontalKeyDown")),
            new KeyPressedEvent(left, KeyPressedType.Held, new DelegateHolder("horizontalKeyHeld")),
            new KeyPressedEvent(right, KeyPressedType.Held, new DelegateHolder("horizontalKeyHeld")),
            new KeyPressedEvent(left, KeyPressedType.Up, new DelegateHolder("horizontalKeyReleased")),
            new KeyPressedEvent(right, KeyPressedType.Up, new DelegateHolder("horizontalKeyReleased")),

            // vertical keys
            new KeyPressedEvent(up, KeyPressedType.Down, new DelegateHolder("verticalKeyDown")),
            new KeyPressedEvent(down, KeyPressedType.Down, new DelegateHolder("verticalKeyDown")),
            new KeyPressedEvent(up, KeyPressedType.Held, new DelegateHolder("verticalKeyHeld")),
            new KeyPressedEvent(down, KeyPressedType.Held, new DelegateHolder("verticalKeyHeld")),
            new KeyPressedEvent(up, KeyPressedType.Up, new DelegateHolder("verticalKeyReleased")),
            new KeyPressedEvent(down, KeyPressedType.Up, new DelegateHolder("verticalKeyReleased")),

            // select
            new KeyPressedEvent(select, KeyPressedType.Down, new DelegateHolder("selectKeyDown"), 0.1f), // have a delay of 0.1 seconds for the select key
            new KeyPressedEvent(select, KeyPressedType.Held, new DelegateHolder("selectKeyHeld")),
            new KeyPressedEvent(select, KeyPressedType.Up, new DelegateHolder("selectKeyReleased")),

            // fixed update events go in fixed update in massive if chains
        };

        public static KeyPressedEventHandler fixedHorizontalKeyHeld;
        public static KeyPressedEventHandler fixedVerticalKeyHeld;

        public void AddEvent(string handlerName, KeyPressedEventHandler delegateInput)
        {
            foreach (KeyPressedEvent e in events)
            {
                if (e.eventHandler.name == handlerName)
                {
                    e.eventHandler.Add(delegateInput);
                }
            }
        }

        public void RemoveEvent(string handlerName, KeyPressedEventHandler delegateInput)
        {
            foreach (KeyPressedEvent e in events)
            {
                if (e.eventHandler.name == handlerName)
                {
                    e.eventHandler.Remove(delegateInput);
                }
            }
        }

        private void Awake()
        {
            instance = this;
        }

        // TODO: comment this 
        private bool dontProcessQueue;
        private IEnumerator ProcessEventsLoop()
        {
            foreach (KeyPressedEvent e in events)
            {
                bool pressed = false;

                switch (e.pressedType)
                {
                    case KeyPressedType.Down:
                        pressed = Input.GetKeyDown(e.key);
                        break;
                    case KeyPressedType.Held:
                        pressed = Input.GetKey(e.key);
                        break;
                    case KeyPressedType.Up:
                        pressed = Input.GetKeyUp(e.key);
                        break;
                }

                if (pressed)
                {
                    e.eventHandler.Invoke(e.key); // i will spoon my eyes out
                    if (e.eventHandler.handler != null && e.delay > 0)
                    {
                        dontProcessQueue = true;
                        yield return new WaitForSeconds(e.delay);
                        dontProcessQueue = false;
                    }
                }
            }
        }

        private void Update()
        {
            if (!dontProcessQueue)
            {
                StartCoroutine(ProcessEventsLoop());
            }
        }

        private void FixedUpdate()
        {
            if (Input.GetKey(left)) fixedHorizontalKeyHeld?.Invoke(new KeyPressedEventArgs(left));
            if (Input.GetKey(right)) fixedHorizontalKeyHeld?.Invoke(new KeyPressedEventArgs(right));

            if (Input.GetKey(up)) fixedVerticalKeyHeld?.Invoke(new KeyPressedEventArgs(up));
            if (Input.GetKey(down)) fixedVerticalKeyHeld?.Invoke(new KeyPressedEventArgs(down));
        }

        public class KeyPressedEventArgs : EventArgs
        {
            public KeyCode key;

            public KeyPressedEventArgs(KeyCode key)
            {
                this.key = key;
            }
        }

        public class KeyPressedEvent
        {
            public KeyCode key;
            public KeyPressedType pressedType;
            public DelegateHolder eventHandler;
            public float delay;

            public KeyPressedEvent(KeyCode key, KeyPressedType pressedType, DelegateHolder eventHandler, float delay = 0)
            {
                this.key = key;
                this.pressedType = pressedType;
                this.eventHandler = eventHandler;
                this.delay = delay;
            }
        }

        public class DelegateHolder
        {
            public string name;
            public KeyPressedEventHandler handler;

            public DelegateHolder(string name)
            {
                this.name = name;
            }

            public void Add(KeyPressedEventHandler input)
            {
                handler += input;
            }

            public void Remove(KeyPressedEventHandler input)
            {
                handler -= input;
            }

            public void Invoke(KeyCode k)
            {
                handler?.Invoke(new KeyPressedEventArgs(k));
            }
        }
    }
}
#pragma warning restore CS0067
