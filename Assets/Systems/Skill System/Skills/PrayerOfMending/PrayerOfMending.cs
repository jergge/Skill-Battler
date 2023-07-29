using System.Collections;
using System.Collections.Generic;
using DamageSystem;
using Jergge.Extensions;
using SkillSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class PrayerOfMending : Skill, IActiveSkill
{
    public float baseHealAmount = 50f;
    float healAmount => GetModifiedValue(SkillSystem.Properties.ModifiableSkillProperty.ModifyValue.healing, baseHealAmount);
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
            pom.Configure(this);
            pom.remainingCharges = baseChargesCount;
            pom.sourceSkill = this;
        }
    }
}
