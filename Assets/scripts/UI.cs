using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace spacegame
{
    public class UI : MonoBehaviour
    {
        public Text text;

        public void Initialize(Vector2 size)
        {
            text = GetComponentInChildren<Text>();

            // set box scale
            RectTransform rect = GetComponent<RectTransform>();
            
            rect.sizeDelta = size;

            // set text position and scale
            //text.transform.localPosition = new Vector2(57, -7);
            Image image = GetComponent<Image>();
            RectTransform textRect = text.GetComponent<RectTransform>();
            textRect.localPosition = new Vector2(0, 0);
        }
    }
}
