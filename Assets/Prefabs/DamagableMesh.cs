using System.Collections;
using System.Collections.Generic;
using DamageSystem;
using SkillSystem;
using UnityEngine;

public class DamagableMesh : MonoBehaviour, IDamageable
{
    public GameObject passToIDamageable;
    IDamageable IDamageable;
    
    public GameObject passToIHealable;
    IHealable IHealable;

    public GameObject passToSkillManager;
    SkillManager skillMananger;

    void Start()
    {
        if ( passToIDamageable.TryGetComponent<IDamageable>(out IDamageable) && passToIHealable.TryGetComponent<IHealable>(out IHealable))
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

    public HealInfo? TakeHeal(HealUnit heal)
    {
        return IHealable.TakeHeal(heal);
    }
}
