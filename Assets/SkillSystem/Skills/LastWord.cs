using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SkillSystem;

public class LastWord : Skill
{
    public float radius;
    public int damage;
    public float refreshCloseTargetsTime = .3f;

    LivingEntity[] allLE;

    ProximityTracker<LivingEntity, Action<CastEventInfo>> tracker;


    // A passive skill that damages any enemy who casts a spell within a certain radius

    public override void Cast(Transform spawnLoaction, TargetInfo targetInfo)
    {
        throw new System.NotImplementedException();
    }

    public override void OnStartInSpellbook()
    {
        tracker = new ProximityTracker<LivingEntity, Action<CastEventInfo>>(source.transform, radius, refreshCloseTargetsTime, "OnAfterCast", TriggerDamage);
    }

    public override void OnStartInWorld()
    {
        base.OnStartInWorld();
    }

    public override void UpdateInSpellBook()
    {
        
    }

    public override void UpdateInWorld()
    {
        base.UpdateInWorld();
    }

    void TriggerDamage(CastEventInfo info)
    {
        IDamageable dmg;
        if (info.source.TryGetComponent<IDamageable>(out dmg))
            dmg.TakeDamage(damage);
    }

}
