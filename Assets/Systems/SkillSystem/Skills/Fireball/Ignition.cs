using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using DamageSystem;

public class Ignition : MonoBehaviour, IStackableComponent
{

    public float timeRemaining;
    public float baseDuration = 10f;
    public int damageAtStacks = 200;

    public GameObject source;

    public int stackCount { get; set; }

    void Start()
    {
        timeRemaining = baseDuration;
        stackCount ++;
    }

    void Update()
    {
        if (stackCount >= 3)
        {
            AtMaxStacks();
        }

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            Destroy(this);
        }
    }

    void AtMaxStacks()
    {
        IDamageable target;
        if (gameObject.TryGetComponent<IDamageable>(out target))
        {
            target.TakeDamage( (DamageUnit)damageAtStacks );
        }

        Destroy(this);
    }

    public void AddStack(int s)
    {
        stackCount += s;
        timeRemaining = baseDuration;
    }

}
