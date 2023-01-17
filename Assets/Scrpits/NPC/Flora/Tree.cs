using System.Collections;
using System.Collections.Generic;
using DamageSystem;
using UnityEngine;

public class Tree : Flora, IDamageable
{
    public float SingleHitDestroyAmount = 10f;

    public bool IsDead()
    {
        throw new System.NotImplementedException();
    }

    public DamageInfo? TakeDamage(DamageUnit damage)
    {
        if ( damage.baseAmount >= SingleHitDestroyAmount )
        {
            Die();
            return new DamageInfo(true, (float)damage, 0, (float)damage);
        }
        else return new DamageInfo(false, 0, Mathf.CeilToInt(SingleHitDestroyAmount), (float)damage);
    }

    public DamageInfo? TakeHeal(DamageUnit healUnit)
    {
        return null;
    }

    protected void Die()
    {
        TriggerOnDeath();
        gameObject.SetActive(false);
    }
}
