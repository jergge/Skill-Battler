using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
// using UnityEngine.UIElements;

public class PlayerHUD : PlayerUI
{
    public GameObject energies;
    public PlayerMP prefab;
    public Player player;
    public DPad dPad;
    List<PlayerMP> mps = new List<PlayerMP>();

    public float spaceBetweenStatsBars;

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        foreach ( Transform t in energies.transform)
        {
            Destroy(t.gameObject);
        }

        int i = 0;

        foreach(StatsTracker statsTracker in GetComponentsInParent<StatsTracker>())
        {
             PlayerMP mp = GameObject.Instantiate(prefab);
             mp.transform.SetParent(energies.transform);
             mp.stats = statsTracker;
             mp.transform.localPosition = Vector3.zero + Vector3.down * spaceBetweenStatsBars * i;

            i++;
             //mps.Add(mp);
        }
    }
}
