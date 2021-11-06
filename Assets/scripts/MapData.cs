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
        public Vector2[] transferPoints;

        public static MapData map;

        private void Awake()
        {
            map = this;
        }

        public void Init(int transferPointIndex = 0)
        {
            if (!string.IsNullOrEmpty(autoBgm) && !string.IsNullOrWhiteSpace(autoBgm))
                BGMPlayer.instance.Play(autoBgm);

            if (transferPoints != null && transferPoints.Length > 0)
                Controller.instance.gameObject.transform.position = transferPoints[transferPointIndex];
        }
    }
}
