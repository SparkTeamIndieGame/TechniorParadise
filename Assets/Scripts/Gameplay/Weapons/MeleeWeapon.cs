using Spark.Gameplay.Entities.Common.Data;
using System.Collections;
using UnityEngine;

namespace Spark.Gameplay.Weapons
{
    [CreateAssetMenu(fileName = "NewMeleeWeapon", menuName = "Weapon/New melee weapon", order = 51)]
    public class MeleeWeapon : Weapon
    {
        [SerializeField, Min(.1f)] private float _swingDelay;

        private Transform _handPoint;
        private float _lastSwingTime;
        private TrailRenderer trail;

        private void OnValidate()
        {
            _lastSwingTime = 0.0f;
        }

        public void SetHandPoint(Transform handPoint)
        {
            _handPoint = handPoint;
        }

        public void TakeSwing()
        {
            if (_lastSwingTime + _swingDelay > Time.time) return;

            //trail.GetComponent<TrailRenderer>().emitting = true;

            var hits = Physics.OverlapSphere(_handPoint.position, Range);
            foreach (var hit in hits)
            {
                if (hit.transform.TryGetComponent(out IDamagable damagable))
                {
                    ParticlPlay(_impactParticleSystem, hit.transform);
                    damagable.TakeDamage(Damage);
                }
            }

            _lastSwingTime = Time.time;
        }
    }
}