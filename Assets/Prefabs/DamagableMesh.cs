using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageSystem;

public class DamagableMesh : MonoBehaviour, IDamageable
{
    public GameObject passToIDamageable;
    IDamageable IDamageable;

    void Start()
    {
        if ( passToIDamageable.TryGetComponent<IDamageable>(out IDamageable))
        {

        } else 
        {
            Destroy(this);
        }
    }

    public bool IsDead()
    {
        return IDamageable.IsDead();
    }

    public DamageInfo? TakeDamage(DamageUnit damage)
    {
        return IDamageable.TakeDamage(damage);
    }

    public DamageInfo? TakeHeal(DamageUnit heal)
    {
        return IDamageable.TakeHeal(heal);
    }
}
