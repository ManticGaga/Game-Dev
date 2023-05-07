using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelRotate : MonoBehaviour
{
    public Transform Bip01;
      
    void Update()
    {
        transform.rotation = Bip01.rotation;
    }
}
