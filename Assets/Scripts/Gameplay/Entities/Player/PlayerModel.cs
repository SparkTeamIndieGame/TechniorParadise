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
using Spark.Utilities;
using Unity.VisualScripting;
using System.Linq;
using System.Collections.Generic;

namespace Spark.Gameplay.Entities.Player
{
    [Serializable]
    public class PlayerModel : IPlayer
    {
        
        #region Player events
        public event Action<float> OnHealthChanged;
        public event Action<int> OnDetailsChanged;
        #endregion

        [SerializeField] private CharacterController _controller;
        [SerializeField] private Transform _transform;

        [SerializeField, Min(10.0f)] private float _healthMax;
        [SerializeField, Min(0.0f)] private float _health;
        [SerializeField] private float _moveSpeed;
        [SerializeField, Range(0.0f, 2.0f)] private float _turnSpeed;

        [SerializeField] private int _details;
        public int Details
        {
            get
            {
                return _details;
            }
            set
            {
                OnDetailsChanged?.Invoke(value);
                _details = value;
            }
        }

        [field: SerializeField] public FlashCard.FlashCard FlashCard { get; set; }

        [SerializeField] private List<MeleeWeaponData> _meleeWeaponsData;
        [SerializeField] private List<RangedWeaponData> _rangedWeaponsData;
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

        public bool CanShoot { get; set; }

        public AudioSystem AudioSystem;
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
            AudioSystem.AudioDictinory["TakeDamage"].Play();

            if (Health <= 0) Die();
        }

        public void Die()
        {
            AudioSystem.AudioDictinory["Dead"].Play();
            
        }
        #endregion

        public void Attack()
        {
            if (!CanShoot) return;

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
                _currentMeleeWeaponData = (_currentMeleeWeaponData + 1) % _meleeWeaponsData.Count;
                ActiveWeapon.Data = _meleeWeaponsData[_currentMeleeWeaponData];
            }
            else if (ActiveWeapon.Data is RangedWeaponData)
            {
                _currentRangedWeaponData = (_currentRangedWeaponData + 1) % _rangedWeaponsData.Count;
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

        public bool AddNewWeapon(WeaponData weaponData)
        {
            bool success = true;
            if (weaponData is MeleeWeaponData)
            {
                if (_meleeWeaponsData.Find(weapon => weapon == weaponData) == null)
                    _meleeWeaponsData.Add(weaponData as MeleeWeaponData);

                else success = false;
            }
            else
            {
                if (_rangedWeaponsData.Find(weapon => weapon == weaponData) == null)
                    _rangedWeaponsData.Add(weaponData as RangedWeaponData);

                else success = false;
            }
            return success;
        }
    }
}