using Spark.Gameplay.Entities.Player;
using UnityEngine;

namespace Spark.Gameplay.Entities.Enemies
{
    public class MeleeEnemy : Enemy
    {
        protected override void CalculateHit()
        {
            var hits = Physics.OverlapSphere(transform.position, _attackRange);
            foreach (var hit in hits)
            {
                if (hit.transform.TryGetComponent<PlayerController>(out var playerModel))
                {
                    OnEnemyAttack?.Invoke(_damage);
                    ParticlPlay(_impactParticleSystem, hit.transform);
                }
            }
        }
    }
}