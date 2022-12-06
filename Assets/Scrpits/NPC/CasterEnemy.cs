using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using System;

[RequireComponent(typeof(Rigidbody))]
public class CasterEnemy : NPC
{
    public enum State {
        patrol,
        aware,
        attack
    }

    public float awarenessRaduis = 40;
    public float awarenessAngle = 180;

    public float moveSpeed = 5f;

    public float attackRange = 20;

    public float castInterval = 2f;

    Rigidbody rb;

    Vector3 moveDirecton;

    float timeOfLastCast;

    public List<LivingEntity> awareOf = new List<LivingEntity>();

    LivingEntity currentTarget;
    float distanceToCurrentTarget => Vector3.Distance(currentTarget.transform.position, transform.position);

    Skill skill;

    State state = State.patrol;
    // Start is called before the first frame update
    void Awake()
    {
        timeOfLastCast = Time.time;
        //skill = spellBook[0];
        rb = GetComponent<Rigidbody>();
        StartCoroutine(OnPatrol());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator OnPatrol()
    {
        state = State.patrol;
        
        while (true)
        {
            awareOf.Clear();
            foreach(LivingEntity le in GameObject.FindObjectsOfType<LivingEntity>())
            {
                awareOf.Add(le);

                if ( Vector3.Distance(transform.position, le.transform.position) < attackRange && Skill.IsValidTarget(gameObject, le.gameObject, Skill.ValidTargets.Enemies) && !le.IsDead() )
                {
                    currentTarget = le;
                    //Debug.Log(name + " will attack " + currentTarget.name);
                    StopAllCoroutines();
                    StartCoroutine(OnAttack());
                }
            }

            yield return null;
        }
    }

    IEnumerator OnAttack()
    {
        state = State.attack;
        
        while (true)
        {
            if((distanceToCurrentTarget <= attackRange) && ((timeOfLastCast + castInterval) < Time.time))
            {
                timeOfLastCast = Time.time;
                //Debug.Log("Trying to cast at " + currentTarget.name);
                //GetComponentsInChildren<Skill>()[0].Cast(skillManager.skillSpawnLocation, new TargetInfo(currentTarget.gameObject, distanceToCurrentTarget, currentTarget.transform.position), null);
                TargetInfo targetInfo = new TargetInfo(currentTarget.gameObject, distanceToCurrentTarget, currentTarget.NPCBodyTarget.position);
                skillManager.NPCUseSkill(skillManager.attack, targetInfo, true);
            } else if (distanceToCurrentTarget <= awarenessRaduis && distanceToCurrentTarget > attackRange)
            {
                //moveDirecton = (currentTarget.transform.position - transform.position).normalized;
            } else if (distanceToCurrentTarget > awarenessRaduis) 
            {
                moveDirecton = Vector3.zero;
                StopAllCoroutines();
                StartCoroutine(OnPatrol());
            }
        //Debug.Log("attacking");
        yield return null;
        }
    }

    void FixedUpdate()
    {
        Vector3 newPosition = rb.position + transform.TransformDirection(moveDirecton * Time.fixedDeltaTime * moveSpeed);
        rb.MovePosition(newPosition);
    }
}
