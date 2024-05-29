using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using Project.Weapons;

public class PlayerMovement: NetworkBehaviour
{
    [Header("Movement")]
    [SerializeField]private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float climbSpeed;
    public float speedReductionWhenAttacking;
    private float attackMoveMultiplier;


    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;
    


    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    public PhysicMaterial PlayerMat;


    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode crouchKey = KeyCode.LeftAlt;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;
    public bool climbing;
    public bool attacking;
    public bool landed;
    public float relLandingVel;
    bool exitingslope;

    [Header("Slope handling")]
    public float maxSlopeAngle;
    RaycastHit slopehit;

    public Transform orientation;

    public float horizontalInput;
    public float verticalInput;

    Vector3 moveDirection;

    public Rigidbody rb;

    public MovementState state;
    public MoveCam MC;
    public float AttackVelocity;
    public WeaponHolder WeaponManager;
    Animator ani;


    public enum MovementState
    {
        walking,
        sprinting,
        air,
        crouching,
        climbing,
    }

    private void AnimHandler()
    {
        if(!IsOwner)
        {
            return;
        }

        ani.SetFloat("Vel X", Vector3.Dot(orientation.right, rb.velocity));
        ani.SetFloat("Vel Z", Vector3.Dot(rb.velocity, orientation.forward));

        


    }


    private void Statehandler()
    {
        
        //Climbing
        if (climbing)
        {
            state = MovementState.climbing;
            moveSpeed = climbSpeed;
        }

        //Crouching
        else if(Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        //Sprinting
        else if (grounded && Input.GetKey(sprintKey))
        {
           
            state = MovementState.sprinting;
            
            moveSpeed = sprintSpeed;
        }


        //Walking
        else if(grounded)
        {
            
            
            state = MovementState.walking;
            
            moveSpeed = walkSpeed*attackMoveMultiplier;
        }
        
        //Idle
        
        //Air
        else
        {
            state = MovementState.air;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            landed = true;
            relLandingVel = collision.relativeVelocity.y;
            Invoke("resetLanded", 0.3f);
        }
        else landed = false;
    }

    public void resetLanded()
    {
        landed = false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startYScale = transform.localScale.y;
        ani = GetComponentInChildren<Animator>();
        moveSpeed = walkSpeed;
        readyToJump = true;
        WeaponManager = Camera.main.GetComponentInChildren<WeaponHolder>();
    }

    public float SphereRadius;
    private void Update()
    {
        //Debug.Log(rb.velocity.magnitude);

        AnimHandler();
        Statehandler();
        // ground check
        grounded = Physics.SphereCast(transform.position, SphereRadius, Vector3.down, out RaycastHit floor , playerHeight * 0.5f + 0.3f, whatIsGround);  

        MyInput();
        SpeedControl();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        if(!IsOwner)
        {
            return;
        }
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetKeyDown(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if(!IsOwner)
        {
            return;
        }

        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5, ForceMode.Impulse);
        }

        if (Input.GetKeyUp(crouchKey) || state != MovementState.crouching)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);

        }

        if (WeaponManager.anim.GetBool("Secondary"))
        {
            attacking = true;
            attackMoveMultiplier = speedReductionWhenAttacking;
        }
        else
        {
            attackMoveMultiplier = 1;
            attacking = false;
        }
        
    }

    private void MovePlayer()
    {

        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(OnSlope() && !exitingslope)
        {
            rb.AddForce(GetSlopeMoveDir() * moveSpeed * 7f, ForceMode.Force);
        }

        // on ground
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        //rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        if(OnSlope() && !exitingslope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        if(!IsOwner)
        {
            return;
        }
        exitingslope = true;
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
        exitingslope = false;
    }
    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down ,  out slopehit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopehit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDir()
    {
        return  Vector3.ProjectOnPlane(moveDirection, slopehit.normal).normalized;
    }

}