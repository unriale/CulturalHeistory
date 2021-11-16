using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float throwForce = 10f;

    private Rigidbody _rigidBody;
    
    private static int amount = 3;

    public static event Action OnAmountChanged; 


    public void ThrowFrom(Vector3 hand, Vector3 aim, Transform player)
    {
        // TODO: instantiate from the hand once we have an animation
        if (amount <= 0) return;
        GameObject coin = Instantiate(coinPrefab, hand, coinPrefab.transform.rotation);
        _rigidBody = coin.GetComponent<Rigidbody>();
        Vector3 towards = aim - player.position;
        _rigidBody.AddForce(new Vector3(throwForce * towards.x, 0, throwForce * towards.z), ForceMode.Acceleration);
        ReduceCoinsAmount();
        OnAmountChanged();
    }

    private void ReduceCoinsAmount() => --amount;

    public int GetCoinsAmount() => amount;
}
