using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

namespace spacegame
{
    public static class InventoryManager 
    {
        public static List<ItemData> itemData = new List<ItemData>();

        public static void InitItems()
        {
            foreach (string file in Directory.GetDirectories(Application.streamingAssetsPath + "\\items\\"))
            {
                ItemData i = JsonConvert.DeserializeObject<ItemData>(File.ReadAllText(file));
                itemData.Add(i);
            }
        }

        // don't use this, just use the python script in dev scripts instead
        public static void GenerateItemDataSkeleton(string output)
        {
            string whatsit = JsonConvert.SerializeObject(new ItemData());
            File.WriteAllText(output, whatsit);
        }
    }
}
