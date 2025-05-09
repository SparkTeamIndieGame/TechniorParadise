using Spark.UI;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Spark.Refactored.Gameplay.Entities.Player.MVC;

namespace Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Items
{
    public class DetailsItemPickup : MonoBehaviour, ICollectable
    {
        [SerializeField] protected float _turnSpeed = 120.0f;
        [SerializeField, Range(1, 300)] protected int _maximum = 300;
        [SerializeField, Range(0, 300)] protected int _minimum = 0;

        void FixedUpdate() => transform.Rotate(Vector3.up, _turnSpeed * Time.deltaTime);

        public void Activate(View player)
        {
            player.OnDetailsPickUped(Random.Range(_minimum, _maximum));
            // todo: add visual effect here
            Deactivate();
        }

        public void Deactivate()
        {
            Destroy(gameObject);
        }
    }
}