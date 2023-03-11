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
    public float climbSpeed = 3f;
    public float maxClimbTime = 0.75f;
    private float climbTimer;

    public float climbJumpUpForce = 15f;
    public float climbJumpBackForce = 15f;

    private bool climbing;

    [Header("ClimbJumping")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode vaultKey = KeyCode.W;
    public int climbJumps;
    private int climbJumpsLeft;

    [Header("Detection")]
    public float detectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle = 30f;
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

    [Header("Ledge Grabbing")]
    public Transform cam;

    public float moveToLedgeSpeed;
    public float ledgeJumpForwardForce;
    public float ledgeJumpUpForce;
    public float maxLedgeJumpUpSpeed;
    public float maxLedgeGrabDistance;

    public float minTimeOnLedge;
    private float timeOnLedge;

    private bool holding;

    [Header("Ledge Detection")]
    public float ledgeDetectionLength;
    public float ledgeSphereCastRadius;
    public LayerMask whatIsLedge;

    private Transform lastLedge;
    public Transform currLedge;

    private RaycastHit ledgeHit;
    private Vector3 directionToLedge;
    private float distanceToLedge;

    [Header("Vaulting")]
    public float vaultDetectionLength;
    public bool topReached;
    public float vaultClimbSpeed;
    public float vaultJumpForwardForce;
    public float vaultJumpUpForce;
    public float vaultCooldown;

    bool readyToVault;
    bool vaultPerformed;
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

        // State 3 - Exiting
        if (exitingWall)
        {
            if (climbing) StopClimbing();

            if (exitWallTimer > 0) exitWallTimer -= Time.deltaTime;
            if (exitWallTimer < 0) exitingWall = false;
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
            climbJumpsLeft = climbJumps;
        }

        // vaulting
        if(Physics.Raycast(transform.position, orientation.forward, detectionLength, whatIsWall))
            print("raycastCheck done");

        midCheck = Physics.Raycast(transform.position, orientation.forward, vaultDetectionLength, whatIsWall);
        feetCheck = Physics.Raycast(transform.position + new Vector3(0, -0.9f, 0), orientation.forward, vaultDetectionLength, whatIsWall);

        topReached = feetCheck && !midCheck;

        // ledge detection
        bool ledgeDetected = Physics.SphereCast(transform.position, ledgeSphereCastRadius, cam.forward, out ledgeHit, ledgeDetectionLength, whatIsLedge);

        if (ledgeHit.transform == null) return;

        directionToLedge = ledgeHit.transform.position - transform.position;
        distanceToLedge = directionToLedge.magnitude;

        if (lastLedge != null && ledgeHit.transform == lastLedge) return;


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

    
 
    private void OnDrawGizmos()
    {
        if (currLedge == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(currLedge.position, 1f);
    }
}
