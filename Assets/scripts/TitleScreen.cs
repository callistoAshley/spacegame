using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            UINavigateable ui = UIManager.instance.NewNavigateable(new Vector2(0, -140), new Vector2(400, 100),
                UI.PrintTextOptions.CallbackAfterInput | UI.PrintTextOptions.Instant); // don't destroy the ui after callback
            ui.SetOptions(new string[] { "play game", "don't play game", "fabjsdgfjk", "hi" },
                new Action(() => Input(ui.selectedOption)));
        }

        private void Input(string selectedOption)
        {
            switch (selectedOption)
            {
                case "play game":
                    MapManager.ChangeMap("ship_alison_intro");
                    break;
                case "don't play game":
                    UI ui = UIManager.instance.New(new Vector2(0, -140), new Vector2(400, 200));
                    StartCoroutine(ui.PrintText("see ya later!", new Action(() => Application.Quit()),
                        UI.PrintTextOptions.CallbackAfterInput));
                    break;
            }
        }
    }
}
