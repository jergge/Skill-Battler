using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using SkillSystem.Properties;
using SkillSystem.Extensions;

public class Fireball : Projectile
{
    public int baseDamage;
    protected int damage => 
        GetModifiedValueInt(ModifiableSkillProperty.ModifyValue.damage, baseDamage);

    public override void UpdateInWorld()
    {
        base.UpdateInWorld();
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Triggering fireball with " + other.gameObject.name);
        if (IsValidTarget(source, other.gameObject))
        {   
            IDamageable temp;
            if ( other.gameObject.TryGetComponent<IDamageable>(out temp) )
            {
                temp.TakeDamage(damage);

                //this uses an override that I wrote to manage things that are suppoed to "stack"
                other.gameObject.AddComponent<Ignition>(1);
                Destroy(gameObject);
            }
            //other.gameObject.AddComponent<Silenced>();
        }
    }

    public override void UpdateInSpellBook()
    {
        base.UpdateInSpellBook();
    }

    public override void Cast(Transform spawnLoaction, TargetInfo targetInfo)
    {
        if (OnCooldown()) {
            return;
        }
        //Debug.Log("Casting the skill Fireball");
        Fireball temp = GameObject.Instantiate(this);
        temp.spellState = SpellState.InWorld;
        temp.transform.position = spawnLoaction.position;
        temp.targetPoint = targetInfo.position;
        temp.targetObject = targetInfo.target;
        //temp.casterVelocity = source.GetComponent<Rigidbody>().velocity;
        temp.SetSource(source);

        ResetCooldown();
    }

}

