using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem{
public class TargetInfo
{
    #nullable enable
    public GameObject? target;

    public float? distanceToTarget;

    public Vector3 position;

    public Vector3 direction;

    public TargetInfo(GameObject target, float distanceToTarget, Vector3 targetPosition)
    {
        this.target = target;
        this.distanceToTarget = distanceToTarget;
        this.position = targetPosition;
    }
    public TargetInfo(){
        
    }
    #nullable disable
}}
