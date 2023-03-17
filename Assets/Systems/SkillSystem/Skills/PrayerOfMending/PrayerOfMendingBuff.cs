using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using DamageSystem;

public class PrayerOfMendingBuff : Buff
{
    public int remainingCharges;
    LivingEntity livingEntity;
    public PrayerOfMending sourceSkill;
    float timeAlive;

    // Start is called before the first frame update
    void Start()
    {
        if(!gameObject.TryGetComponent<LivingEntity>(out livingEntity))
        {
            Destroy(this);
            return;
        }
        livingEntity.OnTakeDamage += TriggerHeal;
    }

    void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive > sourceSkill.baseBuffDuration)
        {
            RemoveSelf();
        }
    }

    protected void TriggerHeal(DamageInfo info)
    {
        if(info.realHPLost >0 && !info.wasLethalHit)
        {
            livingEntity.TakeHeal((DamageUnit)sourceSkill.baseHealAmount);
            if (remainingCharges == 0)
            {
                RemoveSelf();
            }
                remainingCharges -= 1;
                JumpToNew();
        }
    }

    void RemoveSelf()
    {
        livingEntity.OnTakeDamage -= TriggerHeal;
        Destroy(this);
    }

    void JumpToNew()
    {

    }

    public override void Configure(Skill skill)
    {
        throw new System.NotImplementedException();
    }
}
