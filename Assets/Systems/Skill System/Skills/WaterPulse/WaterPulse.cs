using System.Collections;
using System.Collections.Generic;
using SkillSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaterPulse : Missile, IActiveSkill
{
    void Update()
    {
        TickCooldown();
    }

    public override void PrepareCast(Transform spawnLoaction, TargetInfo targetInfo)
    {
        if (IsOnCooldown())
        {
            return;
        }

        missileToFire = GameObject.Instantiate<MissilePrefab>(misslePrefab, spawnLoaction.position, Quaternion.identity);

        missileToFire.Configure(this, targetInfo);

        missileToFire.gameObject.SetActive(false);

        ResetCooldown();
    }



}
