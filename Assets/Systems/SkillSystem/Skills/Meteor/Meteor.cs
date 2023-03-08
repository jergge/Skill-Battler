using System.Collections;
using System.Collections.Generic;
using SkillSystem;
using UnityEngine;

public class Meteor : Missile, IDamagingSkill, IActiveSkill
{
    public float fallHeight = 50;
    public void Cast(Transform spawnLoaction, TargetInfo targetInfo)
    {
        MissilePrefab meteor = CreateFromPrefab(targetInfo.position + (Vector3.up * fallHeight), Quaternion.Euler(90, 0, 0), targetInfo);
        Debug.Log("creating new meteor at: " + (targetInfo.position + Vector3.up * fallHeight));
    }
}
