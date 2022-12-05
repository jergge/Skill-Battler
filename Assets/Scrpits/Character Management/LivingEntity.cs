using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

[RequireComponent(typeof(StatsTracker), typeof(HealthStats), typeof(SkillManager))]
[RequireComponent(typeof(Animator))]
public class LivingEntity : MonoBehaviour, IDamageable, IForceable, IOnCastEvents
{
    public Transform NPCHeadTarget;
    public Transform NPCBodyTarget;
    public HealthStats HP;
    public StatsTracker MP;
    public SkillManager skillManager;
    public Animator animator;
    public List<MonoBehaviour> disableOnDie = new List<MonoBehaviour>();

    protected void Start()
    {
        HP = GetComponent<HealthStats>();
        MP = GetComponent<StatsTracker>();
        skillManager = GetComponent<SkillManager>();
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
    public DamageInfo TakeDamage(int d)
    {
        var stats = HP.SubtractHP(d);
        Debug.Log(name + " took " + stats.delta + " damage");
        //HP.SubtractHP(d);

        DamageInfo info = new DamageInfo((stats.current == 0) ? true : false, d, stats.current);
        
        // if(OnTakeDamage != null)
        // {
        //     OnTakeDamage(info);
        // }

        OnTakeDamage?.Invoke(info);
        
        if (stats.current == 0)
        {
            OnDeath();
        }
        return info;
    }

    public void TakeHeal(int h)
    {
        HP.AddHP(h);
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