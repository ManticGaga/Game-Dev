using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyRotate : MonoBehaviour
{
    public Transform fingerRotate;
    private void Start()
    {
        transform.rotation = fingerRotate.rotation;
    }   
}
