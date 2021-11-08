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
    }

    private void Update()
    {
        if (!_treasure) return;
        _leftAmount.text = _treasure.currentAmount.ToString();
    }
}
