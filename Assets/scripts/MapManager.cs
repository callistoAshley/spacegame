using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace spacegame
{
    public static class MapManager
    {
        // this string is set to the name of the scene that is being changed to while ChangeMap is being called
        // to prevent changing the map twice, which skibbadoodles the player instance
        private static string changingName;

        public static void ChangeMap(int id, int transferPoint = 0)
            => ChangeMap(SceneManager.GetSceneByBuildIndex(id).name, transferPoint);

        public static void ChangeMap(string name, int transferPoint = 0)
        {
            if (changingName == name) return;
            changingName = name;

            Logger.WriteLine($"changing map: {name}, transferPoint {transferPoint}");

            Player.instance?.ToggleMovementHooks(false);
            InputManager.instance.RemoveEvent(Constants.Input.VERTICAL_KEY_DOWN, UIManager.instance.ProcessInputQueue);
            InputManager.instance.RemoveEvent(Constants.Input.SELECT_KEY_DOWN, UIManager.instance.ProcessInputQueue);

            // load the scene
            AsyncOperation a = SceneManager.LoadSceneAsync(name);
            a.completed += new Action<AsyncOperation>((x) => 
            {
                // call map data init as callback
                MapData.map.Init(transferPoint);
                // and set changingName back to an empty string 
                changingName = string.Empty;
            });

            // movement hooks are re-added in Controller.Awake
        }
    }
}
