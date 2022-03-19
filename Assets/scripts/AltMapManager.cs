using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace spacegame
{
    public class AltMapManager : MonoBehaviour
    {
        public static AltMapManager instance;
        public bool altMapEnabled;

        private bool doingAnimation;

        private void Awake()
        {
            instance = this;
        }

        public void Toggle(bool instant = false)
        {
            if (doingAnimation) return;
            StartCoroutine(Animation(!altMapEnabled, instant));
        }

        // setting "instant" to true just skips the animation and the sound
        private IEnumerator Animation(bool enabled, bool instant = false)
        {
            doingAnimation = true && !instant;

            if (!instant)
                SFXPlayer.instance.Play(enabled ? "sfx_alt_activate" : "sfx_alt_deactivate");
            if (enabled)
            {
                while (MainCamera.instance.altMapMask.localScale.y <= 20)
                {
                    MainCamera.instance.altMapMask.localScale += new Vector3(0, 10 * Time.deltaTime, 0);
                    if (!instant) yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                while (MainCamera.instance.altMapMask.localScale.y >= 1)
                {
                    MainCamera.instance.altMapMask.localScale -= new Vector3(0, 10 * Time.deltaTime, 0);
                    if (!instant) yield return new WaitForEndOfFrame();
                }
            }
            if (!instant) doingAnimation = false;

            altMapEnabled = enabled;
        }
    }
}
