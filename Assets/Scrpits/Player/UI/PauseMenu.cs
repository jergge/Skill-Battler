using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : PlayerUI
{
    void OnEnable()
    {
        //Debug.Log("settign the pause menu to active!~");
        UnlockMouse();
        UseMenuInputMap();
    }

    void OnDisable()
    {
        //Debug.Log("Setting the pause menu to inactive");
    }
}
