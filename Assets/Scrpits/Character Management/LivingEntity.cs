using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

[RequireComponent(typeof(StatsTracker), typeof(StatsTracker), typeof(SkillManager))]
[RequireComponent(typeof(Animator))]
public class LivingEntity : MonoBehaviour, IDamageable, IForceable, IOnCastEvents
{
    public Transform NPCHeadTarget;
    public Transform NPCBodyTarget;
    public StatsTracker HP;
    public StatsTracker MP;
    public SkillManager skillManager;
    public Animator animator;
    public List<MonoBehaviour> disableOnDie = new List<MonoBehaviour>();

    protected void Start()
    {
        // HP = GetComponent<HealthStats>();
        // MP = GetComponent<StatsTracker>();
        // skillManager = GetComponent<SkillManager>();
    }

    public event Action<CastEventInfo, CheckForAny> CanICast;
    protected void FireCanICast(CastEventInfo info, CheckForAny check)
    {
        // if (CanICast != null)
        // {
        //     CanICast(info, check);
        // }

        CanICast?.Invoke(info, check);
    }

    public event Action<CastEventInfo> OnBeforeCast;
    protected void FireOnBeforeCast(CastEventInfo info)
    {
        // if (OnBeforeCast != null)
        // {
        //     OnBeforeCast(info);
        // }

        OnBeforeCast?.Invoke(info);
    }

    public event Action<CastEventInfo> OnAfterCast;
    protected void FireOnAfterCast(CastEventInfo info)
    {
        // if (OnAfterCast != null)
        // {
        //     OnAfterCast(info);
        // }

        OnAfterCast?.Invoke(info);
    }

    public event Action<DamageInfo> OnTakeDamage;
    public DamageInfo TakeDamage(float damage)
    {
        //var stats = HP.SubtractHP(damage);

        HP -= damage;
        var HPChangeInfo = HP.afterLastChange;

        Debug.Log(name + " took " + HPChangeInfo.delta + " damage");

        DamageInfo damageInfo = new DamageInfo(HPChangeInfo);
        //DamageInfo damageInfo = new DamageInfo((statsInfo.current == 0) ? true : false, damage, statsInfo.current);

        OnTakeDamage?.Invoke(damageInfo);
        
        if (HPChangeInfo.current == 0)
        {
            OnDeath();
        }
        return damageInfo;
    }

    public void TakeHeal(float h)
    {
        HP += h;
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
        ForceMode forceMode
    )
    {
        Rigidbody rb;
        if (gameObject.TryGetComponent<Rigidbody>(out rb))
        {
            rb.AddExplosionForce(magnitude, origin, explosionRadius, upwardsModifier, forceMode);
        }
    }

        public void DisableColliders()
    {
        foreach (MeshRenderer mr in gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            mr.enabled = false;
        }
        foreach (Collider col in gameObject.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }
    }

    public void EnableColliders()
    {
        foreach (MeshRenderer mr in gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            mr.enabled = true;
        }
        foreach (Collider col in gameObject.GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
        }
    }

    void OnDeath()
    {
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