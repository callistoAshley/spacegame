using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace spacegame
{
    // REMEMBER THIS IS ATTACHED TO THE CANVAS PREFAB
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;
        // ui input queue

        void Awake()
        {
            instance = this;
        }
    }
}