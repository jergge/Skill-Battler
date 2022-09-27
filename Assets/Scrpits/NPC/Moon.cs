using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Moon : MonoBehaviour
{
    public Vector3 rotationPoint = Vector3.zero;
    public float speed;
    Rigidbody rb;
    Vector3 forceDirection;
    
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        //rb.AddForce(Vector3.one);
    }
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        //forceDirection = new Vector3(Mathf.Sin(Time.time), 0, Mathf.Cos(Time.time));
        //rb.AddRelativeForce(forceDirection);
    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawLine(transform.position, transform.position + Vector3.Scale(forceDirection, new Vector3(4,4,4)));
    }
}
