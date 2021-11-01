using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace spacegame.alisonscript
{ 
    public class Object
    {
        public string value;
        public object reference; // an alisonscript object can be a reference to a c# object

        public bool isReference
        {
            get
            {
                return reference != null;
            }
        }

        public Object(string value)
        {
            this.value = value;
        }

        public Object(object reference)
        {
            this.reference = reference;
            value = string.Empty;
        }
    }
}