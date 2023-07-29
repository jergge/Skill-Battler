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

    protected void TriggerHeal(DamageReport damageReport)
    {
        if(damageReport.targetRemainingHP > 0 && !damageReport.lethalHit)
        {
            livingEntity.TakeHeal(new HealPacket(sourceSkill.baseHealAmount, ActionUnit.Type.holy, source));
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
