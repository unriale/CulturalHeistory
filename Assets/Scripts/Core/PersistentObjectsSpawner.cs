using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObjectsSpawner : MonoBehaviour
{
    [SerializeField] GameObject persistentObjects;
    public static GameObject persistentGameObjects;

    static bool hasSpawned = false;

    private void Awake()
    {
        if (hasSpawned) return;
        SpawnPersistentObjects();
        hasSpawned = true;
    }


    private void SpawnPersistentObjects()
    {
        persistentGameObjects = Instantiate(persistentObjects);
        DontDestroyOnLoad(persistentGameObjects);
    }

    public void DestroyPersistentGameObjects()
    {
        hasSpawned = false;
        Destroy(persistentGameObjects);
    }
}
