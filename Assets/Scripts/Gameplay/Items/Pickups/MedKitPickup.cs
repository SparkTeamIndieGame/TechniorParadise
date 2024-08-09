using Spark.Gameplay.Entities.Common.Abilities;
using Spark.Gameplay.Entities.Player;
using UnityEngine;

namespace Spark.Gameplay.Items.Pickupable
{
    public class MedKitPickup : MonoBehaviour, IPickupable
    {
        [SerializeField] protected float _turnSpeed = 240.0f;
        [SerializeField] PlayerController _playerController;

        void FixedUpdate() => transform.Rotate(Vector3.up, _turnSpeed * Time.deltaTime);

        public void Activate(PlayerModel player)
        {
            if (_playerController.PickUpMedKit())
            {
                // todo: add visual effect here
                Destroy(gameObject);
            }
        }
    }
}