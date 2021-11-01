using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace spacegame
{
    public class MapData : MonoBehaviour
    {
        public string autoBgm;

        private void Start()
        {
            if (!string.IsNullOrEmpty(autoBgm) && !string.IsNullOrWhiteSpace(autoBgm))
                BGMPlayer.instance.Play(autoBgm);
        }
    }
}
