using Spark.Refactored.Gameplay.Entities.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Weapons.Ranged
{
    public class RefactoredRangedWeapon : RefactoredWeapon<RefactoredRangedWeaponData>
    {
        [SerializeField] private Image _reloadIconUI;
        [SerializeField] private Text _ammoCounterUI;
        [SerializeField] private Text _ammoMaximumUI;

        private Dictionary<System.Enum, float> _reloadingReadyTime = new();
        private Dictionary<System.Enum, int> _ammo = new();

        public bool isReloading => true;
        public float reloading => Mathf.Max(0, _reloadingReadyTime[data.type] - Time.time);
        public float reloadingDuration => data.reloadDuration;

        public void FillAmmo(System.Enum type)
        {
            var dataByType = _data.Find(current => current.type.Equals(type));
            _ammo[type] = dataByType.ammoMaximum;
        }

        public void Reload()
        {
            _reloadingReadyTime[data.type] = Time.time + reloadingDuration;
        }

        protected override void DoAction()
        {
            if (!data.isAutomatic)       
                TakeShot();

            else StartShooting();
        }

        protected override void AbortAction()
        {
            StopShooting();
        }

        private void StartShooting()
        {
            InvokeRepeating(nameof(TakeShot), .0f, data.rate);
        }

        private void StopShooting()
        {
            CancelInvoke(nameof(TakeShot));
        }

        private void TakeShot()
        {
            float maxDistance = 20.0f;
            Vector3 direction = GetBulletDirection();
            TrailRenderer trail = Instantiate(data.bulletTrail, transform.position, Quaternion.identity);

            if (Physics.Raycast(transform.position, transform.forward, out var hit, maxDistance, LayerMask.GetMask("Obstacle", "Enemy")))
            {
                StartCoroutine(HandleTrail(trail, hit.point, hit.transform));
            }
            else
            {
                Vector3 endPoint = transform.position + direction * maxDistance;
                StartCoroutine(HandleTrail(trail, endPoint, null));
            }
        }

        private Vector3 GetBulletDirection()
        {
            return transform.forward + new Vector3(
                Random.Range(-data.bulletSpreadRange.x, data.bulletSpreadRange.x),
                Random.Range(-data.bulletSpreadRange.y, data.bulletSpreadRange.y),
                Random.Range(-data.bulletSpreadRange.z, data.bulletSpreadRange.z)
            ).normalized;
        }

        private IEnumerator HandleTrail(TrailRenderer trail, Vector3 targetPoint, Transform hitTransform)
        {
            float time = 0;
            Vector3 startPosition = trail.transform.position;

            while (time < 1)
            {
                trail.transform.position = Vector3.Lerp(startPosition, targetPoint, time);
                time += Time.deltaTime / trail.time;
                yield return null;
            }

            if (hitTransform != null &&
                Vector3.Distance(hitTransform.position, trail.transform.position) < 0.5f &&
                hitTransform.TryGetComponent<IDamagable>(out var damagable))
            {
                damagable.health -= data.damage;
            }
            Destroy(trail.gameObject);
        }

        private IEnumerator HandleReloading()
        {
            yield return new WaitForSeconds(reloadingDuration);
            _ammo[data.type] = data.ammoMaximum;
        }
    }

    /*{
        private int _currentWeaponData;
        private float _reloadingReadyTime = .0f;

        public bool isReloading => Time.time < _reloadingReadyTime;
        public float reloading
        {
            get => Mathf.Max(0, _reloadingReadyTime - Time.time);
        }
        public float reloadingDuration => _data[_currentWeaponData].reloadDuration;


        public int ammo { get; private set; }

        protected override void DoAction()
        {
            if (isReloading || isReady) return;

            Vector3 direction = GetBulletDirection();
            TrailRenderer trail = Instantiate(_data[_currentWeaponData].bulletTrail, transform.position, Quaternion.identity);

            if (Physics.Raycast(transform.position, direction, out var hit, _data[_currentWeaponData].range)) 
            {                
                StartCoroutine(SpawnTrailToHit(trail, hit));
                Debug.LogWarning("with hit on " + hit);
            }
            else
            {
                StartCoroutine(SpawnTrailToPoint(trail, transform.forward * 20.0f));
                Debug.LogWarning("without hit");
            }
        }

        private Vector3 GetBulletDirection()
        {
            var direction = transform.forward + new Vector3(
                    Random.Range(-_data[_currentWeaponData].bulletSpreadRange.x, +_data[_currentWeaponData].bulletSpreadRange.x),
                    Random.Range(-_data[_currentWeaponData].bulletSpreadRange.y, +_data[_currentWeaponData].bulletSpreadRange.y),
                    Random.Range(-_data[_currentWeaponData].bulletSpreadRange.z, +_data[_currentWeaponData].bulletSpreadRange.z)
                );

            direction.Normalize();
            return direction;
        }

        public static IEnumerator SpawnTrailToHit(TrailRenderer trail, RaycastHit hit)
        {
            float time = 0;
            Vector3 startPosition = trail.transform.position;
            
            while (time < 1)
            {
                trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
                time += Time.deltaTime / trail.time;

                yield return null;
            }

            if (hit.transform != null && 
                Vector3.Distance(hit.point, trail.transform.position) < 0.5 &&
                hit.transform.TryGetComponent<IDamagable>(out var damagable))
            {
                Debug.Log("Damage to " + hit.transform);
                // damagable.TakeDamage(_data.damage);
            }
            Destroy(trail.gameObject);
        }

        public static IEnumerator SpawnTrailToPoint(TrailRenderer trail, Vector3 point)
        {
            float time = 0;
            Vector3 startPosition = trail.transform.position;
            
            while (time < 1)
            {
                trail.transform.position = Vector3.Lerp(startPosition, point, time);
                time += Time.deltaTime / trail.time;

                yield return null;
            }
            Destroy(trail.gameObject);
        }
    }*/
}