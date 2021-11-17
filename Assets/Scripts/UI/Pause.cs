using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToChangeActiveStatus;
    private GameObject treasuresUI;

    private void Awake()
    {
        treasuresUI = GameObject.FindGameObjectWithTag("TreasuresUI");
        objectsToChangeActiveStatus.Add(treasuresUI);
    }

    public void ChangeTreasuresUI()
    {
        objectsToChangeActiveStatus[3] = GameObject.FindGameObjectWithTag("TreasuresUI");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowHidePauseMenu();
        }
    }

    public void ShowHidePauseMenu()
    {
        Fader fader = FindObjectOfType<Fader>();
        if (fader)
            if (!fader.HasFadedIn()) return;
        ChangeActiveStatus(objectsToChangeActiveStatus);
        Time.timeScale = ChangeTimeScale();
    }

    private void ChangeActiveStatus(List<GameObject> objectsToChangeActiveStatus)
    {
        foreach (var obj in objectsToChangeActiveStatus)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }

    private int ChangeTimeScale()
    {
        return Time.timeScale == 0 ? 1 : 0;
    }
}
