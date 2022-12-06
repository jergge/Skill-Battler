using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetPrefab : LivingEntity
{
    
    public GameObject master;
    public float currentMoveSpeed;
    public float maxSpeedWhileTurning;
    public float turnSpeed = 1;
    Vector3 currentMoveDirectionTarget;
    Vector3 currentMoveDirection;
    public float masterFollowRadius = 5f;
    public Rigidbody rb;
    public DPadMap dPadMap;
    float distanceToMaster => Vector3.Distance(transform.position, master.transform.position);
    public enum PetState
    {
        follow,
        stay,
        explore,
        attack,
        dead
    }

    public DPadMap GetDPadMap()
    {
        return dPadMap;
    }

    PetState petState = PetState.follow;


    new void Start()
    {
        base.Start();
        StartCoroutine(Follow());
    }

    void Update()
    {
        TurnToFace();
        animator.SetFloat("moveSpeed", currentMoveSpeed);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.forward * currentMoveSpeed * Time.fixedDeltaTime);

    }

    IEnumerator Follow()
    {
        Debug.Log("now following " + master.name);
        while(true)
        {
            // Debug.Log(distanceToMaster);
            if (distanceToMaster <= masterFollowRadius)
            {
                currentMoveSpeed = 0;
                // Debug.Log("stopping");
            } else {
            // Debug.Log("starting up again ");
            currentMoveSpeed = 3;

            }
            //currentMoveDirectionTarget = master.transform.position.normalized;

            yield return null;
        }
    }

    IEnumerator Stay()
    {
        Debug.Log("now staying - what a good girl!");
        currentMoveSpeed = 0;
        yield return new WaitForSeconds(3);
    }

    public void TurnToFace()
    {
        transform.LookAt(new Vector3(master.transform.position.x, transform.position.y, master.transform.position.z));

    }

    public void OnStay()
    {
        StopAllCoroutines();
        StartCoroutine(Stay());
    }

    public void OnFollow()
    {
        StopAllCoroutines();
        StartCoroutine(Follow());
    }

}
