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

    public DamageReport TakeDamage(DamagePacket damage)
    {
        if ( damage.value >= SingleHitDestroyAmount )
        {
            Die();
            return new DamageReport(damage.source, this, this.transform.position, damage.value, damage.value, 0, SingleHitDestroyAmount, 0, 0, true);
        }
        else return DamageReport.NoDamge(damage.source, this, this.transform.position, damage.value, SingleHitDestroyAmount, 100);
    }

    public DamageReport? TakeHeal(DamagePacket healUnit)
    {
        return null;
    }

    protected void Die()
    {
        TriggerOnDeath();
        gameObject.SetActive(false);
    }
}
