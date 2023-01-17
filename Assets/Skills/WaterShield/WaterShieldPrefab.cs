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
    
    void Start()
    {
       transform.SetParent(source.transform); 
    }

    void OnTriggerEnter(Collider other)
    {
        MissilePrefab missile;
        if ( other.gameObject.TryGetComponent<MissilePrefab>(out missile) )
        {
            if (missile.source.layer != source.gameObject.layer)
            {
                Destroy(other.gameObject);

                StatsTracker sourceStats;
                if(source.TryGetComponent<StatsTracker>(out sourceStats))
                {
                    sourceStats += 20;
                }
            }
        }
    }
}
