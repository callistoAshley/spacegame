using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace spacegame
{
    public class SFXPlayer : MonoBehaviour
    {
        private AudioSource aud;
        public static SFXPlayer instance;

        public AudioClip[] sfx;

        // Start is called before the first frame update
        void Awake()
        {
            aud = GetComponent<AudioSource>();
            instance = this;
        }

        public void Play(string name)
        {
            foreach (AudioClip a in sfx)
            {
                if (a.name == name)
                {
                    aud.PlayOneShot(a);
                    return;
                }
            }
            throw new Exception($"no AudioClip called {name} in sfx array");
        }
    }
}
