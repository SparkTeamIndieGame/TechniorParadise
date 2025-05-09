using Spark.Gameplay.Entities.Common.Abilities;
using Spark.Gameplay.Entities.Player;
using Spark.Refactored.Gameplay.Entities.Player.MVC;
using UnityEngine;

namespace Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Items
{
    public class MedKitPickup : MonoBehaviour, ICollectable
    {
        [SerializeField] protected float _turnSpeed = 240.0f;

        void FixedUpdate() => transform.Rotate(Vector3.up, _turnSpeed * Time.deltaTime);

        public void Activate(View player)
        {
            player.OnMedKitPickUped.Invoke(this);
        }

        public void Deactivate()
        {
            Destroy(gameObject);
        }
    }
}