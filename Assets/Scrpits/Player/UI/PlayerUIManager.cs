using System;
using System.Collections;
using System.Collections.Generic;
using SkillSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIManager : MonoBehaviour
{
    public PlayerUI defautUI;

    public PauseMenu pauseMenu;
    public PlayerHUD playerHUD;
    public SkillConfigUI skillConfigUI;

    public PlayerInput playerInput;


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
        // Debug.Log("Setting " + newUI.name + " to active");
        currentUI.gameObject.SetActive(false);
        newUI.gameObject.SetActive(true);
        // menuHistory.Push(currentUI);
        currentUI = newUI;

    }

    // protected void RestorePreviousUI()
    // {
    //     currentUI.gameObject.SetActive(false);
    //     currentUI = (menuHistory.Count > 0) ? menuHistory.Pop() : defautUI;
    //     currentUI.gameObject.SetActive(true);
    // }

    void SetUIDefault()
    {
        currentUI.gameObject.SetActive(false);
        currentUI = defautUI;
        currentUI.gameObject.SetActive(true);
        playerInput.SwitchCurrentActionMap(playerInput.defaultActionMap);
    }

    void OnPauseMenu()
    {
        if (currentUI != pauseMenu)
        {
            SetUI(pauseMenu);
        } else
        {
            SetUIDefault();
        }
    }

    public void OnSkillsMenu()
    {
        if (currentUI != skillConfigUI)
        {
            SetUI(skillConfigUI);
        } else 
        {
            SetUIDefault();
        }
    }

    void OnExitMenu()
    {
        SetUIDefault();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting the game");
    }
}
