using System.Collections;
using System.Collections.Generic;
using DamageSystem;
using Jergge.Extensions;
using SkillSystem.Properties;
using UnityEngine;

namespace SkillSystem{
[RequireComponent(typeof(MeshRenderer), typeof(SphereCollider))]
public class MissilePrefab : MonoBehaviour
{

    float speed;
    float damage;
    DamagePacket.Type damageType;
    float maxTravelDistance;
    float distanceTraveled;
    float MaxTravelTime;
    float timeAlive;
    float radius;
    Skill.ValidTargets validTargets;
    List<GameObject> createOnOffloadTrigger = new List<GameObject>();

    public float deathRoutineDuration = 5f;

    public List<GameObject> disableOnDieStart = new List<GameObject>();

    bool dying = false;

    protected delegate void Del(GameObject other);
    Del triggerOnIDamageableOffload;

    protected delegate void Damage();
    Damage triggerDamage;

    List<IDamageable> alreadyDamagedInFrame = new List<IDamageable>();
    List<IDamageable> alreadyDamagedEver = new List<IDamageable>();

    public GameObject source { get; protected set; }

    LayerMask collisionOffload;
    LayerMask collisionDestroy;

    GameObject targetObject;
    Vector3 initialTarget;
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
        MaxTravelTime = m.maxTravelTime;
        collisionOffload = m.collisionOffload;
        collisionDestroy = m.collisionSelfDestruct;
        source = m.source;
        validTargets = m.validTargets;
        gameObject.transform.localScale = gameObject.transform.localScale * m.sizeScale;
        triggerOnIDamageableOffload = m.TriggerOnIDamageableOffload;
        createOnOffloadTrigger = m.createOnOffloadTrigger;
    }
    // Update is called once per frame
    void Update()
    {
        if (dying)
        {
            return;
        }
        CheckExpiery();

        Vector3 moveAmount = transform.forward * speed * Time.deltaTime;
        transform.position += moveAmount;
        
        timeAlive += Time.deltaTime;
        distanceTraveled += speed * Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (dying)
        {
            return;
        }
        alreadyDamagedInFrame.Clear();
    }

    void CheckExpiery()
    {
        if((timeAlive > MaxTravelTime) || (distanceTraveled > maxTravelDistance))
        {
            Die();
        } 
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Skill " + name + " colliding with " + other.gameObject.name);
        if( collisionDestroy.Contains(other.gameObject) )
        {
            Die();
        } else if ( collisionOffload.Contains(other.gameObject) && Skill.IsValidTarget(source, other.gameObject, validTargets))
        {
            SpawnOffloadObjects();
            IDamageable damagable;
            if(other.gameObject.TryGetComponent<IDamageable>(out damagable))
            {
                if(!alreadyDamagedInFrame.Contains(damagable))
                {
                    alreadyDamagedInFrame.Add(damagable);
                    alreadyDamagedEver.Add(damagable);
                    // Debug.Log("Skill " + name + " dealing damage to " + other.name);
                    // damagable.TakeDamage(damage);
                    damagable.TakeDamage(new DamagePacket(damage, damageType, source));
                    triggerOnIDamageableOffload(other.gameObject);
                    Die();
                }
            }
        }
    }

    void Die()
    {
        // Debug.Log("Die method called");
        dying = true;

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;

        foreach ( GameObject go in disableOnDieStart)
        {
            if  (go is not null)
            {
                go.SetActive(false);
            }
        }

        StartCoroutine(DeathRoutine());

    }

    IEnumerator DeathRoutine()
    {
        // Debug.Log("Death routine started");
        yield return new WaitForSeconds(deathRoutineDuration);
        Destroy(gameObject);
    }

    void SpawnOffloadObjects()
    {
            // Debug.Log("spawning offload objects");
            if ( createOnOffloadTrigger is not null)
        {
            foreach (var obj in createOnOffloadTrigger)
            {
                if (obj is not null)
                {
                    GameObject.Instantiate<GameObject>(obj, transform.position, Quaternion.Euler(Vector3.up));
                }
            }
        }
    }
}}
