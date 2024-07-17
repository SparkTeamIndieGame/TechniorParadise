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

        public void Move(Vector3 direction)
        {
            _controller.SimpleMove(direction * _moveSpeed * Time.deltaTime);
        }

        public void Turn(Vector3 direction)
        {
            if (direction !=  Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                _transform.rotation = Quaternion.RotateTowards(_transform.rotation, toRotation, _turnSpeed * Time.deltaTime);
            }
        }
    }
}