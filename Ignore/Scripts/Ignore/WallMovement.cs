using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WallMovement : MonoBehaviour
{

    [Serializable]
    public class WallRunningData
    {
        [Header("Wall Running")]
        public float wallRunForce;
        public float wallRunJumpUpForce;
        public float wallRunJumpSideForce;
        public float pushToWallForce;
        public float maxWallRunTime;

        [Header("Gravity")]
        public bool useGravity;
        public float customGravity;
        public float yDrossleSpeed;
    }
}