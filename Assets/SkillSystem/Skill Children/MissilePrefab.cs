using System.Collections;
using System.Collections.Generic;
using Jergge.Extensions;
using UnityEngine;
using SkillSystem.Properties;

namespace SkillSystem{
public class MissilePrefab : MonoBehaviour
{

    float speed;
    int damage;
    float maxTravelDistance;
    float distanceTraveled;
    float MaxTravelTime;
    float timeAlive;
    Skill.ValidTargets validTargets;

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
        targetObject = targetInfo.target;
        initialTarget = targetInfo.position;
        maxTravelDistance = m.maxTravelDistance;
        MaxTravelTime = m.MaxTravelTime;
        collisionDamage = m.collisionDamage;
        collisionDestroy = m.collisionDestroy;
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
        distanceTraveled += speed* Time.deltaTime;
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
        if( collisionDestroy.Contains(other.gameObject))
        {
            Die();
        } else if ( collisionDamage.Contains(other.gameObject) && Skill.IsValidTarget(source, other.gameObject, validTargets))
        {
            IDamageable d;
            if(other.gameObject.TryGetComponent<IDamageable>(out d))
            {
                d.TakeDamage(Mathf.RoundToInt(damage));
                Die();
            }
        }
    }

    void Die()
    {
        //do some stuff beforehand...

        Destroy(gameObject);
    }
}}
