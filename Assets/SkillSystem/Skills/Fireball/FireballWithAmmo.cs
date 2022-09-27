using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
public class FireballWithAmmo : Fireball
{
 
    public override void Cast(Transform spawnLoaction, TargetInfo targetInfo)
    {
        if (OnCooldown()) {
            return;
        }
        //Debug.Log("Casting the skill Fireball");
        FireballWithAmmo temp = GameObject.Instantiate(this);
        temp.spellState = SpellState.InWorld;
        temp.transform.position = spawnLoaction.position;
        temp.targetPoint = targetInfo.position;
        temp.targetObject = targetInfo.target;
        //temp.casterVelocity = source.GetComponent<Rigidbody>().velocity;
        temp.SetSource(source);

        ResetCooldown();
    }
}}
