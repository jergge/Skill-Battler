using System;
using System.Collections;
using System.Collections.Generic;
using DamageSystem;
using SkillSystem;
using UnityEngine;

[RequireComponent(typeof(StatsTracker), typeof(SkillManager))]
[RequireComponent(typeof(Animator))]
public class LivingEntity : MonoBehaviour, IDamageable, IHealable, IOnDeathEvents, IForceable//, IOnCastEvents
{
    public Transform NPCHeadTarget;
    public Transform NPCBodyTarget;
    public StatsTracker hitPointsTracker;
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

    public event Action<CastEventInfo> OnSuccessfulSkillCast;
    protected void FireOnAfterCast(CastEventInfo info)
    {
        OnSuccessfulSkillCast?.Invoke(info);
    }

    public event Action<DamageReport> OnTakeDamage;
    public DamageReport TakeDamage(DamagePacket PreMitigationDamagePacket)
    {
        // Apply damage to pre-mitigation shields
        ShieldsPreMitigation(PreMitigationDamagePacket);

        // Mitigate the remaing damage
        DamagePacket mitigatedDamagePacket = ApplyDamageMitigation(PreMitigationDamagePacket);

        // Apply remaining damage to post-mitigation shields


        // Apply remaining damage to HP (via StatsTracker)
        hitPointsTracker -= mitigatedDamagePacket.value;
        var hitPointsReport = hitPointsTracker.eventReport;

        Debug.Log(name + " took " + hitPointsReport.delta + " damage");

        DamageReport damageReport = new DamageReport(PreMitigationDamagePacket, mitigatedDamagePacket, this, this.transform.position, 0, hitPointsReport);

        OnTakeDamage?.Invoke(damageReport);
        
        if (damageReport.lethalHit)
        {
            Die();
        }
        return damageReport;
    }

    protected DamagePacket ShieldsPreMitigation(DamagePacket damagePacket)
    {
        return damagePacket;
    }

    protected DamagePacket ApplyDamageMitigation(DamagePacket damagePacket)
    {
        return damagePacket;
    }

    protected DamagePacket ShieldsPostMitigation(DamagePacket damagePacket)
    {
        return damagePacket;
    }


    public HealReport TakeHeal(HealPacket healPacket)
    {
        // Apply Healing Mitigation
        HealPacket mitigatedHealPacket = ApplyHealMitigation(healPacket);

        // Apply the healing
        hitPointsTracker += mitigatedHealPacket.value;
        var hitPointsReport = hitPointsTracker.eventReport;

        return new HealReport(healPacket, mitigatedHealPacket, this, this.transform.position, hitPointsReport);
    }

    protected HealPacket ApplyHealMitigation(HealPacket healPacket)
    {
        return healPacket;
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
        
        foreach(MonoBehaviour monoBehaviour in disableOnDie)
        {
            if ( monoBehaviour != null )
            {
                monoBehaviour.enabled = false;
            }
        }
    }

    public bool IsDead()
    {
        if (hitPointsTracker.currentPercent == 0)
        {
            return true;
        }

        return false;
    }
}