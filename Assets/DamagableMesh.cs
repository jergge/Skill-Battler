using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableMesh : MonoBehaviour, IDamageable
{
    public LivingEntity passDamageTarget;

    public bool IsDead()
    {
        return passDamageTarget.IsDead();
    }

    public DamageInfo TakeDamage(float damage)
    {
        return passDamageTarget.TakeDamage(damage);
    }
}
