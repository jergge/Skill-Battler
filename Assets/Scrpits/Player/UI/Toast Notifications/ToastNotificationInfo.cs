using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public readonly struct ToastNotificationInfo
{
    public readonly Sprite sprite;
    public readonly string message;
    public readonly float duration;

    public ToastNotificationInfo(Sprite sprite, string message, float duration = 5f)
    {
        this.sprite = sprite;
        this.message = message;
        this.duration = duration;
    }
}
