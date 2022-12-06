using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using UnityEngine.InputSystem;

public class WaterPulse : Missile, IActiveSkill
{
    public void Cast(Transform spawnLoaction, TargetInfo targetInfo)
    {
        if(CoolingDown())
        {
            return;
        }

        MissilePrefab missile = GameObject.Instantiate<MissilePrefab>(misslePrefab, spawnLoaction.position, Quaternion.identity);

        missile.Configure(this, targetInfo);

        ResetCooldown();
    }

}
