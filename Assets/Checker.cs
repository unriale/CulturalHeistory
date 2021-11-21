using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    [SerializeField] string id;

    private void Awake()
    {
        if (PlayerPrefs.GetInt(id) == 1)
        {
            Destroy(gameObject);
        }
    }
}
