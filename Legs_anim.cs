using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legs_anim : MonoBehaviour
{

    public Animator playerModelAnimator;
    public Animator legAnimator;
    public PlayerMovementAdvanced pm;
    public PlayerCombat pc;
    public Transform playerOrientation;
  
    void Update()
    {


        /*if (pc.taekwondo)
        {
            // Get the current rotation and add an offset angle
            Quaternion newRotation = playerOrientation.rotation * Quaternion.Euler(0f, 90f, 0f);

            // Assign the new rotation to the transform
            transform.rotation = newRotation;
        }
        transform.rotation = playerOrientation.rotation;
        */

    }
}