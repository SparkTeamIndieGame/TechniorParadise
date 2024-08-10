using Spark.Gameplay.Entities.Common.Data;
using Spark.Gameplay.Weapons.RangedWeapon;
using UnityEngine;

namespace Spark.Gameplay.Weapons.MeleeWeapon
{
    class MeleeWeapon : Weapon
    {
        [SerializeField] private MeleeWeaponData _data;
        public override WeaponData Data
        {
            get => _data;
            set
            {
                if (value is MeleeWeaponData meleeWeaponData) _data = meleeWeaponData; 

                else 
                {
                    Debug.LogError("Assigned data is not of type meleeWeaponData");
                }
            }
        }

        private Transform _handPoint;
        private float _lastSwingTime;

        public void SetHandPoint(Transform handPoint)
        {
            _handPoint = handPoint;
        }

        public void TakeSwing()
        {
            if (_lastSwingTime + _data.SwingDelay > Time.time) return;

            PlaySound(_data.AudioClip, audioSource);

            var hits = Physics.OverlapSphere(_handPoint.position, _data.AttackRange);
            foreach (var hit in hits)
            {
                if (hit.transform.TryGetComponent(out IDamagable damagable))
                {
                    _data.PlayParticleSystem(_data.HitEntityParticleSystem, hit.transform);
                    damagable.TakeDamage(_data.AttackDamage);
                }
            }

            _lastSwingTime = Time.time;
        }
    }
}