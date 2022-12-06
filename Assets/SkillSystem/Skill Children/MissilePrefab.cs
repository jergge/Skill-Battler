using System.Collections;
using System.Collections.Generic;
using Jergge.Extensions;
using UnityEngine;
using SkillSystem.Properties;
using DamageSystem;

namespace SkillSystem{
public class MissilePrefab : MonoBehaviour
{

    float speed;
    float damage;
    DamageUnit.DamageType damageType;
    float maxTravelDistance;
    float distanceTraveled;
    float MaxTravelTime;
    float timeAlive;
    Skill.ValidTargets validTargets;

    List<IDamageable> alreadyDamagedInFrame = new List<IDamageable>();

    public GameObject source;

    LayerMask collisionDamage;
    LayerMask collisionDestroy;

    public GameObject targetObject;
    public Vector3 initialTarget;
    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(initialTarget);
    }

    public void Configure(Missile m, TargetInfo targetInfo)
    {
        speed = m.speed;
        damage = m.damage;
        damageType = m.damageType;
        targetObject = targetInfo.target;
        initialTarget = targetInfo.position;
        maxTravelDistance = m.maxTravelDistance;
        MaxTravelTime = m.MaxTravelTime;
        collisionDamage = m.collisionOffload;
        collisionDestroy = m.collisionSelfDestruct;
        source = m.GetSource();
        validTargets = m.validTargets;
    }
    // Update is called once per frame
    void Update()
    {
        CheckExpiery();

        Vector3 moveAmount = transform.forward * speed * Time.deltaTime;
        transform.position += moveAmount;
        
        timeAlive += Time.deltaTime;
        distanceTraveled += speed * Time.deltaTime;
    }

    void FixedUpdate()
    {
        alreadyDamagedInFrame.Clear();
    }

    void CheckExpiery()
    {
        if((timeAlive > MaxTravelTime) || (distanceTraveled > maxTravelDistance))
        {
            Destroy(gameObject);
        } 
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Skill " + name + " colliding with " + other.name);
        if( collisionDestroy.Contains(other.gameObject) )
        {
            Die();
        } else if ( collisionDamage.Contains(other.gameObject) && Skill.IsValidTarget(source, other.gameObject, validTargets))
        {
            IDamageable damagable;
            if(other.gameObject.TryGetComponent<IDamageable>(out damagable))
            {
                if(!alreadyDamagedInFrame.Contains(damagable))
                {
                    alreadyDamagedInFrame.Add(damagable);
                    Debug.Log("Skill " + name + " dealing damage to " + other.name);
                    // damagable.TakeDamage(damage);
                    damagable.TakeDamage(new DamageUnit(damage, damageType, source));
                    Die();
                }
            }
        }
    }

    void Die()
    {
        //do some stuff beforehand...

        Destroy(gameObject);
    }
}}
