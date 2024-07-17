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

        [SerializeField] public float _health;
        [SerializeField] public float _moveSpeed;
        [SerializeField] public float _turnSpeed;

        public float MaxHealth => 100.0f;
        public float Health { get; private set; }

        public PlayerModel(CharacterController controller, Transform transform)
        {
            _controller = controller;
            _transform = transform;

            Health = MaxHealth;
        }

        public void Move(float axis)
        {
            _controller.SimpleMove(_transform.forward * axis * _moveSpeed * Time.deltaTime);
        }

        public void Turn(float axis)
        {
            _transform.Rotate(.0f, axis * _turnSpeed * Time.deltaTime, .0f);
        }
    }
}