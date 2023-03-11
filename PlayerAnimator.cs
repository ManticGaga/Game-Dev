using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator cameraAnimator;
    public PlayerMovementAdvanced pm;
    private string state;
    private void Update()
    {
        state = pm.state.ToString();
        cameraAnimator.SetTrigger(state);
    }

}
