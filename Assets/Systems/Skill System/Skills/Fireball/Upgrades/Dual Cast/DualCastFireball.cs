using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        Debug.Log(castEventInfo);
        if( remaingDualCastProcTime > 0)
        {
            return;
        }

        LivingEntity randomEnemy = gameObject.GetInRange<LivingEntity>(detectionRange)
            .Random((le) => Skill.IsValidTarget(gameObject, le.gameObject, Skill.ValidTargets.Enemies));

        Debug.Log("Casting the dualcast fireball @" + randomEnemy.name);

        skill.PrepareCast(skillManager.skillSpawnLocation, 
            new TargetInfo(randomEnemy.gameObject, 20, randomEnemy.transform.position));

        remaingDualCastProcTime = timeBetweenDualCastProcs;
    }
}
