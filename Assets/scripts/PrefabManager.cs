using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager instance;
    public List<GameObject> prefabs;

    public GameObject GetPrefab(string name)
    {
        // y'all like python indenting in c#
        foreach (GameObject g in prefabs)
            if (g.name == name)
                return g;
        throw new Exception($"PrefabManager prefabs list does not have a prefab called {name}");
    }
}
