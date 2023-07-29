using System;
using System.Collections;
using System.Collections.Generic;
using DamageSystem;
using SkillSystem;
using SkillSystem.Properties;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fireball : Missile, IOnDealDamageEvents
{
    public new event Action<DamageReport?> OnDealDamage;

    void Update()
    {
        TickCooldown();
    }

    public override void PrepareCast(Transform spawnLoaction, TargetInfo targetInfo)
    {
        // Debug.Log("Fireball prep cast method");
        missileToFire = GameObject.Instantiate<MissilePrefab>(misslePrefab, spawnLoaction.position, Quaternion.identity);
        
        missileToFire.Configure(this, targetInfo);

        TriggerOnCreateMissileObject(missileToFire);

        // Debug.Log(missileToFire);

        //missileToFire.gameObject.SetActive(false);

        ResetCooldown();
    }
}

