using Spark.Gameplay.Entities.Common.Data;
using Spark.Gameplay.Entities.Common.Abilities;
using Spark.Gameplay.Weapons;
using System;
using UnityEngine;
using Spark.Gameplay.Entities.Enemies;

namespace Spark.Gameplay.Entities.Enemies.Bosses.Crab
{
    [Serializable]
    public class CrabModel : IEnemy
    {
        #region C.R.A.B. events
        public event Action<IDamagable> OnHealthChanged;
        #endregion

        [SerializeField] private CharacterController _controller;
        [SerializeField] private Transform _transform;

        [SerializeField, Min(10.0f)] private float _healthMax;
        [SerializeField, Min(0.0f)] private float _health;
        [SerializeField] private float _moveSpeed;
        [SerializeField, Range(0.0f, 2.0f)] private float _turnSpeed;
                
        [SerializeField] private Transform _target;

        public float MaxHealth => _healthMax;
        public float Health
        {
            get => _health;
            private set => _health = value;
        }
        public bool IsAlive => Health > 0;

        public bool HasTarget => _target != null;

        private CrabModel()
        {
            _healthMax = 100.0f;
            _moveSpeed = 100.0f;
            _turnSpeed = 1.0f;

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
                _transform.rotation = Quaternion.RotateTowards(_transform.rotation, toRotation, _turnSpeed * Time.deltaTime * 360.0f);
            }
        }

        public void LookAtTarget()
        {
            _transform.LookAt(_target);
        }

        public void Heal(float points)
        {
            Health = Mathf.Min(Health + points, MaxHealth);
            OnHealthChanged?.Invoke(this);
        }

        public void TakeDamage(float points)
        {
            Health -= points;
            OnHealthChanged?.Invoke(this);

            if (Health <= 0) Die();
        }

        public void Die() => Debug.Log("The C.R.A.B. died!");

        public void Attack()
        {

        }
    }
}