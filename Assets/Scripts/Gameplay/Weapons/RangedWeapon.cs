using Spark.Gameplay.Entities.Common.Data;
using UnityEngine;
using UnityEngine.InputSystem.HID;

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
        [SerializeField] private TrailRenderer _bulletTrail;

        [SerializeField, Min(0.1f)] private float _shootDelay;

        private Transform _firePoint;
        
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

            _reloadTimeLeft = 0.0f;
            _nextReadyTime = 0.0f;

            _lastShootTime = 0.0f;
        }

        public void SetFirePoint(Transform firePoint)
        {
            _firePoint = firePoint;
        }

        public void Shoot()
        {
            if (IsReloading || _lastShootTime + _shootDelay > Time.time) return;

            Vector3 direction = GetBulletDirection();
            if (Physics.Raycast(_firePoint.position, direction, out var hit, Range))
            {
                if(hit.transform.TryGetComponent(out IDamagable damagable))
                {
                    ParticlPlay(_impactParticleSystem, hit.transform);
                    damagable.TakeDamage(Damage);  

                }
            }
            _lastShootTime = Time.time;
            ParticlPlay(_shootingParticleSystem, _firePoint);
            _ammo = Mathf.Max(0, _ammo - _ammoPerShot);
        }

        public void Reload()
        {
            _nextReadyTime = Time.time + _reloadDuration;
            _ammo = _ammoMax;
        }

        public void Update()
        {
            _ammoPerShot = Mathf.Min(_ammoPerShot, _ammoMax);
            _reloadTimeLeft = Mathf.Max(0, _nextReadyTime - Time.time);
        }

        private Vector3 GetBulletDirection()
        {
            var direction = _firePoint.forward + new Vector3(
                    Random.Range(-_bulletSpreadRange, +_bulletSpreadRange),
                    Random.Range(-_bulletSpreadRange, +_bulletSpreadRange),
                    Random.Range(-_bulletSpreadRange, +_bulletSpreadRange)
                );
            direction.Normalize();
            return direction;
        }

    }
}