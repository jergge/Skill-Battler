using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UniqueGravity : MonoBehaviour
{
    public Vector3 gravityDirectionInWorld;
    public float acceleration = 9.8f;
    public float buffDuration;

    Rigidbody rb;
    PlayerController pc;
    
    public bool onGround => pc.onGround;

    void Start() {
        if (!TryGetComponent<Rigidbody>(out rb))
            Destroy(this);

        rb.velocity = Vector3.zero;

        if (!TryGetComponent<PlayerController>(out pc))
            Destroy(this);

    }

    void Update() {

    }

    void FixedUpdate() {
        if(!onGround)
            rb.AddForce(gravityDirectionInWorld*acceleration, ForceMode.Acceleration);
    }

}
