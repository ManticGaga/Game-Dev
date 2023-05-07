using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovementAdvanced : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    [Header("MovementSpeed")]
    public float walkSpeed;
    public float sprintSpeed;
    public float forwardSpeed;
    public float sideSpeed;
    public float backSpeed;
    public float climbSpeed;
    public float vaultSpeed;
    public float airMinSpeed;
    public float maxYSpeed;
    public float jumpForce;


    [Header("States")]
    public float currentspeed;
    public float animovespeed;   
    public string previousMovementState;
    public string previousDirState;
    public string statestring;
    public string dirstring;
    
    [Header("Cooldowns")]
    private float dodgedelay = 0.4f;
    private float dodgecd = 0.3f;
    private bool readyToDodge;


    


    public float groundDrag;
    
    [Header("Stamina")]
    public float stamina; // The player's starting stamina
    public float maxStamina = 100f; // The player's maximum stamina
    public float staminaDepletionRate = 10f; // How quickly the player's stamina depletes when running
    public float staminaRecoveryRate = 5f; // How quickly the player's stamina recovers when not running
    public float staminaRecoveryDelay = 1f; // How long the player has to wait after stopping running before stamina starts recovering
    private float staminaRecoveryTimer = 0f; // Timer used to delay stamina recovery

    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode backKey = KeyCode.S;
    public KeyCode attackKey = KeyCode.Mouse0;
    public KeyCode blockKey = KeyCode.Mouse1;


    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode dodgeKey = KeyCode.Space;



    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;



    [Header("References")]
    public PlayerCam playerCam;
    private ClimbingDone climbingScriptDone;
    public Transform orientation;    
    public PlayerModelMovement pmm;
    public MoveCamera moveCamera;


    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;
    public MovementDirection moveDir;

    public enum MovementDirection
    { 
        forward,
        backward,
        left,
        right
    }
    public bool left;
    public bool right;
    public bool backward;
    public bool forward;

    public enum MovementState
    {
        standing,       
        walking,
        sprinting,
        climbing,
        dodge,
        crouching,
        air,
        restricted
    }
    public bool standing;
    public bool walking;
    public bool sprinting;

    public bool crouching;
    public bool dodge;
    public bool wallrunning;
    public bool climbing;
    public bool restricted;

    public TextMeshProUGUI text_speed;
    public TextMeshProUGUI text_mode;

    private void Start()
    {
        climbingScriptDone = GetComponent<ClimbingDone>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToDodge = true;
        stamina = 100f;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        MyInput();
        SpeedControl();
        StateHandler();
        DirectionHandler();
        TextStuff();
        statestring = state.ToString();
        dirstring = moveDir.ToString();
        // handle drag
        if (state == MovementState.walking || state == MovementState.sprinting || state == MovementState.crouching || state == MovementState.standing)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
        // If the player is not running or has no more stamina, start recovering stamina
        if (!sprinting || stamina <= 0f || staminaRecoveryTimer > 0f)
        {
            stamina += staminaRecoveryRate * Time.deltaTime;
        }

        // Clamp the stamina to prevent it from going over the maximum or below 0
        stamina = Mathf.Clamp(stamina, 0f, maxStamina);
    }

    private void FixedUpdate()
    {
        if (state == MovementState.walking || state == MovementState.sprinting || state == MovementState.crouching || state == MovementState.standing)
        {
            MovePlayer();
        }
        
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");    

        // start crouch
        if (Input.GetKeyDown(crouchKey) && (state != MovementState.air))
        {
            crouching = true;
        }      
            // stop crouch
            if (Input.GetKeyUp(crouchKey))
        {

            crouching = false;
        }

        // Dodge
        if (Input.GetKey(dodgeKey) && readyToDodge && grounded)
        {
            dodge = true;
            Invoke(nameof(Dodge), 0.2f);
        }
    }


    bool keepMomentum;
    private void DirectionHandler()
    {
        // Dir - Forward    
        if (Input.GetKey(forwardKey))
        {
            PreviousDir();
            moveDir = MovementDirection.forward;
            desiredMoveSpeed = forwardSpeed;                                            
        }
        // Dir - Left    
        if (Input.GetKey(leftKey))
        {
            PreviousDir();
            moveDir = MovementDirection.left;
            desiredMoveSpeed = sideSpeed;
        }
        // Dir - Right
        if (Input.GetKey(rightKey))
        {
            PreviousDir();
            moveDir = MovementDirection.right;
            desiredMoveSpeed = sideSpeed;
        }
        // Dir - Backward
        if (Input.GetKey(backKey))
        {
            PreviousDir();
            moveDir = MovementDirection.backward;
            desiredMoveSpeed = backSpeed;
        }
    }

    private void StateHandler()
    {
                
        // Mode - Climbing
        if (climbing)
        {
            PreviousState();
            state = MovementState.climbing;
            desiredMoveSpeed = climbSpeed;
        }

        // Mode - Crouching
        else if (crouching)
        {
            
            PreviousState();
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }

        // Mode - Dodge
        else if (dodge)
        {

            PreviousState();
            state = MovementState.dodge;            
        }

        // Mode - Standing
        else if (grounded && (float)currentspeed < 0.1)
        {
            PreviousState();
            state = MovementState.standing;
        }

        // Mode - Sprinting
        else if (grounded && Input.GetKey(sprintKey) && Input.GetKey(KeyCode.W) && stamina > 0f)
        {
            desiredMoveSpeed = sprintSpeed;
            stamina -= staminaDepletionRate * Time.deltaTime;
            staminaRecoveryTimer = staminaRecoveryDelay;
            Debug.Log(stamina);
            PreviousState();
            state = MovementState.sprinting;            
        }
        // Mode - Walking
        else if (grounded && state != MovementState.climbing && (float)currentspeed > 0.1)
        {
            PreviousState();
            state = MovementState.walking;
        }
       
        // Mode - Air
        else
        {
            PreviousState();
            state = MovementState.air;

            if (moveSpeed < airMinSpeed)
                desiredMoveSpeed = airMinSpeed;
        
        }
        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;

        if (desiredMoveSpeedHasChanged)
        {
            if (keepMomentum)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                moveSpeed = desiredMoveSpeed;
            }
        }
       
           

        // deactivate keepMomentum
        if (Mathf.Abs(desiredMoveSpeed - moveSpeed) < 0.1f) keepMomentum = false;
    }
    private void PreviousDir()
    {
        if (previousDirState != dirstring)
        {
            previousDirState = dirstring;
            //-27.7 16.8 7
        }
    }

    private void PreviousState()
    {
        if (previousMovementState != statestring)
        {
            previousMovementState = statestring;
        }
    }
    private void Dodge()
    {
        desiredMoveSpeed = 0;
        readyToDodge = false;        
        rb.AddForce(rb.position + moveDirection.normalized*300f);
        dodge = false;
        Invoke(nameof(ResetDodge), 1.2f);
    }


    private float speedChangeFactor;
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 1.5f;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        if (climbingScriptDone.exitingWall) return;
        if (restricted) return;

        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        // on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
}

    private void SpeedControl()
    {
    // limiting speed on ground or in air                
    Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

    // limit velocity if needed
    
        if (flatVel.magnitude > moveSpeed)
    {
        Vector3 limitedVel = flatVel.normalized * moveSpeed;
        rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
    }        
        // limit y vel
        if (maxYSpeed != 0 && rb.velocity.y > maxYSpeed)
            rb.velocity = new Vector3(rb.velocity.x, maxYSpeed, rb.velocity.z);
    }


    private void ResetDodge()
    {
        readyToDodge = true;
        Debug.Log(dodge);
    }

    private void TextStuff()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        currentspeed = Round(flatVel.magnitude, 1);
        animovespeed = (float)(currentspeed * 0.3);
        text_speed.SetText("Speed: " + currentspeed + " / " + Round(moveSpeed, 1));
        text_mode.SetText(statestring);
        //text_mode.SetText(moveDir.ToString());

    }

    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }
}
