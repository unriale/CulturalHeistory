using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] Treasure _treasure;

    private Text _leftAmount;

    private void Awake()
    {
        _leftAmount = transform.GetChild(1).GetComponent<Text>();
        SetInitialTextAmount();
    }

    public void UpdateAmount(int amount, float delay)
    {
        StartCoroutine(DelayUpdate(amount, delay));
    }

    IEnumerator DelayUpdate(int amount, float delay)
    {
        if (!_treasure) yield return null;
        yield return new WaitForSeconds(delay);
        _leftAmount.text = amount.ToString();
        ActivateSlot();
    }

    private void ActivateSlot()
    {
        transform.GetChild(0).GetComponent<Image>().color = Color.white;
    }

    public void SetInitialTextAmount()
    {
        if (!_treasure)
        {
            print("NOT A TRESURE");
            return;
        }
        print(_treasure.maxAmount.ToString());

        _leftAmount.text = _treasure.maxAmount.ToString();
    }
}
