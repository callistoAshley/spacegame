using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace spacegame
{
    public class TitleScreen : MonoBehaviour
    {
        private void Start()
        {
            CreateMainMenu();
        }

        private void CreateMainMenu()
        {
            UINavigateable ui = UIManager.instance.NewNavigateable(new Vector2(0, -140), new Vector2(400, 100));
            ui.SetOptions(new string[] { "play game", "don't play game", "fabjsdgfjk", "hi" },
                new Action(() => Input(ui.selectedOption)));
        }

        private void Input(string selectedOption)
        {
            Debug.Log(selectedOption);
        }
    }
}
