using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [SerializeField] List<Treasure> _treasures;
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject fader;
    [SerializeField] GameObject coinsUI;
    [SerializeField] GameObject treasuresUI;

    static int allTreasures = 0;

    private void Awake()
    {
        foreach (Treasure t in _treasures)
        {
            allTreasures += t.maxAmount;
        }
        print("TREASURES amount to steal = " + allTreasures);
    }

    public void DecrementTreasures()
    {
        --allTreasures;
        print("Treasures count = " + allTreasures);
        if(allTreasures <= 0)
        {
            fader.SetActive(false);
            winScreen.SetActive(true);
            coinsUI.SetActive(false);
            treasuresUI.SetActive(false);
            print("WIIIIIIIIIIIIIIIIIIIN");
        }

       
    }


}
