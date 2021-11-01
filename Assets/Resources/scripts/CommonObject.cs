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
        private void Awake()
        {
            if (!Global.instance.commonObjects.ContainsKey(gameObject.name))
                Global.instance.commonObjects.Add(gameObject.name, gameObject);
        }
    }
}
