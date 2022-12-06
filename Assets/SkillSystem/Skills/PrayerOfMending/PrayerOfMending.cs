using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jergge.Extensions;
using UnityEngine.InputSystem;
using SkillSystem;
using DamageSystem;

public class PrayerOfMending : Skill, IActiveSkill
{
    public float baseHealAmount = 50f;
    public int baseChargesCount = 5;
    protected int remainingCharges;
    public float baseJumpRange = 20f;
    public float projectileSpeed = 10f;
    public float baseBuffDuration = 10f;

    public MissilePrefab missilePrefab;
    
    public void Cast(Transform spawnLoaction, TargetInfo targetInfo)
    {
        LivingEntity livingEntityTarget;
        if (targetInfo.target.gameObject.TryGetComponent<LivingEntity>(out livingEntityTarget))
        {
            //var missile = Instantiate(missilePrefab);
            var pom = livingEntityTarget.gameObject.AddComponent<PrayerOfMendingBuff>();
            pom.remainingCharges = baseChargesCount;
            pom.sourceSkill = this;
        }
    }

    public override void UpdateInSpellBook()
    {
        base.UpdateInSpellBook();
    }

    void TriggerHeal(DamageInfo info)
    {
        if (info.amountDone >0 && !info.lethalHit)
        {
            var le = gameObject.GetComponent<LivingEntity>();
            le.TakeHeal(((int)baseHealAmount));
            le.OnTakeDamage -= TriggerHeal;
            if (remainingCharges > 0 )
            {
                LivingEntity newTarget = GetInDistance<LivingEntity>(baseJumpRange).Random<LivingEntity>();

                var pom = newTarget.gameObject.AddComponent<PrayerOfMending>();
                pom.remainingCharges = remainingCharges -1;
            }
            Destroy(this);
        }
    } 
}
