using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

public class BlinkStrike : Skill
{
    public float baseRange = 20f;
    protected float range => GetModifiedValue(SkillSystem.Properties.ModifiableSkillProperty.ModifyValue.range, baseRange);
    public float extraDistanceBehind = 5f;

    public float baseBlinkDuration = .25f;

    public int baseDamage = 10;
    protected int damage => GetModifiedValueInt(SkillSystem.Properties.ModifiableSkillProperty.ModifyValue.damage, baseDamage);


    //tweening the blink
    protected float castTime;
    protected float endTime => castTime + baseBlinkDuration;
    protected Vector3 initialPos;
    protected Vector3 finalPos;

    
    public override void Cast(Transform spawnLoaction, TargetInfo targetInfo)
    {
        if (!OnCooldown())
        {
            if ( Vector3.Distance(source.transform.position, targetInfo.position) <= range)
            {
                ResetCooldown();
                StartCoroutine(BlinkTravel(source.transform.position, targetInfo.target.transform));

            }

        }
    }

    IEnumerator BlinkTravel(Vector3 startPos, Transform target)
    {
        PauseCooldown();
        //source.DisableColliders();
        source.GetComponent<Rigidbody>().useGravity = false;
        
        float timeRemaining = baseBlinkDuration;

        while ( timeRemaining > 0)
        {
            Vector3 dirToTarget = target.position - startPos;
            dirToTarget.Normalize();

            Vector3 targetPos = target.position + dirToTarget * extraDistanceBehind;    
            
            source.GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(targetPos, startPos, timeRemaining/baseBlinkDuration));

            timeRemaining  -= Time.deltaTime;
            yield return null;
        }

        source.transform.LookAt(target, Vector3.up);
        
        source.GetComponent<Rigidbody>().useGravity = true;
        //source.EnableColliders();
        
        IDamageable targetDmg;
        if (target.TryGetComponent<IDamageable>(out targetDmg))
        {
            // targetDmg.TakeDamage(damage);
            if ( targetDmg.TakeDamage(damage).lethalHit )
            {
                remainingCooldown = 0;
            }
        }
        ResumeCooldown();
    }


    public override void OnStartInSpellbook()
    {
        base.OnStartInSpellbook();
    }

    public override void OnStartInWorld()
    {
        base.OnStartInWorld();
    }

    public override void UpdateInSpellBook()
    {
        base.UpdateInSpellBook();
    }

    public override void UpdateInWorld()
    {
        base.UpdateInWorld();
    }
}
