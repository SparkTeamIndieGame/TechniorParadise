using Spark.Gameplay.Entities.Player;
using UnityEngine;

namespace Spark.Gameplay.Items.Pickupable
{
    public class AidKitPickup : MonoBehaviour, IPickupable
    {
        [SerializeField] protected float _turnSpeed = 120.0f;
        [SerializeField] protected float _healPoints = 20.0f;

        void FixedUpdate() => transform.Rotate(Vector3.up, _turnSpeed * Time.deltaTime);

        public void Activate(PlayerModel player)
        {
            player.Heal(_healPoints);
            // todo: add visual effect here
            Destroy(gameObject);
        }
    }
}