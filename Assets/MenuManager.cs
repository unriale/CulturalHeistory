using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject tutorialPanel;
   

    private static bool isTutorialPanelActive = true;

    private void Awake()
    {
        tutorialPanel.SetActive(isTutorialPanelActive);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ChangePanel();
        }
    }

    public void ChangePanel()
    {
        isTutorialPanelActive = !isTutorialPanelActive;
        tutorialPanel.SetActive(isTutorialPanelActive);
    }
}
