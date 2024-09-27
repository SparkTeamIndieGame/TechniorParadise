using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    private void Awake()
    {
        DirectionArrow.TargetNew = _target;
    }

    private void OnTriggerEnter(Collider other)
    {
        DirectionArrow.TargetNew = _target;
    }
}
