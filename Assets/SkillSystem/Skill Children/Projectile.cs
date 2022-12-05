using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem.Properties;
using Jergge.Extensions;
using System;


namespace SkillSystem{
    [Obsolete("Use the Missle and MisslePrefab classes now")]
public abstract class Projectile : Skill
{

    public bool tracksTarget = false;
    public LayerMask trackableTargetLayers;

    protected Vector3 setDirection;
    
    public float baseMissleSpeed = 5;
    protected float missleSpeed =>
        GetModifiedValue(ModifiableSkillProperty.ModifyValue.speed, baseMissleSpeed);

    public float baseMaxTravelDistance = 50;
    protected float maxTravelDistance =>
        GetModifiedValue(ModifiableSkillProperty.ModifyValue.range, baseMaxTravelDistance); 

    public float baseMaxTravelTime;
    protected float remainingTime;
    protected Vector3 targetPoint;
    protected GameObject targetObject;

    public override void OnStartInWorld()
    {
        remainingTime = baseMaxTravelTime;
        transform.LookAt(targetPoint, Vector3.up);

        if(targetObject == null)
        {
            tracksTarget = false;
        } else if(!trackableTargetLayers.Contains(targetObject))
        {
            tracksTarget = false;
        }
    }

    public override void UpdateInSpellBook()
    {
        base.UpdateInSpellBook();
    }

    public override void UpdateInWorld()
    {
        if(tracksTarget)
        {
            //some random movement
            transform.LookAt(targetObject.transform.position, Vector3.up);
            transform.position = transform.position + transform.forward * missleSpeed * Time.deltaTime; 
        } else
        {
            transform.position = transform.position + transform.forward * missleSpeed * Time.deltaTime;
        }

        if (remainingTime <= 0) 
        {
            Destroy(gameObject);
        } else
        {
            remainingTime -= Time.deltaTime;
            //Debug.Log(remainingTime);
        }
    }
}}