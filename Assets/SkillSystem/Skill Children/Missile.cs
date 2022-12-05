using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem.Properties;

namespace SkillSystem{
public abstract class Missile : Skill
{
    public MissilePrefab misslePrefab;
    
    public float baseDamage = 20;
    public float damage =>
        GetModifiedValue(ModifiableSkillProperty.ModifyValue.damage, baseDamage);

    public float baseSpeed = 10;
    public float speed =>
        GetModifiedValue(ModifiableSkillProperty.ModifyValue.speed, baseSpeed);

    public float BaseMaxTravelDistance = 10;
    public float maxTravelDistance =>
        GetModifiedValue(ModifiableSkillProperty.ModifyValue.range, BaseMaxTravelDistance);

    public float MaxTravelTime = 10;

    public LayerMask collisionOffload;
    public LayerMask collisionSelfDestruct;
}}
