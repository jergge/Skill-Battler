using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput), typeof(Player))]
public class PlayerController : MonoBehaviour
{
    public float maxCameraAngleUp = 90;
    public float maxCameraAngleDown = 80;

    /// <summary>
    /// How many units moved per second
    /// </summary>
    public float moveSpeed = 6;

    /// <summary>
    /// Is the sprinting action active?
    /// </summary>
    bool sprinting;

    /// <summary>
    /// How many units moved per second if [sprinting] = true
    /// </summary>
    public float sprintSpeed = 10;

    /// <summary>
    /// Changes how fast the camera pans on the X axis
    /// </summary>
    public float turnSpeed = 300;

    /// <summary>
    /// Prevents the camera from panning
    /// </summary>
    bool panLock = false;
    public void LockPan()
    {
        panLock = true;
    }
    public void UnlockPan()
    {
        panLock = false;
    }

    /// <summary>
    /// Changes how fast the camera tilts on the Y axis
    /// </summary>
    public float lookVerticalSpeed = 10;

    /// <summary>
    /// Prevents the camera from tilting
    /// </summary>
    bool tiltLock = false;
    public void LockTilt()
    {
        tiltLock = true;
    }
    public void UnlockTilt()
    {
        tiltLock = false;
    }

    /// <summary>
    /// Force to add to Y direction when the jump event is fired
    /// </summary>
    public float jumpForce = 5;

    /// <summary>
    /// How many jumps can be made between landings
    /// </summary>
    public int numberOfJumps = 2;

    /// <summary>
    /// Tracks how many jumps have been made without landing
    /// </summary>
    public int jumpCount = 0;

    /// <summary>
    /// small value for small offsets
    /// </summary>
    float eps = 0.0001f;

    /// <summary>
    /// The layers which count as ground for resetting the jump count
    /// </summary>
    public LayerMask groundedLayers;

    public event Action<CheckForAny> CanIJump;
    public event Action<Vector3> OnMultiJump;

    public RaycastHit groundHit;
    public bool onGround = true;

    public Transform groundedCheckTransform;
    public float checkRise = 0.1f;

    /// <summary>
    /// The Rigidbody that will perform the movement actions
    /// </summary>
    Rigidbody rb;

    /// <summary>
    /// The inputs from PlayerInput for the OnMove event
    /// </summary>
    Vector3 moveInputs;

    /// <summary>
    /// The Inputs from PlayerInput for the OnLook event
    /// </summary>
    Vector2 lookInputs;

    /// <summary>
    /// Set by OnMove and used in FixedUpdate
    /// </summary>
    Vector3 currentMoveDirection;

    /// <summary>
    /// Checked in Update then speed is used in FixedUpdate
    /// </summary>
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

        Physics.Raycast(groundedCheckTransform.position, transform.up*-1, out groundHit, 30f, groundedLayers);
        Debug.DrawRay(transform.position, transform.up*-10, Color.white);
        onGround = (groundHit.distance <= checkRise + .05) ? true : false;
    }
    
    /// <summary>
    /// Event fired when PlayerInput sends movement data
    /// </summary>
    /// <param name="value">The PlayerInput data sent through the event</param>
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
//        Debug.Log("trying to jump");
        if (onGround)
        {
            //jumpCount = 0;
            rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
        }

        // //TODO this doesn't work with the new inputs?~?~?!?
        // if ((jumpCount < numberOfJumps-1))
        // {
        // CheckForAny checker = new CheckForAny(false);
        //     if(CanIJump != null)
        //     {
        //         //Debug.Log("about to run the subbed methods");
        //         CanIJump(checker);
        //     }

        //     if ( checker.NotFound() )
        //         rb.AddForce(transform.up*jumpForce, ForceMode.VelocityChange);
        //         if( !onGround && OnMultiJump!=null)
        //         {
        //             OnMultiJump(moveInputsLocal);
        //         }
        //     jumpCount++;
    }
}
