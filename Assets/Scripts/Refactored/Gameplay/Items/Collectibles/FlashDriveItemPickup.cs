using Spark.Refactored.Gameplay.Entities.Player.MVC;
using UnityEngine;

namespace Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Items
{
    public class FlashDriveItemPickup : MonoBehaviour, ICollectable
    {
        [SerializeField] protected float _turnSpeed = 180.0f;

        void FixedUpdate() => transform.Rotate(Vector3.up, _turnSpeed * Time.deltaTime);

        public void Activate(View player)
        {
            player.OnFlashDrivePickUped.Invoke();
            // todo: add visual effect here
            Deactivate();
        }

        public void Deactivate()
        {
            Destroy(gameObject);
        }
    }
}