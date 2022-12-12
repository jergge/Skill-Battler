using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageSystem;

public class Tree : Flora, IDamageable
{
    public float SingleHitDestroyAmount = 10f;

    public bool IsDead()
    {
        throw new System.NotImplementedException();
    }

    public DamageInfo? TakeDamage(float damage)
    {
        if ( damage >= SingleHitDestroyAmount )
        {
            Die();
            return new DamageInfo(true, damage, 0, damage);
        }
        else return new DamageInfo(false, 0, Mathf.CeilToInt(SingleHitDestroyAmount), damage);
    }

    public DamageInfo? TakeHeal(float heal)
    {
        return null;
    }

    protected void Die()
    {
        TriggerOnDeath();
        gameObject.SetActive(false);
    }
}
