using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsDisplay : MonoBehaviour
{
    Text amount;

    private void Awake()
    {
        amount = GetComponent<Text>();
    }

    private void OnEnable()
    {
        Coin.OnAmountChanged += ChangeText;
    }

    private void OnDisable()
    {
        Coin.OnAmountChanged -= ChangeText;
    }

    void ChangeText()
    {
        amount.text = FindObjectOfType<Coin>().GetCoinsAmount().ToString();
    }
}
