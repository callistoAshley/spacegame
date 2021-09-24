using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace spacegame
{
    public class MainCamera : MonoBehaviour
    {
        public GameObject player;
        public bool followPlayer;

        public static MainCamera instance;

        // Start is called before the first frame update
        void Start()
        {
            instance = this;
            if (player == null) player = GameObject.Find("alison");
        }

        // Update is called once per frame
        void Update()
        {
            if (player != null) transform.position = player.transform.position - new Vector3(0, 0, 10);
        }
    }
}
