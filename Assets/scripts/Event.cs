using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace spacegame
{
    [Serializable]
    public class Event 
    {
        public enum EventType
        {
            RunScript,
            ChangeMap,
            PlaySfx,
        }

        public EventType eventType; // the type of event
        public string[] args; // arguments

        public Event(EventType eventType, params string[] args)
        {
            this.eventType = eventType;
            this.args = args;
        }
    }
}
