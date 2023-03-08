using System.Collections;
using System.Collections.Generic;
using DamageSystem;
using Jergge.Extensions;
using SkillSystem;
using SkillSystem.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;

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
            var pom = livingEntityTarget.gameObject.AddComponent<PrayerOfMendingBuff>(this);
            pom.remainingCharges = baseChargesCount;
            pom.sourceSkill = this;
        }
    }
}
