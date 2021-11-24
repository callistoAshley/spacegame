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
        public static KeyPressedEventHandler horizontalKeyHeld;
        public static KeyPressedEventHandler horizontalKeyDown;
        public static KeyPressedEventHandler horizontalKeyReleased;
        public static KeyPressedEventHandler fixedHorizontalKeyHeld;

        public static KeyPressedEventHandler verticalKeyHeld;
        public static KeyPressedEventHandler verticalKeyDown;
        public static KeyPressedEventHandler verticalKeyReleased;
        public static KeyPressedEventHandler fixedVerticalKeyHeld;

        public static KeyPressedEventHandler selectKeyHeld;
        public static KeyPressedEventHandler selectKeyDown;
        public static KeyPressedEventHandler selectKeyReleased;

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
            new KeyPressedEvent(left, KeyPressedType.Held, new DelegateHolder("horizontalKeyHeld")),
            new KeyPressedEvent(right, KeyPressedType.Held, new DelegateHolder("horizontalKeyHeld")),
            new KeyPressedEvent(left, KeyPressedType.Down, new DelegateHolder("horizontalKeyDown")),
            new KeyPressedEvent(right, KeyPressedType.Down, new DelegateHolder("horizontalKeyDown")),
            new KeyPressedEvent(left, KeyPressedType.Up, new DelegateHolder("horizontalKeyReleased")),
            new KeyPressedEvent(right, KeyPressedType.Up, new DelegateHolder("horizontalKeyReleased")),

            // vertical keys
            new KeyPressedEvent(up, KeyPressedType.Held, new DelegateHolder("verticalKeyHeld")),
            new KeyPressedEvent(down, KeyPressedType.Held, new DelegateHolder("verticalKeyHeld")),
            new KeyPressedEvent(up, KeyPressedType.Down, new DelegateHolder("verticalKeyDown")),
            new KeyPressedEvent(down, KeyPressedType.Down, new DelegateHolder("verticalKeyDown")),
            new KeyPressedEvent(up, KeyPressedType.Up, new DelegateHolder("verticalKeyReleased")),
            new KeyPressedEvent(down, KeyPressedType.Up, new DelegateHolder("verticalKeyReleased")),

            // select
            new KeyPressedEvent(select, KeyPressedType.Down, new DelegateHolder("selectKeyDown"), 0.1f), // have a delay of 0.1 seconds for the select key
            new KeyPressedEvent(select, KeyPressedType.Held, new DelegateHolder("selectKeyHeld")),
            new KeyPressedEvent(select, KeyPressedType.Up, new DelegateHolder("selectKeyReleased")),

            // fixed update events go in fixed update in massive if chains
        };

        public void AddEvent(string handlerName, KeyPressedEventHandler delegateInput)
        {
            foreach (KeyPressedEvent e in events)
            {
                if (e.eventHandler.name == handlerName)
                {
                    e.eventHandler.handler += delegateInput;
                    return;
                }
            }
            throw new Exception($"no KeyPressedEvent with an event with name {handlerName}");
        }

        public void RemoveEvent(string handlerName, KeyPressedEventHandler delegateInput)
        {
            foreach (KeyPressedEvent e in events)
            {
                if (e.eventHandler.name == handlerName)
                {
                    e.eventHandler.handler -= delegateInput;
                    return;
                }
            }
            throw new Exception($"no KeyPressedEvent with an event with name {handlerName}");
        }

        private void Awake()
        {
            instance = this;
            StartCoroutine(ProcessEventsLoop());
        }

        // TODO: comment this 
        private IEnumerator ProcessEventsLoop()
        {
            while (true)
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
                        e.eventHandler.handler?.Invoke(new KeyPressedEventArgs(e.key));
                        if (e.eventHandler.handler != null) yield return new WaitForSeconds(e.delay);
                    }
                }
                yield return new WaitForEndOfFrame();
            }
            throw new Exception("broke out of input manager processing loop");
        }

        private void FixedUpdate()
        {
            if (Input.GetKey(left)) fixedHorizontalKeyHeld?.Invoke(new KeyPressedEventArgs(KeyCode.LeftArrow));
            if (Input.GetKey(right)) fixedHorizontalKeyHeld?.Invoke(new KeyPressedEventArgs(KeyCode.RightArrow));

            if (Input.GetKey(up)) fixedVerticalKeyHeld?.Invoke(new KeyPressedEventArgs(KeyCode.UpArrow));
            if (Input.GetKey(down)) fixedVerticalKeyHeld?.Invoke(new KeyPressedEventArgs(KeyCode.DownArrow));
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
        }
    }
}
#pragma warning restore CS0067

