using System.Collections;
using System.Collections.Generic;
using SkillSystem;
using UnityEngine;

public class Meteor : Missile, IDamagingSkill, IActiveSkill
{
    public float fallHeight = 50;
    public override void PrepareCast(Transform spawnLoaction, TargetInfo targetInfo)
    {
        missileToFire = CreateFromPrefab(targetInfo.position + (Vector3.up * fallHeight), Quaternion.Euler(90, 0, 0), targetInfo);

        missileToFire.gameObject.SetActive(false);

        Debug.Log("creating new meteor at: " + (targetInfo.position + Vector3.up * fallHeight));
    }
}
