using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public PlayerUI defautUI;

    public PauseMenu pauseMenu;
    public PlayerHUD playerHUD;

    PlayerUI currentUI;
    [Obsolete] PlayerUI previousUI;

    Stack<PlayerUI> menuHistory = new Stack<PlayerUI>();

    void Start()
    {
        currentUI = defautUI;
        currentUI.gameObject.SetActive(true);
    }

    protected void SetUI(PlayerUI newUI)
    {
        Debug.Log("Setting " + newUI.name + " to active");
        currentUI.gameObject.SetActive(false);
        newUI.gameObject.SetActive(true);
        menuHistory.Push(currentUI);
        currentUI = newUI;

    }

    protected void RestorePreviousUI()
    {
        currentUI.gameObject.SetActive(false);
        currentUI = (menuHistory.Count > 0) ? menuHistory.Pop() : defautUI;
        currentUI.gameObject.SetActive(true);
    }


    void OnPauseMenu()
    {
        if (currentUI != pauseMenu)
        {
            SetUI(pauseMenu);
        } else
        {
            RestorePreviousUI();
        }
    }
}
