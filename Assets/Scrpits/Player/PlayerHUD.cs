using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerHUD : MonoBehaviour
{
    public GameObject energies;
    public PlayerMP prefab;
    public Player player;
    public DPad dPad;
    List<PlayerMP> mps = new List<PlayerMP>();

    public float gap;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach ( Transform t in energies.transform)
        {
            Destroy(t.gameObject);
        }
        int i = 0;
        foreach(ManaStats m in GetComponentsInParent<ManaStats>())
        {
             PlayerMP mp = GameObject.Instantiate(prefab);
             mp.transform.SetParent(energies.transform);
             mp.stats = m;
             mp.transform.localPosition = Vector3.zero + Vector3.down * gap * i;

            i++;
             //mps.Add(mp);
        }
    }
}
