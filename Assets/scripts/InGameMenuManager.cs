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
            // if the menu is already open, close it
            if (open)
            {
                Close();
                return;
            }
            open = !open;

            // stop the player's horizontal movement animation and disallow movement
            Player.instance.StopHorizontalAnimation();
            Player.instance.canMove = false;

            // create ui
            ui = UIManager.instance.NewNavigateable(new Vector2(-328, -4), new Vector2(292, 100),
                customPrintTextOptions: UI.PrintTextOptions.CallbackAfterInput | UI.PrintTextOptions.Instant);
            // set options
            ui.SetOptions(new string[]
            {
                "back",
                //"inventory",
                "settings",
                "save",
                "title"
            }, new Action(() => ProcessMenuButton(ui.selectedOption)));

            // play sound
            SFXPlayer.instance.Play("sfx_menu_confirm");
        }

        public static void Close()
        {
            open = false;

            // destroy ui
            ui.DestroyGameObject();
            ui.readyToDequeue = true;
            ui = null;

            // allow movement
            Player.instance.canMove = true;

            // play sound
            SFXPlayer.instance.Play("sfx_menu_back");
        }

        public static void ProcessMenuButton(string button)
        {
            switch (button)
            {
                case "back":
                    Close();
                    SFXPlayer.instance.Play("sfx_menu_back");
                    break;
                case "settings":
                    break;
                case "save":
                    break;
                // return to title screen
                case "title":
                    UINavigateable options = null;
                    UI textbox = null;

                    // create callback to printing the confirmation text as creating the ui navigateable
                    Action createOptions = new Action(() =>
                    {
                        // create navigateable
                        options = UIManager.instance.NewNavigateable(new Vector2(92, -134), new Vector2(485, 50), 
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
                        () => 
                        {
                            switch (options.selectedOption)
                            {
                                case "no": // do nothing
                                    // destroy ui after callback isn't working here for some reason so i'll just do this
                                    options.DestroyGameObject();
                                    SFXPlayer.instance.Play("sfx_menu_back");
                                    break;
                                case "yes":
                                    Close();
                                    MapManager.ChangeMap("title");
                                    break;
                            }
                        });
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
