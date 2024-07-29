using Spark.Gameplay.Entities.Common.Data;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;

namespace Spark.Gameplay.Weapons
{
    [CreateAssetMenu(fileName = "NewRangedWeapon", menuName = "Weapon/New ranged weapon", order = 52)]
    public class RangedWeapon : Weapon
    {
        [SerializeField, Min(0.5f)] private float _reloadDuration;
        [SerializeField, Min(1)] private int _ammoMax;
        [SerializeField, Min(1)] private int _ammoPerShot;
        [SerializeField] private bool _automatic;

        [SerializeField, Range(0.0f, 1.0f)] private float _bulletSpreadRange;
        [SerializeField] private Transform _bulletSpawnPoint;
        [SerializeField] private TrailRenderer _bulletTrail;

        [SerializeField] private ParticleSystem _shootingParticleSystem;
        [SerializeField] private ParticleSystem _impactParticleSystem;
        [SerializeField, Min(0.1f)] private float _shootDelay;
        private float _lastShootTime;

        private float _reloadTimeLeft;
        private float _nextReadyTime;
        private int _ammo;

        public bool IsAutomatic => _automatic;
        public float ReloadTimeLeft => _reloadTimeLeft;
        public bool IsReloading => Time.time < _nextReadyTime;
        public bool HasAmmo => _ammo > 0;
        public int Ammo => _ammo;

        private void OnValidate()
        {
            _ammo = _ammoMax;

            _nextReadyTime = 0.0f;
            _reloadTimeLeft = 0.0f;

            _lastShootTime = 0.0f;
        }

        public void SetBulletSpawnPoint(Transform transform)
        {
            _bulletSpawnPoint = transform;
        }

        public void Shoot()
        {
            if (IsReloading || _lastShootTime + _shootDelay > Time.time) return;

            Vector3 direction = GetBulletDirection();
            Debug.DrawRay(_bulletSpawnPoint.position + Vector3.up, direction, Color.magenta, Range);
            if (Physics.Raycast(_bulletSpawnPoint.position + Vector3.up, direction + Vector3.up, out var hit, Range))
            {
                hit.transform.TryGetComponent(out IDamagable damagable);
                damagable?.TakeDamage(Damage);  
            }
            _lastShootTime = Time.time;
            _ammo = Mathf.Max(0, _ammo - _ammoPerShot);
        }

        public void Reload()
        {
            _nextReadyTime = Time.time + _reloadDuration;
            _ammo = _ammoMax;
            Debug.Log("Reloded! Ammo: " + _ammo);
        }

        public void Update()
        {
            _ammoPerShot = Mathf.Min(_ammoPerShot, _ammoMax);
            _reloadTimeLeft = Mathf.Max(0, _nextReadyTime - Time.time);
        }

        private Vector3 GetBulletDirection()
        {
            var direction = _bulletSpawnPoint.forward + new Vector3(
                    Random.Range(-_bulletSpreadRange, +_bulletSpreadRange),
                    Random.Range(-_bulletSpreadRange, +_bulletSpreadRange),
                    Random.Range(-_bulletSpreadRange, +_bulletSpreadRange)
                );
            direction.Normalize();
            return direction;
        }
    }
}