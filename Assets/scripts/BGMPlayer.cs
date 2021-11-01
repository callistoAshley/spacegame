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
        void Awake()
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
                    return;
                }
            }
            throw new System.Exception($"no AudioClip called {name} in bgm array");
        }

        public void Stop()
        {
            aud.Stop();
        }
    }
}