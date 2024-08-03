using Spark.Gameplay.Entities.Common.Data;
using Spark.Gameplay.Entities.Common.Abilities;
using Spark.Gameplay.Weapons;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Spark.Gameplay.Entities.Enemies;

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

        [SerializeField, Min(10.0f)] private float _healthMax;
        [SerializeField, Min(0.0f)] private float _health;
        [SerializeField] private float _moveSpeed;
        [SerializeField, Range(0.0f, 2.0f)] private float _turnSpeed;

        [SerializeField] private FlashAbility _flashAbility;
        [SerializeField] private InvulnerAbility _invulnerAbility;

        [SerializeField] private MeleeWeapon[] _meleeWeapons;
        [SerializeField] private RangedWeapon[] _rangedWeapons;
        [SerializeField] private Weapon _activeWeapon;
        
        [SerializeField] private Transform _target;

        private int _currentMeleeWeapon;
        private int _currentRangedWeapon;

        public float FlashCooldown => _flashAbility.Cooldown;
        public float InvulnerCooldown => _invulnerAbility.Cooldown;

        public float MaxHealth => _healthMax;
        public float Health
        {
            get => _health;
            private set => _health = value;
        }
        public bool IsAlive => Health > 0;

        public bool HasTarget => _target != null;

        private PlayerModel()
        {
            _healthMax = 100.0f;
            _moveSpeed = 100.0f;
            _turnSpeed = 1.0f;

            Health = MaxHealth;
        }

        public PlayerModel(
            PlayerView playerView, 
            CharacterController controller, 
            Transform transform) : this()
        {
            _flashAbility = new FlashAbility(_controller, _transform);
            _invulnerAbility = new InvulnerAbility(playerView);

            _controller = controller;
            _transform = transform;
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

        public void UseFlashAbility() => _flashAbility.Use();
        public void UseInvulnerAbility() => _invulnerAbility.Use();
        
        public void Update()
        {
            _flashAbility.Update();
            _invulnerAbility.Update();

            (_activeWeapon as RangedWeapon)?.Update();
        }

        public void Heal(float points)
        {
            Health = Mathf.Min(Health + points, MaxHealth);
            OnHealthChanged?.Invoke(Health);
        }

        public void TakeDamage(float points)
        {
            Health -= points;
            OnHealthChanged?.Invoke(Health);

            if (Health <= 0) Die();
        }

        public void Die() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        public void Attack()
        {
            if (_activeWeapon is RangedWeapon)
            {
                var weapon = _activeWeapon as RangedWeapon;
                if (weapon.IsReloading) return;

                if (weapon.HasAmmo)
                    weapon.Shoot();
            }
            else if (_activeWeapon is MeleeWeapon melee)
            {
                melee.TakeSwing();
            }
        }
        public void Attack(IDamagable damagable) => Attack();
        public void Attack(IDamagable damagable, float damage) => Attack();

        public void ReloadWeapon() => (_activeWeapon as RangedWeapon)?.Reload();

        public void SwitchWeapon()
        {
            if (_activeWeapon is MeleeWeapon)
            {
                _currentMeleeWeapon = (_currentMeleeWeapon + 1) % _meleeWeapons.Length;
                _activeWeapon = _meleeWeapons[_currentMeleeWeapon];
                
            }
            else if (_activeWeapon is RangedWeapon)
            {
                _currentRangedWeapon = (_currentRangedWeapon + 1) % _rangedWeapons.Length;
                _activeWeapon = _rangedWeapons[_currentRangedWeapon];
            }
        }
        public void SwitchWeaponType()
        {
            _activeWeapon =
                _activeWeapon == _meleeWeapons[_currentMeleeWeapon]
                    ? _rangedWeapons[_currentRangedWeapon] : _meleeWeapons[_currentMeleeWeapon];
        }

        public Weapon GetActiveWeapon() => _activeWeapon;

        public int GetCurrentMeleeWeapon() => _currentMeleeWeapon;
        public int GetCurrentRangedWeapon() => _currentRangedWeapon;

        public void SetTarget(Transform target) => _target = target;
        public void ResetTarget() => _target = null;
    }
}