using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

public class Firebugs : Skill
{
    public int numberSpawned;
    public float missleMoveSpeed;
    public float detectionRadius;
    public int damage;
    Vector3 targetPosotion;
    float timeSpentTowardsTarget = 0;
    public float maxTimeInFlight = .4f;
    Vector3 dirToTarget;
    
    //State bool, true is looking for new target, false is already aquired target;
    bool lookingForTarget = true;

    public override void Cast(Transform spawnLoaction, TargetInfo targetInfo)
    {
        for (int i = 0; i < numberSpawned; i++)
        {
           Firebugs temp = GameObject.Instantiate(this);
           temp.transform.SetParent(source.transform);
           temp.transform.position = source.transform.position + Vector3.up * 3;
           temp.spellState = SpellState.InWorld;
        }
    }

    public override void OnStartInSpellbook()
    {
        base.OnStartInSpellbook();
    }

    public override void OnStartInWorld()
    {
        EnableColliders();
    }

    public override void UpdateInSpellBook()
    {
        //throw new System.NotImplementedException();
    }

    public override void UpdateInWorld()
    {
        if(lookingForTarget)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
            foreach (var hitCollider in hitColliders)
            {
                IDamageable dmg;
                if (IsValidTarget(source.gameObject, hitCollider.gameObject) && hitCollider.gameObject.TryGetComponent<IDamageable>(out dmg))
                {
                    targetPosotion = hitCollider.gameObject.transform.position;
                    lookingForTarget = false;
                    transform.parent = null;
                    Debug.Log(name + " has found target: " + hitCollider.gameObject.name);
                    return;
                }
            }
        } else
        {   
            if (timeSpentTowardsTarget <= maxTimeInFlight)
            {
                transform.position = transform.position + (targetPosotion - transform.position).normalized * missleMoveSpeed * Time.deltaTime;
                timeSpentTowardsTarget += Time.deltaTime;
            } else {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        IDamageable dmg;
        if (IsValidTarget(source.gameObject, other.gameObject) && other.gameObject.TryGetComponent<IDamageable>(out dmg))
        {
            dmg.TakeDamage(damage);
            Destroy(gameObject);
        }

    }

    void OnDrawGizmos()
    {
        if (spellState == SpellState.InWorld && lookingForTarget)
        {
            Gizmos.DrawWireSphere(gameObject.transform.position, detectionRadius);
        }
    }
}
