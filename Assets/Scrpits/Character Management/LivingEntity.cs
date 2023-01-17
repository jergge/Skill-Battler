using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using DamageSystem;

[RequireComponent(typeof(StatsTracker), typeof(SkillManager))]
[RequireComponent(typeof(Animator))]
public class LivingEntity : MonoBehaviour, IDamageable, IOnDeathEvents, IForceable, IOnCastEvents
{
    public Transform NPCHeadTarget;
    public Transform NPCBodyTarget;
    public StatsTracker HP;
    public StatsTracker MP;
    public SkillManager skillManager;
    public Animator animator;
    public List<MonoBehaviour> disableOnDie = new List<MonoBehaviour>();

    public event Action OnDeath;

    public event Action<CastEventInfo, CheckForAny> CanICast;
    protected void FireCanICast(CastEventInfo info, CheckForAny check)
    {
        CanICast?.Invoke(info, check);
    }

    public event Action<CastEventInfo> OnBeforeCast;
    protected void FireOnBeforeCast(CastEventInfo info)
    {
        OnBeforeCast?.Invoke(info);
    }

    public event Action<CastEventInfo> OnAfterCast;
    protected void FireOnAfterCast(CastEventInfo info)
    {
        OnAfterCast?.Invoke(info);
    }

    public event Action<DamageInfo> OnTakeDamage;
    public DamageInfo? TakeDamage(DamageUnit damageUnit)
    {
        HP -= damageUnit.baseAmount;
        var HPChangeInfo = HP.afterLastChange;

        Debug.Log(name + " took " + HPChangeInfo.delta + " damage");

        DamageInfo damageInfo = new DamageInfo(HPChangeInfo);
        //DamageInfo damageInfo = new DamageInfo((statsInfo.current == 0) ? true : false, damage, statsInfo.current);

        OnTakeDamage?.Invoke(damageInfo);
        
        if (HPChangeInfo.current == 0)
        {
            Die();
        }
        return damageInfo;
    }


    public DamageInfo? TakeHeal(DamageUnit damageUnit)
    {
        HP += damageUnit.baseAmount;

        return new DamageInfo(HP.afterLastChange);
    }

    public void ApplyForce(Vector3 directon, float magnitude, ForceMode forceMode)
    {
        Rigidbody rb;
        if (gameObject.TryGetComponent<Rigidbody>(out rb))
        {
            rb.AddForce(directon * magnitude, forceMode);
        }
    }

    public void ApplyExplosiveForce(
        float magnitude,
        Vector3 origin,
        float explosionRadius,
        float upwardsModifier,
        ForceMode forceMode)
    {
        Rigidbody rb;
        if (gameObject.TryGetComponent<Rigidbody>(out rb))
        {
            rb.AddExplosionForce(magnitude, origin, explosionRadius, upwardsModifier, forceMode);
        }
    }

    //     public void DisableColliders()
    // {
    //     foreach (MeshRenderer mr in gameObject.GetComponentsInChildren<MeshRenderer>())
    //     {
    //         mr.enabled = false;
    //     }
    //     foreach (Collider col in gameObject.GetComponentsInChildren<Collider>())
    //     {
    //         col.enabled = false;
    //     }
    // }

    // public void EnableColliders()
    // {
    //     foreach (MeshRenderer mr in gameObject.GetComponentsInChildren<MeshRenderer>())
    //     {
    //         mr.enabled = true;
    //     }
    //     foreach (Collider col in gameObject.GetComponentsInChildren<Collider>())
    //     {
    //         col.enabled = true;
    //     }
    // }

    void Die()
    {
        OnDeath?.Invoke();

        if(animator != null) {animator.SetTrigger("Death");}
        
        foreach(MonoBehaviour m in disableOnDie)
        {
            if ( m != null )
            {
                m.enabled = false;
            }
        }
    }

    public bool IsDead()
    {
        if (HP.currentPercent == 0)
        {
            return true;
        }

        return false;
    }
}