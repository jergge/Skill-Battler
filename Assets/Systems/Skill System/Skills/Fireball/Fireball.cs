using System;
using System.Collections;
using System.Collections.Generic;
using DamageSystem;
using SkillSystem;
using SkillSystem.Properties;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fireball : Missile, IActiveSkill, IOnDealDamageEvents
{
    public new event Action<DamageInfo?> OnDealDamage;

    void Update()
    {
        TickCooldown();
    }

    public void Cast(Transform spawnLoaction, TargetInfo targetInfo)
    {
        MissilePrefab missile = GameObject.Instantiate<MissilePrefab>(misslePrefab, spawnLoaction.position, Quaternion.identity);
        
        missile.Configure(this, targetInfo);
        
        ResetCooldown();
    }

}

