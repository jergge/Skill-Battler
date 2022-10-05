using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

public class WaterShieldPrefab : MonoBehaviour
{
    public float radius;
    public float expansionTime;
    public bool expandOnStart = true;
    public GameObject source;
    // Start is called before the first frame update
    void Start()
    {
       transform.SetParent(source.transform); 
    }

    void OnTriggerEnter(Collider other)
    {
        MissilePrefab p;
        if ( other.gameObject.TryGetComponent<MissilePrefab>(out p))
        {
            if (p.source.layer != source.gameObject.layer)
            {
            Destroy(other.gameObject);
            }

            ManaStats ms;
            if(source.TryGetComponent<ManaStats>(out ms))
            {
                ms.Regen(20);
            }
        }
    }


}
