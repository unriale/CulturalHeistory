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


    public void ThrowFrom(Vector3 hand, Vector3 aim, Transform player, Animator animator)
    {
        if (amount <= 0) return;
        animator.SetBool("HoldingThrow", false);
        StartCoroutine(ThrowWithDelay(hand, aim, player));
    }

    IEnumerator ThrowWithDelay(Vector3 hand, Vector3 aim, Transform player)
    {
        yield return new WaitForSeconds(0.45f);
        GameObject coin = Instantiate(coinPrefab, hand, coinPrefab.transform.rotation);
        _rigidBody = coin.GetComponent<Rigidbody>();
        Vector3 towards = aim - player.position;
        _rigidBody.AddForce(new Vector3(throwForce * towards.x, 0, throwForce * towards.z), ForceMode.Acceleration);
        ReduceCoinsAmount();
        OnAmountChanged();
    }

    public static void ReloadCoins() => amount = 3;

    private void ReduceCoinsAmount() => --amount;

    public int GetCoinsAmount() => amount;
}
