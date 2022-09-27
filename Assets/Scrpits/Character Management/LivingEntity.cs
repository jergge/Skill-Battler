using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

[RequireComponent(typeof(ManaStats), typeof(HealthStats), typeof(SkillManager))]
public class LivingEntity : MonoBehaviour, IDamageable, IForceable, IOnCastEvents
{
    public HealthStats HP;
    public ManaStats MP;
    public SkillManager skillManager;

    protected void Start()
    {
        HP = GetComponent<HealthStats>();
        MP = GetComponent<ManaStats>();
        skillManager = GetComponent<SkillManager>();
    }

    public event Action<CastEventInfo, CheckForAny> CanICast;
    protected void FireCanICast(CastEventInfo info, CheckForAny check)
    {
        if (CanICast != null)
        {
            CanICast(info, check);
        }
    }

    public event Action<CastEventInfo> OnBeforeCast;
    protected void FireOnBeforeCast(CastEventInfo info)
    {
        if (OnBeforeCast != null)
        {
            OnBeforeCast(info);
        }
    }

    public event Action<CastEventInfo> OnAfterCast;
    protected void FireOnAfterCast(CastEventInfo info)
    {
        if (OnAfterCast != null)
        {
            OnAfterCast(info);
        }
    }

    public event Action<DamageInfo> OnTakeDamage;
    public DamageInfo TakeDamage(int d)
    {
        var stats = HP.SubtractHP(d);
        Debug.Log(name + " took " + stats.delta + " damage");
        //HP.SubtractHP(d);

        DamageInfo info = new DamageInfo((stats.current == 0) ? true : false, d, stats.current);
        
        if(OnTakeDamage != null)
        {
            OnTakeDamage(info);
        }
        
        if (stats.current == 0)
        {
            Destroy(gameObject);
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
}
