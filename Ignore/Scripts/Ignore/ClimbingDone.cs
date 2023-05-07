using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingDone : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public PlayerMovementAdvanced pm;
    public LayerMask whatIsWall;

    [Header("Climbing")]
    public float climbSpeed;
    public float maxClimbTime;
    private float climbTimer;
    //

    private bool climbing;

    [Header("Vaulting")]
    public KeyCode vaultKey = KeyCode.W;
 

    [Header("Detection")]
    public float detectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle = 35f;
    private float wallLookAngle;

    private RaycastHit frontWallHit;
    private bool wallFront;

    private Transform lastWall;
    private Vector3 lastWallNormal;
    public float minWallNormalAngleChange;

    [Header("Exiting")]
    public bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    public Transform cam;

    [Header("Vaulting")]
    public float vaultDetectionLength;
    public bool topReached;
    public float vaultClimbSpeed;
    public float vaultJumpForwardForce;
    public float vaultJumpUpForce;
    public float vaultCooldown;

    bool midCheck;
    bool feetCheck;


    private void Update()
    {
        WallCheck();
        StateMachine();

        if (climbing && !exitingWall) ClimbingMovement();
    }

    private void StateMachine()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector2 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        if (wallFront && Input.GetKey(KeyCode.W) && wallLookAngle < maxWallLookAngle)
        {
            if (!climbing) StartClimbing();                        
            if (!wallFront)
            {
                StopClimbing();
            }
        }

        // State 4 - None
        else
        {
            if (climbing) StopClimbing();
        }


    }

    private void WallCheck()
    {
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, whatIsWall);
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

        bool newWall = frontWallHit.transform != lastWall || Mathf.Abs(Vector3.Angle(lastWallNormal, frontWallHit.normal)) > minWallNormalAngleChange;

        if ((wallFront && newWall) || pm.grounded)
        {
            climbTimer = maxClimbTime;
        }

        // vaulting
        midCheck = Physics.Raycast(transform.position, orientation.forward, vaultDetectionLength, whatIsWall);
        feetCheck = Physics.Raycast(transform.position + new Vector3(0, -0.9f, 0), orientation.forward, vaultDetectionLength, whatIsWall);
        topReached = feetCheck && !midCheck;
        if (topReached == true)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * 2f, ForceMode.Impulse);
            topReached = false;
            print("Подбросило");
        }



    }

    private void StartClimbing()
    {
        climbing = true;
        pm.climbing = true;

        lastWall = frontWallHit.transform;
        lastWallNormal = frontWallHit.normal;
        ///cam.DoShake(1, 1);
    }

    private void ClimbingMovement()
    {
        float speed = topReached ? vaultClimbSpeed : climbSpeed;
        rb.velocity = new Vector3(rb.velocity.x, speed, rb.velocity.z);
    }

    private void StopClimbing()
    {
        climbing = false;
        pm.climbing = false;
    }

}
