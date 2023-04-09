using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public struct DPadMap
{
    public ButtonInfo up;
    public ButtonInfo down;
    public ButtonInfo left;
    public ButtonInfo right;

    [System.Serializable]
    public struct ButtonInfo
    {
        public delegate void Callback();

        public string name;
        public Image image;
        public Callback action;
        public UnityAction uacton;
        public UnityEvent uevent;
    }

}
