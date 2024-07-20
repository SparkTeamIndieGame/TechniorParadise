using Spark.Gameplay.Entities.Common.Data;
using Spark.Gameplay.Entities.Player.Abilities;
using System;
using UnityEngine;

namespace Spark.Gameplay.Entities.Player
{
    [Serializable]
    public class PlayerModel : IPlayer
    {
        #region Player events
        public event Action<float> OnHealthChanged;
        #endregion

        [SerializeField] private CharacterController _controller;
        [SerializeField] private Transform _transform;

        [SerializeField] private float _healthMax;
        [SerializeField] private float _health;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _turnSpeed;
        [SerializeField] private float _damage;

        [SerializeField] private PlayerFlashAbility _flashAbility;
        [SerializeField] private PlayerInvulnerAbility _invulnerAbility;

        public float FlashCooldown => _flashAbility.Cooldown;
        public float InvulnerCooldown => _invulnerAbility.Cooldown;

        public float HealthMax => _healthMax;
        public float Health
        {
            get => _health;
            private set
            {
                float points = value;

                if (points > 0) _health = Mathf.Min(Health + points, HealthMax);
                else if (points < 0)
                {
                    _health -= points;
                    if (_health <= 0) Die();
                }
            }
        }
        public bool IsAlive => Health > 0;

        public PlayerModel(CharacterController controller, Transform transform)
        {
            _controller = controller;
            _transform = transform;

            Health = HealthMax;

            _flashAbility = new PlayerFlashAbility(_controller, _transform);
            _invulnerAbility = new PlayerInvulnerAbility(this);
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

        public void UseFlashAbility() => _flashAbility.Use();
        public void UseInvulnerAbility() => _invulnerAbility.Use();
        
        public void UpdateAbilities()
        {
            _flashAbility.Update();
            _invulnerAbility.Update();
        }

        public void Heal(float points)
        {
            Health += points;
            // OnHealthChanged?.Invoke(Health);
        }

        public void TakeDamage(float points)
        {
            Health -= points;
            // OnHealthChanged?.Invoke(Health);
        }

        public void Die() => Debug.Log("You are dead!");

        public void Attack(IDamagable damagable) => damagable.TakeDamage(_damage);
    }
}