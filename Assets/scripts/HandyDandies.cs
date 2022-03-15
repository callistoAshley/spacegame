using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace spacegame
{
    public class HandyDandies : MonoBehaviour
    {
        public static HandyDandies instance;

        private void Awake()
        {
            instance = this;
        }

        public IEnumerator DoAfter(Func<bool> predicate, Action callback)
        {
            yield return new WaitUntil(predicate);
            callback.Invoke();
        }

        public IEnumerator DoAfterSeconds(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback.Invoke();
        }

        // shoutout to gls on the modshot discord for looking at my code and going "bruh"
        public string GetElapsedTimeString(TimeSpan time)
        {
            int[] times = new int[]
            {
                time.Hours,
                time.Minutes,
                time.Seconds
            };
            string[] strings = new string[]
            {
                "hour",
                "minute",
                "second"
            };

            string ret = string.Empty;
            for (int i = 0; i < times.Length; i++)
            {
                if (times[i] == 0)
                    continue;

                ret += $"{times[i]} {strings[i]}";
                ret += times[i] > 1 ? "s" : "";
            }

            return ret;
        }
    }
}
