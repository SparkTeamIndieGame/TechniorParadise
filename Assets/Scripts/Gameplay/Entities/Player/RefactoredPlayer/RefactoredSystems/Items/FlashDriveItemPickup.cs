using Spark.Gameplay.Entities.RefactoredPlayer;
using UnityEngine;

namespace Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Items
{
    public class FlashDriveItemPickup : MonoBehaviour, IPickupable
    {
        [SerializeField] protected float _turnSpeed = 180.0f;

        void FixedUpdate() => transform.Rotate(Vector3.up, _turnSpeed * Time.deltaTime);

        public void Activate(RefactoredPlayerView player)
        {
            player.OnFlashDrivePickUped.Invoke();
            // todo: add visual effect here
            Destroy(gameObject);
        }
    }
}