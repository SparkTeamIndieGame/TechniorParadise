using Spark.Gameplay.Entities.Player;
using Spark.UI;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace Spark.Gameplay.Items.Pickupable
{
    public class DropItemPickup : MonoBehaviour, IPickupable
    {
        
        [SerializeField] protected float _turnSpeed = 120.0f;
        [SerializeField, Range(1, 300)] protected int _maxDetails = 300;
        [SerializeField, Range(0, 300)] protected int _minDetails = 0;

        void FixedUpdate() => transform.Rotate(Vector3.up, _turnSpeed * Time.deltaTime);

        public void Activate(PlayerModel player)
        {
            player.Details += Random.Range(_minDetails, _maxDetails);
            Debug.Log("Now u have: " + player.Details);
            
            // todo: add visual effect here
            Destroy(gameObject);
        }
    }
}