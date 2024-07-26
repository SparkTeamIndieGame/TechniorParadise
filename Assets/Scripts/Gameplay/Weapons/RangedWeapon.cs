using System.Collections;
using UnityEngine;

namespace Spark.Gameplay.Weapons
{
    [CreateAssetMenu(fileName = "NewRangedWeapon", menuName = "Weapon/New ranged weapon", order = 52)]
    public class RangedWeapon : Weapon
    {
        [SerializeField, Min(0.5f)] private float _reloadDuration;
        [SerializeField, Min(1)] private int _ammoMax;
        [SerializeField, Min(1)] private int _ammoPerShot;

        private float _reloadTimeLeft;
        private float _nextReadyTime;
        private int _ammo;

        public float ReloadTimeLeft => _reloadTimeLeft;
        public bool IsReloading => Time.time < _nextReadyTime;
        public bool HasAmmo => _ammo > 0;
        public int Ammo => _ammo;

        public void Shoot()
        {
            if (!IsReloading) _ammo = Mathf.Max(0, _ammo - _ammoPerShot);
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
    }
}