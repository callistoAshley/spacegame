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

        public void Initialize(Vector2 position, Vector2 size)
        {
            text = GetComponentInChildren<Text>();

            // set box scale
            gameObject.transform.position = position;
            gameObject.transform.localScale = size;

            // set text position and scale
            text.transform.localPosition = new Vector2(57, -7);
            text.gameObject.transform.localScale = size; 
        }
    }
}
