using System;
using System.Collections.Generic;
using System.Collections;
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
            StartCoroutine(DebugCode());
        }

        private void CreateMainMenu()
        {
            UINavigateable ui = UIManager.instance.NewNavigateable(new Vector2(0, -140), new Vector2(400, 100),
                UI.PrintTextOptions.CallbackAfterInput | UI.PrintTextOptions.Instant); // don't destroy the ui after callback
            ui.SetOptions(new string[] { "play game", "don't play game", "fabjsdgfjk", "hi" },
                new Action(() => TitleInput(ui.selectedOption)));
        }

        private void TitleInput(string selectedOption)
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

        IEnumerator DebugCode()
        {
            // MOLLY
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.M));
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.O));
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.L));
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.L));
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Y));

            Global.instance.EnterDebugMode();

            UI ui = UIManager.instance.New(new Vector2(-220, -60), new Vector2(440, 240));
            StartCoroutine(ui.PrintText("wow it's debug mode", 
                options: UI.PrintTextOptions.DestroyUIAfterCallback));
        }
    }
}
