using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace spacegame
{
    public class BGMPlayer : MonoBehaviour
    {
        private AudioSource aud;
        public static BGMPlayer instance;

        public AudioClip[] bgm;

        // Start is called before the first frame update
        void Start()
        {
            aud = GetComponent<AudioSource>();
            instance = this;
        }

        public void Play(string name)
        {
            foreach (AudioClip a in bgm)
            {
                if (a.name == name)
                {
                    aud.clip = a;
                    aud.Play();
                }
            }
        }

        public void Stop()
        {
            aud.Stop();
        }
    }
}