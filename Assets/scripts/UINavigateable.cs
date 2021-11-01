using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace spacegame
{
    public class UINavigateable : UI
    {
        public string[] options;
        public int index; // the index position of the currently selected option
        private GameObject arrow;

        public string selectedOption
        {
            get
            {
                return options[index];
            }
        }

        // remember to create new ui options in UIManager.New or UIManager.NewNavigateable
        public void SetOptions(string[] options, Action callback)
        {
            // references
            RectTransform rect = GetComponent<RectTransform>();
            GameObject canvas = Global.instance.GetCommonObject("Canvas");
            Vector2 currentSize = rect.sizeDelta;
            GameObject arrow = PrefabManager.instance.GetPrefab("arrow");
            Text text = GetComponentInChildren<Text>();

            this.options = options;
            // resize box to fit options
            Initialize(new Vector2(currentSize.x, currentSize.y + (35 * options.Length))); // for each option, add 50 to the y scale

            // print text (this also adds it to the input queue)
            string optionsPrint = string.Join("\n", options);
            StartCoroutine(PrintText(optionsPrint, callback, 
                PrintTextOptions.CallbackAfterInput | PrintTextOptions.Instant | PrintTextOptions.DestroyUIAfterCallback));

            // create arrow
            Vector2 arrowPosition = new Vector2(rect.rect.xMin + 10, 
                rect.rect.yMax - (rect.rect.yMax - text.rectTransform.rect.yMax) - 15);
            this.arrow = Instantiate(arrow, new Vector2(arrowPosition.x,/* 25 * (options.Length - 1)*/arrowPosition.y) + (Vector2)gameObject.transform.position, 
                Quaternion.identity, gameObject.transform);
        }

        // returns the option the arrow lands on
        public void Navigate(bool up)
        {
            // increment/decrement index
            // the top option has the lowest index and the lowest option has the highest index
            if (up && index > 0)
            {
                index--;
                arrow.transform.position += new Vector3(0, 35);
            }
            else if (!up && index < options.Length - 1)
            {
                index++;
                arrow.transform.position -= new Vector3(0, 35);
            }
        }
    }
}
