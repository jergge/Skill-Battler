using System.Collections;
using System.Collections.Generic;
using DamageSystem;
using SkillSystem;
using UnityEngine;

public class Ignited : UniqueBuff<Ignited>
{
    public int procDamage = 200;
    public int procStacks = 3;

    public GameObject source;

    public int stackCount = 1;

    void Start()
    {
        baseDuration = 10f;
        remainingTime = baseDuration;
    }

    void Update()
    {
        if (stackCount >= procStacks)
        {
            AtMaxStacks();
        }

        ReduceDuration();
    }

    void AtMaxStacks()
    {
        IDamageable target;
        if (gameObject.TryGetComponent<IDamageable>(out target))
        {
            target.TakeDamage( new DamagePacket(procDamage, ActionUnit.Type.fire, source) );
        }

        Destroy(this);
    }

    protected override S CombineTwoBuffs<S>(S other)
    {
        baseDuration = Mathf.Max(this.baseDuration, other.baseDuration);
        remainingTime = baseDuration;
        stackCount = this.stackCount + other.stackCount;
        procStacks = Mathf.Min(this.procStacks, other.procStacks);
        procDamage = Mathf.Max(this.procDamage, other.procDamage);

        Destroy(other);

        return (S)this;
    }
}