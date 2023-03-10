using System.Collections;
using System.Collections.Generic;
using DamageSystem;
using Jergge.Extensions;
using SkillSystem;
using SkillSystem.Properties;
using UnityEngine;

//Shoot a second fireball at a random target every 5 seconds when you cast
//Every 5 seconds, if a fireball hits a target, also shoot another fireball at a random target
public class DualCastFireball : IModifySkill<Fireball>
{
    public float timeBetweenDualCastProcs = 2;
    protected float remaingDualCastProcTime;
    public float detectionRange = 20;

    public float timeBetweenOnDamageProcs = 5;
    protected float remaingOnDamageProcTime;

    void Update()
    {
        ReduceDuration();
        RecudeProcCooldown();
    }

    protected void RecudeProcCooldown()
    {
        remaingDualCastProcTime = (remaingDualCastProcTime <= 0) ? 0 : remaingDualCastProcTime - Time.deltaTime;
        remaingOnDamageProcTime = (remaingOnDamageProcTime <= 0) ? 0 : remaingOnDamageProcTime - Time.deltaTime;
    }

    protected override void OnSkillCast(CastEventInfo castEventInfo)
    {
        LivingEntity randomTarget = gameObject.GetInRange<LivingEntity>(detectionRange).Random<LivingEntity>();
        Debug.Log("Casting the dualcast fireball @" + randomTarget.name);

        skill.Cast(gameObject.GetComponent<SkillManager>().skillSpawnLocation, 
            new TargetInfo(randomTarget.gameObject, 20, randomTarget.transform.position));

        remaingDualCastProcTime = timeBetweenDualCastProcs;
    }

    protected override void OnDealDamage(DamageInfo? damageInfo)
    {
        LivingEntity randomTarget = gameObject.GetInRange<LivingEntity>(detectionRange).Random<LivingEntity>();
        Debug.Log("Casting the bonus on hit fireball @" + randomTarget.name);

        skill.Cast(gameObject.GetComponent<SkillManager>().skillSpawnLocation, 
            new TargetInfo(randomTarget.gameObject, 20, randomTarget.transform.position));

        remaingOnDamageProcTime = timeBetweenDualCastProcs;
    }
}
