using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerUI : MonoBehaviour
{
    protected PlayerInput playerInput;

    void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
    }

    protected void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    protected void UseMenuInputMap()
    {
        playerInput.SwitchCurrentActionMap("Menu Control");
    }

    protected void UseDefaultInputMap()
    {
        playerInput.SwitchCurrentActionMap(playerInput.defaultActionMap);
    }
}
