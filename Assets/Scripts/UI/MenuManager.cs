using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject tutorialPanel;
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    private static bool isTutorialPanelActive = true;

    private void Awake()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
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
