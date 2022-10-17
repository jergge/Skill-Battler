using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class DPad : MonoBehaviour
{
    public Image up;
    public Image down;
    public Image left;
    public Image right;

    public void NewMap(DPadMap map)
    {
        Debug.Log("calling a new d map");
    }

    void Start()
    {
        Player player = GetComponentInParent<Player>();
        player.skillManager.NewDPadMap += NewMap;
    }
}
