using Spark.Gameplay.Entities.Common.Data;
using Spark.Gameplay.Entities.Common.Abilities;
using System;
using UnityEngine;
using Spark.Gameplay.Weapons;

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

        private int _currentMeleeWeapon;
        private int _currentRangedWeapon;


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

        private PlayerModel()
        {
            _healthMax = 100.0f;
            _moveSpeed = 100.0f;
            _turnSpeed = 1.0f;

            Health = HealthMax;

            _flashAbility = new FlashAbility(_controller, _transform);
            _invulnerAbility = new InvulnerAbility();
        }

        public PlayerModel(CharacterController controller, Transform transform) : this()
        {
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
            Health += points;
            // OnHealthChanged?.Invoke(Health);
        }

        public void TakeDamage(float points)
        {
            Health -= points;
            // OnHealthChanged?.Invoke(Health);
        }

        public void Die() => Debug.Log("You are dead!");

        public void Attack(IDamagable damagable, float damage)
        {
            if (_activeWeapon is RangedWeapon)
            {
                if ((_activeWeapon as RangedWeapon).IsReloading)
                {
                    Debug.Log("Weapon reloading...");
                    return;
                }

                if ((_activeWeapon as RangedWeapon).HasAmmo)
                    (_activeWeapon as RangedWeapon).Shoot();
                
                else
                {
                    Debug.Log("No ammo!");
                    return;
                }
                Debug.Log("Shot! Ammo: " + (_activeWeapon as RangedWeapon).Ammo);
            }
            damagable?.TakeDamage(damage);
        }
        public void Attack(IDamagable damagable) => Attack(damagable, _activeWeapon.Damage);

        public void ReloadWeapon() => (_activeWeapon as RangedWeapon)?.Reload();

        public void SwitchWeapon()
        {
            if (_activeWeapon is MeleeWeapon)
            {
                _currentMeleeWeapon = (_currentMeleeWeapon + 1) % _meleeWeapons.Length;
                _activeWeapon = _meleeWeapons[_currentMeleeWeapon];
            }
            else
            {
                _currentRangedWeapon = (_currentRangedWeapon + 1) % _rangedWeapons.Length;
                _activeWeapon = _rangedWeapons[_currentRangedWeapon];
            }
            Debug.Log("after: " + _activeWeapon.Name);
        }

        public void SwitchWeaponType()
        {
            _activeWeapon =
                _activeWeapon == _meleeWeapons[_currentMeleeWeapon]
                    ? _rangedWeapons[_currentRangedWeapon] : _meleeWeapons[_currentMeleeWeapon];
            Debug.Log("after: " + _activeWeapon.Name);
        }

        public Weapon GetActiveWeapon()
        {
            return _activeWeapon;
        }
    }
}