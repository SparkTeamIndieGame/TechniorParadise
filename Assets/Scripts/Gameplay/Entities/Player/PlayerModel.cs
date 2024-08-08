using Spark.Gameplay.Entities.Common.Data;
using Spark.Gameplay.Entities.Common.Abilities;
using Spark.Gameplay.Weapons.MeleeWeapon;
using Spark.Gameplay.Weapons.RangedWeapon;
using Spark.Gameplay.Weapons;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Spark.Gameplay.Entities.Enemies;
using UnityEngine.UI;
using Spark.Gameplay.Entities.Common.Behaviour;

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

        [field: SerializeField] public int Details { get; set; }

        [SerializeField] private MeleeWeaponData[] _meleeWeaponsData;
        [SerializeField] private RangedWeaponData[] _rangedWeaponsData;
        [SerializeField] public Weapon ActiveWeapon 
        { 
            get => _toggleActiveWeaponType ? ActiveRangedWeapon : ActiveMeleeWeapon;
            set
            {
                if (!value) return; // ?
                if (value is MeleeWeapon) ActiveMeleeWeapon.Data = (value as MeleeWeapon).Data;
                else ActiveRangedWeapon.Data = (value as RangedWeapon).Data;
            }
        }
        bool _toggleActiveWeaponType = true;

        [SerializeField] private MeleeWeapon ActiveMeleeWeapon;
        [SerializeField] private RangedWeapon ActiveRangedWeapon;

        [SerializeField] private Transform _target;

        private int _currentMeleeWeaponData;
        private int _currentRangedWeaponData;

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

            ActiveWeapon = ActiveMeleeWeapon;
        }

        #region Movement
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
        #endregion

        #region Damagable
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
        #endregion

        public void Attack()
        {
            if (ActiveWeapon.Data is RangedWeaponData rwData)
            {
                if (rwData.IsReloading) return;

                if (rwData.HasAmmo)
                    (ActiveWeapon as RangedWeapon).Shoot();
            }
            else if (ActiveWeapon.Data is MeleeWeaponData)
            {
                (ActiveWeapon as MeleeWeapon).TakeSwing();
            }
        }

        public void ReloadWeapon() => (ActiveWeapon.Data as RangedWeaponData)?.Reload();

        public void SwitchWeapon()
        {
            if (ActiveWeapon.Data is MeleeWeaponData)
            {
                _currentMeleeWeaponData = (_currentMeleeWeaponData + 1) % _meleeWeaponsData.Length;
                ActiveWeapon.Data = _meleeWeaponsData[_currentMeleeWeaponData];
            }
            else if (ActiveWeapon.Data is RangedWeaponData)
            {
                _currentRangedWeaponData = (_currentRangedWeaponData + 1) % _rangedWeaponsData.Length;
                ActiveWeapon.Data = _rangedWeaponsData[_currentRangedWeaponData];
            }
        }
        public void SwitchWeaponType()
        {
            _toggleActiveWeaponType = !_toggleActiveWeaponType;
            ActiveWeapon.Data = _toggleActiveWeaponType ? ActiveRangedWeapon.Data : ActiveMeleeWeapon.Data;
        }

        public int GetCurrentMeleeWeapon() => _currentMeleeWeaponData;
        public int GetCurrentRangedWeapon() => _currentRangedWeaponData;

        public void SetTarget(Transform target) => _target = target;
        public void ResetTarget() => _target = null;
    }
}