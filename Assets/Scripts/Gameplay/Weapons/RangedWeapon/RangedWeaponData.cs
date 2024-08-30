using Spark.Gameplay.Entities.Common.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.HID;

namespace Spark.Gameplay.Weapons.RangedWeapon
{
    [CreateAssetMenu(fileName = "NewRangedWeapon", menuName = "Weapon/New ranged weapon", order = 52)]
    public class RangedWeaponData : WeaponData
    {
        [field: SerializeField] public TrailRenderer BulletTrail { get; private set; }
        [field: SerializeField] public Vector3 BulletSpreadRange { get; private set; } = new(.0f, .0f, .0f);

        [field: SerializeField, Min(0.5f)] public float ReloadDuration { get; private set; }
        [field: SerializeField, Min(1)] public int AmmoMax { get; private set; }
        [field: SerializeField, Min(1)] public int AmmoPerShot { get; private set; }
        [field: SerializeField] public bool Automatic { get; private set; }
        [field: SerializeField, Min(0.1f)] public float FireRate { get; private set; }

        private float _reloadTimeLeft;
        private float _nextReadyTime;
        private int _ammo;

        public bool IsAutomatic => Automatic;
        public float ReloadTimeLeft => _reloadTimeLeft;
        public bool IsReloading => Time.time < _nextReadyTime;
        public bool HasAmmo => _ammo > 0;
        public int Ammo => _ammo;

        private void OnValidate()
        {
            AmmoPerShot = Mathf.Min(AmmoPerShot, AmmoMax);
            
            _reloadTimeLeft = .0f;
            _nextReadyTime = .0f;

            _ammo = AmmoMax;
        }

        public void Shot()
        {
            _ammo = Mathf.Max(0, _ammo - AmmoPerShot);
        }

        public void Reload()
        {
            _nextReadyTime = Time.time + ReloadDuration;
            _ammo = AmmoMax;
        }

        public void Update()
        {
            _reloadTimeLeft = Mathf.Max(0, _nextReadyTime - Time.time);
        }
    }
}