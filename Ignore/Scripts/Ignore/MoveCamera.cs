using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;
    private void Start()
    {
        transform.position = cameraPosition.position;
    }

    private void LateUpdate()
    {

        transform.position = cameraPosition.position;
    }
}
/*
 -0.413 -0.028 0.08
 -0.45 -0.1 0
 -0.41 0.25 0.2
 
*/