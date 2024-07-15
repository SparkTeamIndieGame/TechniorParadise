using System;
using UnityEngine;

namespace Spark.Gameplay.Entities.Player
{
    [Serializable]
    public class PlayerModel
    {
        #region Player events
        public event Action<float> OnHealthChanged;
        #endregion

        [SerializeField] private CharacterController _controller;
        [SerializeField] private Transform _transform;

        public float MaxHealth => 100.0f;
        public float Health { get; private set; }

        public PlayerModel(CharacterController controller, Transform transform)
        {
            _controller = controller;
            _transform = transform;

            Health = MaxHealth;
        }

    }
}