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

        // assign these in inspector
        public Parallax.ParallaxData[] parallaxes = new Parallax.ParallaxData[0];
        // these are the parallax game objects
        [HideInInspector] public List<Parallax> parallaxObjects = new List<Parallax>();

        private void Awake()
        {
            map = this;

            // create parallaxes
            GameObject parallax = PrefabManager.instance.GetPrefab("parallax object");
            for (int i = 0; i < parallaxes?.Length; i++)
            {
                // instantiate the prefab at the origin point
                GameObject inst = Instantiate(parallax, Vector3.zero, Quaternion.identity);

                // set the sorting layer
                inst.GetComponent<SpriteRenderer>().sortingLayerName = $"parallax{i}";

                // set the data and add the parallax to the parallaxObjects list
                Parallax p = inst.GetComponent<Parallax>();
                p.data = parallaxes[i];
                p.moveSpeedMultiplier = (i + 1) * 0.1f;
                parallaxObjects.Add(p);

                // call init
                p.Init();
            }
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
