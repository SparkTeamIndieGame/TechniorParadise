using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseDM : MonoBehaviour
{
    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        transform.rotation = initialRotation;
    }
}
