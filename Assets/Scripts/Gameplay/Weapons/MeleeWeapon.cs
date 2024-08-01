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

            Debug.DrawRay(_handPoint.position, _handPoint.forward, Color.black, Range);
            var hits = Physics.OverlapSphere(_handPoint.position, Range);
            foreach ( var hit in hits ) 
            {
                hit.transform.GetComponent<IDamagable>()?.TakeDamage(Damage);
                if (hit.transform.GetComponent<IDamagable>() != null)
                    Debug.Log("I attack it!");
            }
            _lastSwingTime = Time.time;
        }
    }
}