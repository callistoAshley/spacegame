using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PrefabManager 
{
    public static GameObject[] prefabs;

    [RuntimeInitializeOnLoadMethod]
    public static void RegisterPrefabs()
    {
        prefabs = Resources.LoadAll<GameObject>("prefabs/");
    }

    public static GameObject GetPrefab(string name)
    {
        // y'all like python indenting in c#
        foreach (GameObject g in prefabs)
            if (g.name == name)
                return g;
        throw new Exception($"PrefabManager prefabs list does not have a prefab called {name}");
    }
}
