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
        public static void ChangeMap(int id, int transferPoint = 0)
            => ChangeMap(SceneManager.GetSceneByBuildIndex(id).name, transferPoint);

        public static void ChangeMap(string name, int transferPoint = 0)
        {
            // load the scene
            AsyncOperation a = SceneManager.LoadSceneAsync(name);
            a.completed += new Action<AsyncOperation>(
                // call map data init as callback
                x => MapData.map.Init(transferPoint));
            // do map init
            //MapData.map.Init(transferPoint);
        }
    }
}
