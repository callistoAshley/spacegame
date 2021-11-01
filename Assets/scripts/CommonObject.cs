using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace spacegame
{
    // these game objects add themselves to a list in Global in Awake
    // add this to stuff like the canvas etc
    // for quick getter-ering
    public class CommonObject : MonoBehaviour
    {
        public static Dictionary<string, GameObject> commonObjects = new Dictionary<string, GameObject>();
        private void Start()
        {
            if (!commonObjects.ContainsKey(gameObject.name))
                commonObjects.Add(gameObject.name, gameObject);
        }

        public static GameObject GetCommonObject(string name)
        {
            foreach (string s in commonObjects.Keys)
                if (s == name)
                    return commonObjects[s];
            throw new System.Exception($"the common objects dictionary does not have a key called {name}");
        }
    }
}