/*
 #pragma warning disable CS0067 // shut up. huhuhptsu . shut pua. shut pu. shut up. shut the lhjaks. shut up. please.l shut i[
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace spacegame
{
    // TODO: revisit this before space!!!! goes open source
    public class InputManager : MonoBehaviour
    {
        // definitions
        public delegate void KeyPressedEventHandler(object sender, KeyPressedEventArgs e);
        public enum KeyPressedType
        {
            Down,
            Held,
            Up
        } 

        // key pressed events
        public static KeyPressedEventHandler horizontalKeyHeld;
        public static KeyPressedEventHandler horizontalKeyDown;
        public static KeyPressedEventHandler horizontalKeyReleased;
        public static KeyPressedEventHandler fixedHorizontalKeyHeld;

        public static KeyPressedEventHandler verticalKeyHeld;
        public static KeyPressedEventHandler verticalKeyDown;
        public static KeyPressedEventHandler verticalKeyReleased;
        public static KeyPressedEventHandler fixedVerticalKeyHeld;

        public static KeyPressedEventHandler selectKeyHeld;
        public static KeyPressedEventHandler selectKeyDown;
        public static KeyPressedEventHandler selectKeyReleased;

        private static Dictionary<string, KeyPressedEventHandler> doohickeys = new Dictionary<string, KeyPressedEventHandler>
        {
            // horizontal keys
            {nameof(horizontalKeyHeld), horizontalKeyHeld },
            {nameof(horizontalKeyDown), horizontalKeyDown },
            {nameof(horizontalKeyReleased), horizontalKeyReleased },
            {nameof(fixedHorizontalKeyHeld), fixedHorizontalKeyHeld },

            // vertical keys
            {nameof(verticalKeyHeld), verticalKeyHeld },
            {nameof(verticalKeyDown), verticalKeyDown },
            {nameof(verticalKeyReleased), verticalKeyReleased },
            {nameof(fixedVerticalKeyHeld), fixedVerticalKeyHeld },

            // select keys
            {nameof(selectKeyHeld), selectKeyHeld },
            {nameof(selectKeyDown), selectKeyDown },
            {nameof(selectKeyReleased), selectKeyReleased }
        };

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
            new KeyPressedEvent(left, KeyPressedType.Held, nameof(horizontalKeyHeld)),
            new KeyPressedEvent(right, KeyPressedType.Held, nameof(horizontalKeyHeld)),
            new KeyPressedEvent(left, KeyPressedType.Down, nameof(horizontalKeyDown)),
            new KeyPressedEvent(right, KeyPressedType.Down, nameof(horizontalKeyDown)),
            new KeyPressedEvent(left, KeyPressedType.Up, nameof(horizontalKeyReleased)),
            new KeyPressedEvent(right, KeyPressedType.Up, nameof(horizontalKeyReleased)),

            // vertical keys
            new KeyPressedEvent(up, KeyPressedType.Held, nameof(verticalKeyHeld)),
            new KeyPressedEvent(down, KeyPressedType.Held, nameof(verticalKeyHeld)),
            new KeyPressedEvent(up, KeyPressedType.Down, nameof(verticalKeyDown)),
            new KeyPressedEvent(down, KeyPressedType.Down, nameof(verticalKeyDown)),
            new KeyPressedEvent(up, KeyPressedType.Up, nameof(verticalKeyReleased)),
            new KeyPressedEvent(down, KeyPressedType.Up, nameof(verticalKeyReleased)),

            // select
            new KeyPressedEvent(select, KeyPressedType.Down, nameof(selectKeyDown), 0.1f), // have a delay of 0.1 seconds for the select key
            new KeyPressedEvent(select, KeyPressedType.Held, nameof(selectKeyHeld)),
            new KeyPressedEvent(select, KeyPressedType.Up, nameof(selectKeyReleased)),

            // fixed update events go in fixed update in massive if chains
        };

        private void Awake()
        {
            instance = this;
            StartCoroutine(ProcessEventsLoop());
        }

        // TODO: comment this 
        private IEnumerator ProcessEventsLoop()
        {
            while (true)
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
                        Debug.Log("pressed");
                        doohickeys[e.eventHandlerName]?.Invoke(this, new KeyPressedEventArgs(e.key));
                        yield return new WaitForSeconds(e.delay);
                    }
                }
                yield return new WaitForEndOfFrame();
            }
            throw new Exception("broke out of input manager processing loop");
        }

        private void FixedUpdate()
        {
            if (Input.GetKey(left)) fixedHorizontalKeyHeld?.Invoke(this, new KeyPressedEventArgs(left));
            if (Input.GetKey(right)) fixedHorizontalKeyHeld?.Invoke(this, new KeyPressedEventArgs(right));

            if (Input.GetKey(up)) fixedVerticalKeyHeld?.Invoke(this, new KeyPressedEventArgs(up));
            if (Input.GetKey(down)) fixedVerticalKeyHeld?.Invoke(this, new KeyPressedEventArgs(down));
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
            public string eventHandlerName;
            public float delay;

            public KeyPressedEvent(KeyCode key, KeyPressedType pressedType, string eventHandlerName, float delay = 0)
            {
                this.key = key;
                this.pressedType = pressedType;
                this.eventHandlerName = eventHandlerName;
                this.delay = delay;
            }
        }
    }
}
#pragma warning restore CS0067*/
