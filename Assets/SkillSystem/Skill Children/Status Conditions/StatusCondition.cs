using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem {

public class StatusCondition : MonoBehaviour
{
    public Skill attatchedTo;

    public float baseDuration = 6f; //remove later so that these are bound to buffs
    float timeRemaining;

    protected LivingEntity livingEntity;

    void Awake() {
        if (gameObject.TryGetComponent<LivingEntity>(out livingEntity))
        {
            // StatusCondition[] toTest = gameObject.GetComponents<StatusCondition>();
            // if (toTest.Length != 0)
            // {
            //     foreach ( var stat in toTest )
            //     {
            //         if (stat.GetType() is this.GetType() )
            //     }
            // }

        } else {
            Destroy(this);
        }

        timeRemaining = baseDuration;
    }

    void Update()
    {
        if (timeRemaining <= 0)
        {
            Destroy(this);
        } else {
            timeRemaining -= Time.deltaTime;
        }
    }
}}
