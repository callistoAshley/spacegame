using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace spacegame
{
    public static class InGameMenuManager 
    {
        private static bool open;
        private static UINavigateable ui;

        public static void Open()
        {
            // don't open the menu if it's already open
            if (open) return;
            open = !open;

            // create ui
            ui = UIManager.instance.NewNavigateable(new Vector2(-328, -4), new Vector2(292, 100),
                customPrintTextOptions: UI.PrintTextOptions.CallbackAfterInput | UI.PrintTextOptions.Instant);
            // set options
            ui.SetOptions(new string[]
            {
                "back",
                "save",
                "settings",
                "inventory",
                "title"
            }, new Action(() => ProcessMenuButton(ui.selectedOption)));
        }

        public static void Close()
        {
            Debug.Log("closing");
            if (!open) return;
            open = !open;

            // destroy ui
            ui.DestroyGameObject();
            ui = null;

            // allow movement
            Controller.instance.canMove = true;
        }

        public static void ProcessMenuButton(string button)
        {
            switch (button)
            {
                case "back":
                    Close();
                    break;
                case "save":
                    break;
                case "settings":
                    break;
                case "inventory":
                    break;
                // return to title screen
                case "title":
                    UINavigateable options = null;
                    UI textbox = null;

                    // create callback to printing the confirmation text as creating the ui navigateable
                    Action createOptions = new Action(() =>
                    {
                        // create navigateable
                        options = UIManager.instance.NewNavigateable(new Vector2(92, -134), new Vector2(485, 100), 
                            UI.PrintTextOptions.Instant | UI.PrintTextOptions.DestroyUIAfterCallback | UI.PrintTextOptions.CallbackAfterInput);

                        // also destroy the textbox after the callback
                        options.alsoDestroy.Add(textbox);

                        // set the options
                        options.SetOptions(new string[]
                        {
                            "no",
                            "yes",
                        },
                        // callback
                        new Action(() => 
                        {
                            Close();

                            switch (options.selectedOption)
                            {
                                case "no": // do nothing
                                    // destroy ui after callback isn't working here for some reason so i'll just do this
                                    options.DestroyGameObject();
                                    break;
                                case "yes":
                                    MapManager.ChangeMap("title");
                                    break;
                            }
                        }));
                    });

                    textbox = UIManager.instance.New(new Vector2(92, 94), new Vector2(484, 257));
                    
                    textbox.StartCoroutine(textbox.PrintText(
                        $"are you sure you want to return to the title screen? (last saved format this seconds ago)",
                        createOptions, UI.PrintTextOptions.CallbackAfterPrinting | UI.PrintTextOptions.DontPushToInputQueue));

                    break;
            }
        }
    }
}
