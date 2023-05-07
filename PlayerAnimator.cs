using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator cameraAnimator;
    public PlayerMovementAdvanced pm;
    public PlayerCombat pc;
    

    private void Update()
    {

        cameraAnimator.SetBool(pm.previousDirState, false);
        cameraAnimator.SetBool(pm.dirstring, true);        
        cameraAnimator.SetBool(pm.previousMovementState, false);
        cameraAnimator.SetBool(pm.statestring, true);


    }

}
