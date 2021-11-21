using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Updater : MonoBehaviour
{
    [SerializeField] List<Treasure> _treasures;

    private void Awake()
    {
        foreach (Treasure t in _treasures)
        {
            t.currentAmount = 0;
        }
        PlayerPrefs.DeleteAll();
    }

}
