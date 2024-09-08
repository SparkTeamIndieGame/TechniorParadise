using Spark.Gameplay.Entities.Common.Data;
using Spark.Gameplay.Weapons.RangedWeapon;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Weapons.Melee
{
    public class RefactoredMeleeWeapon : RefactoredWeapon<RefactoredMeleeWeaponData>
    {
        protected override void DoAction()
        {
            Debug.Log($"Active by {data.name} ({data.type})");

            /*if (_lastSwingTime + _data.SwingDelay > Time.time) return;

            var hits = Physics.OverlapSphere(transform.position, _data.range);
            foreach (var hit in hits)
            {
                if (hit.transform.TryGetComponent(out IDamagable damagable))
                {
                    damagable.TakeDamage(_data.damage);
                }
            }

            _lastSwingTime = Time.time;*/
        }
    }
}