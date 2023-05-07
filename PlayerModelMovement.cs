using System.Collections;
using System.Collections.Generic;
using UnityEngine;
          
public class PlayerModelMovement : MonoBehaviour
{

    public Animator playerModelAnimator;
    public Animator legAnimator;
    public PlayerMovementAdvanced pm;
    public PlayerCam pc;
    public Transform playerOrientation;

    void Start() 
    {
    }

    void Update()
    {
        AnimatorStateInfo currentState = playerModelAnimator.GetCurrentAnimatorStateInfo(0);
        transform.rotation = playerOrientation.rotation;        
        playerModelAnimator.SetBool(pm.previousDirState, false);
        playerModelAnimator.SetBool(pm.dirstring, true);
        
        playerModelAnimator.SetFloat("Speed", pm.animovespeed);
        
        playerModelAnimator.SetBool(pm.previousMovementState, false);
        playerModelAnimator.SetBool(pm.statestring, true);
        if(currentState.IsName("High_Kick"))
        {
            legAnimator.SetTrigger("Taekwondo");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerModelAnimator.Play("Boxing");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerModelAnimator.Play("Taekwondo");            
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerModelAnimator.Play("Judo");           
        }
    }
}   