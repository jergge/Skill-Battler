using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

public class WaterPulse : Skill
{
    public Missile misslePrefab;
    public int baseDamage = 20;
    public float speed = 10;

    public override void Cast(Transform spawnLoaction, TargetInfo targetInfo)
    {
        if(OnCooldown())
        {
            return;
        }

        Missile missile = GameObject.Instantiate<Missile>(misslePrefab);

        missile.transform.position = spawnLoaction.position;
        missile.damage = baseDamage;
        missile.speed = speed;
        missile.initialTarget = targetInfo.position;

        ResetCooldown();
    }


}
