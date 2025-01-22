using Spark.Gameplay.Entities.Common.Abilities;
using Spark.Gameplay.Entities.Player;
using Spark.Gameplay.Entities.RefactoredPlayer;
using UnityEngine;

namespace Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Items
{
    public class MedKitPickup : MonoBehaviour, IPickupable
    {
        [SerializeField] protected float _turnSpeed = 240.0f;

        void FixedUpdate() => transform.Rotate(Vector3.up, _turnSpeed * Time.deltaTime);

        public void Activate(RefactoredPlayerView player)
        {
            player.OnMedKitPickUped.Invoke(this);
        }

        public void Deactivate()
        {
            Destroy(gameObject);
        }
    }
}