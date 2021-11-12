using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;

    private int amount = 3;


    public void Throw()
    {
        Vector3 pos = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
        GameObject coin = Instantiate(coinPrefab, pos, Quaternion.identity);
    }
}
