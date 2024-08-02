using Spark.Gameplay.Entities.Player;
using UnityEngine;

namespace Spark.Gameplay.Entities.Enemies
{
    public class RangedEnemy : Enemy
    {
        private void Awake()
        {
            canMove = false;
        }

        public override void AnimMoveState()
        {
            if (DistanceToTarget(_target.position) <= _attackRange)
            {
                _animator.SetTrigger("Attack");
                transform.LookAt(_target);
            }
        }

        protected override void CalculateHit()
        {
            Debug.DrawRay(transform.position + Vector3.up, transform.forward, Color.red, Mathf.Infinity);

            if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out var hit, _attackRange, layerMask) && 
                hit.transform.TryGetComponent<PlayerController>(out var playerModel))
                OnEnemyAttack?.Invoke(_damage);




        }

    }
}