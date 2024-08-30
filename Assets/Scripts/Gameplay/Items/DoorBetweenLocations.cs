using System;
using System.Collections;
using System.Collections.Generic;
using Spark.Gameplay.Entities.Player;
using UnityEngine;

public class DoorBetweenLocations : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private int _needFlashDrivers;
    [SerializeField] private Animator _doorAnimator;
    private void OnTriggerEnter(Collider other)
    {
        if (_player.FlashCard.Count == _needFlashDrivers)
        {
            _doorAnimator.SetTrigger("OpenDoors");
            _player.FlashCard.Reset();
            this.gameObject.GetComponent<BoxCollider>().enabled = false;

        }
    }
}
