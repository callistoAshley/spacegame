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
    }
}
