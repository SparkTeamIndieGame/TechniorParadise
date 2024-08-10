using Spark.Gameplay.Entities.Common.Data;
using System.Collections;
using System.Data;
using UnityEngine;

namespace Spark.Gameplay.Weapons.RangedWeapon
{
    class RangedWeapon : Weapon
    {
        [SerializeField] private RangedWeaponData _data;
        public override WeaponData Data
        {
            get => _data;
            set
            {
                if (value is RangedWeaponData rangedWeaponData) _data = rangedWeaponData;

                else
                {
                    Debug.LogError("Assigned data is not of type RangedWeaponData");
                }
            }
        }

        private Transform _firePoint;

        private float _lastShootTime = .0f;

        private void OnValidate()
        {
            _firePoint = null;
            _lastShootTime = .0f;
        }

        public void SetFirePoint(Transform firePoint)
        {
            _firePoint = firePoint;
        }

        public void Shoot()
        {

            if (_data.IsReloading || _lastShootTime + _data.FireRate > Time.time) return;

            PlaySound(_data.AudioClip, audioSource);

            Vector3 direction = GetBulletDirection();
            if (Physics.Raycast(_firePoint.position, direction, out var hit, _data.AttackRange))
            {
                _data.Shot();
                TrailRenderer trail = Instantiate(_data.BulletTrail, _firePoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit));
                _lastShootTime = Time.time;
            }
            if (!_data.HasAmmo) _data.Reload();
        }

        public void Update()
        {
            _data.Update();
        }

        private Vector3 GetBulletDirection()
        {
            var direction = _firePoint.forward + new Vector3(
                    Random.Range(-_data.BulletSpreadRange.x, +_data.BulletSpreadRange.x),
                    Random.Range(-_data.BulletSpreadRange.y, +_data.BulletSpreadRange.y),
                    Random.Range(-_data.BulletSpreadRange.z, +_data.BulletSpreadRange.z)
                );
            direction.Normalize();
            return direction;
        }

        public IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
        {
            float time = 0;
            Vector3 startPosition = trail.transform.position;

            _data.PlayParticleSystem(_data.ImpactParticleSystem, _firePoint);
            while (time < 1.0f)
            {
                trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
                time += Time.deltaTime / trail.time;

                yield return null;
            }

            if (hit.transform != null && hit.transform.TryGetComponent<IDamagable>(out var damagable))
            {
                _data.PlayParticleSystem(_data.HitEntityParticleSystem, hit.transform);
                damagable.TakeDamage(_data.AttackDamage);
            }
            Destroy(trail.gameObject, trail.time);
        }
    }
}