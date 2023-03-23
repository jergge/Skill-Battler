using System;
using System.Collections;
using System.Collections.Generic;
using DamageSystem;
using SkillSystem.Properties;
using UnityEngine;

namespace SkillSystem {
public abstract class Missile : Skill
{
    public MissilePrefab misslePrefab;
    
    public float baseDamage = 20;
    public float damage =>
        GetModifiedValue(ModifiableSkillProperty.ModifyValue.damage, baseDamage);

    public DamageUnit.Type damageType;

    public float baseSpeed = 10;
    public float speed =>
        GetModifiedValue(ModifiableSkillProperty.ModifyValue.speed, baseSpeed);

    public float BaseMaxTravelDistance = 10;
    public float maxTravelDistance =>
        GetModifiedValue(ModifiableSkillProperty.ModifyValue.range, BaseMaxTravelDistance);

    public float baseSizeScale = 1;
    public float sizeScale =>
        GetModifiedValue(ModifiableSkillProperty.ModifyValue.radius, baseSizeScale);

    public float maxTravelTime = 10;

    public LayerMask collisionOffload;
    public List<GameObject> createOnOffloadTrigger = new List<GameObject>();
    public LayerMask collisionSelfDestruct;

    protected MissilePrefab CreateFromPrefab(Vector3 position,  Quaternion rotation, TargetInfo targetInfo)
    {
        MissilePrefab missilePrefab = GameObject.Instantiate<MissilePrefab>(misslePrefab, position, rotation);
        missilePrefab.Configure(this, targetInfo);

        return misslePrefab;
    }

    public event Action OnCollisionOffload;

    public void TriggerOnCollisionOffload()
    {
        OnCollisionOffload?.Invoke();
    }
}}
