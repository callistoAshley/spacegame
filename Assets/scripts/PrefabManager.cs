using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager instance;
    public GameObject[] prefabs;

    private void Awake()
    {
        instance = this;
    }

    public GameObject GetPrefab(string name)
    {
        // y'all like python indenting in c#
        foreach (GameObject g in prefabs)
            if (g.name == name)
                return g;
        throw new Exception($"PrefabManager prefabs array does not have a prefab called {name}");
    }
}
