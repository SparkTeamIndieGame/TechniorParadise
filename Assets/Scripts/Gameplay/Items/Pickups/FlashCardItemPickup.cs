using Spark.Gameplay.Entities.Player;
using UnityEngine;

namespace Spark.Gameplay.Items.Pickupable
{
    public class FlashCardItemPickup : MonoBehaviour, IPickupable
    {
        [SerializeField] protected float _turnSpeed = 180.0f;

        void FixedUpdate() => transform.Rotate(Vector3.up, _turnSpeed * Time.deltaTime);

        public void Activate(PlayerModel player)
        {
            player.FlashCard.Add();
            // todo: add visual effect here
            Destroy(gameObject);
        }
    }
}