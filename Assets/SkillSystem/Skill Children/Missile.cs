using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem{
public class Missile : MonoBehaviour
{
    public float speed;
    public float damage;
    public float maxTravelDistance;
    float distanceTraveled;
    public float MaxTravelTime;
    float timeAlive;

    public LayerMask collisionDamage;
    public LayerMask collisionDestroy;

    public GameObject targetObject;
    public Vector3 initialTarget;
    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(initialTarget);
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
}}
