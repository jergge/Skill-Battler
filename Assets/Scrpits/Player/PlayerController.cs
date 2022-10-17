using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput), typeof(Player))]
public class PlayerController : MonoBehaviour
{
    public float maxCameraAngleUp = 90;
    public float maxCameraAngleDown = 80;

    public float moveSpeed = 6;
    bool sprinting;
    public float sprintSpeed = 10;

    public float turnSpeed = 300;
    bool panLock = false;
    public void LockPan()
    {
        panLock = true;
    }
    public void UnlockPan()
    {
        panLock = false;
    }

    public float lookVerticalSpeed = 10;
    bool tiltLock = false;
    public void LockTilt()
    {
        tiltLock = true;
    }
    public void UnlockTilt()
    {
        tiltLock = false;
    }

    public float jumpForce = 5;
    public int numberOfJumps = 2;
    public int jumpCount = 0;

    float eps = 0.0001f;

    public LayerMask groundedLayers;

    public event Action<CheckForAny> CanIJump;
    public event Action<Vector3> OnMultiJump;

    public RaycastHit groundHit;
    public bool onGround => (groundHit.distance < transform.localScale.y + eps);

    Rigidbody rb;
    Vector3 moveInputs;
    Vector2 lookInputs;

    Vector3 currentMoveDirection;
    float currentMoveSpeed;

    public Camera playerCamera;

    Vector3 moveInputsNormal => moveInputs.normalized;
    Vector3 moveInputsLocal => transform.TransformDirection(moveInputsNormal);

    void Start()
    {
       rb = GetComponent<Rigidbody>(); 
       Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {

        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0,lookInputs.x * turnSpeed,0)* Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);

        
        Vector3 currentMoveVelocity = currentMoveDirection * currentMoveSpeed;
        Vector3 newPosition = rb.position + transform.TransformDirection(currentMoveVelocity * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
    }

    void Update()
    {
        
        if(sprinting)
        {
            currentMoveSpeed = sprintSpeed;
        } else {
            currentMoveSpeed = moveSpeed;
        }

        if(currentMoveDirection == Vector3.zero)
        {
            currentMoveSpeed = 0;
        } 
        GetComponent<Player>().animator.SetFloat("moveSpeed", currentMoveSpeed);
    }
    
    
    void OnMove(InputValue value)
    {
        moveInputs.x = value.Get<Vector2>().x;
        moveInputs.z = value.Get<Vector2>().y;
        currentMoveDirection = moveInputs;
        //Debug.Log(currentMoveDirection); 
    }

    void OnSprint(InputValue value)
    {
        sprinting = value.isPressed; 
    }

    void OnLook(InputValue value)
    {
        lookInputs.x = value.Get<Vector2>().x;
        lookInputs.y = value.Get<Vector2>().y;
        if(!tiltLock)
        {
        Quaternion cameraRotation = Quaternion.Euler(lookInputs.y * -lookVerticalSpeed, 0, 0);
        playerCamera.transform.rotation *= cameraRotation;
        }
    }

    void OnJump()
    {
        //check to see if on ground;
        Physics.Raycast(transform.position, transform.up*-1, out groundHit, 30f, groundedLayers);
        Debug.DrawRay(transform.position, transform.up*-10, Color.white);

        if(onGround)
            jumpCount = 0;
        
        //TODO this doesn't work with the new inputs?~?~?!?
        if ((jumpCount < numberOfJumps-1))
        {
        CheckForAny checker = new CheckForAny(false);
            if(CanIJump != null)
            {
                //Debug.Log("about to run the subbed methods");
                CanIJump(checker);
            }

            if ( checker.NotFound() )
                rb.AddForce(transform.up*jumpForce, ForceMode.VelocityChange);
                if( !onGround && OnMultiJump!=null)
                {
                    OnMultiJump(moveInputsLocal);
                }
            jumpCount++;
        }
    }
}
