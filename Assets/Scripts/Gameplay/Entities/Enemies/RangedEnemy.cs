using Spark.Gameplay.Entities.Player;
using UnityEngine;

namespace Spark.Gameplay.Entities.Enemies
{
    public class RangedEnemy : Enemy
    {
        static Vector3 vector;
        public Vector3 ddd;
        private void Awake()
        {
            canMove = false;
        }

        public override void Update()
        {
            base.Update();
            if(Input.GetKeyDown(KeyCode.Space))
            {
                vector += new Vector3(0, 1, 0);
                ddd = vector;
            }
        }
        public override void AnimMoveState()
        {
            if (DistanceToTarget(_target.position) <= _attackRange)
            {
                if (!Physics.Linecast(transform.position, _target.position, layerMask) &&
               _target.gameObject.layer == SortingLayer.NameToID("Player"))
                {
                    _animator.SetTrigger("Attack");
                    transform.LookAt(_target, vector);
                }
            }
        }

        protected override void CalculateHit()
        {
            Debug.DrawRay(transform.position + Vector3.up, transform.forward, Color.red, Mathf.Infinity);

            if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out var hit, _attackRange, layerMask) && 
                hit.transform.TryGetComponent<PlayerController>(out var playerModel))
            {
                OnEnemyAttack?.Invoke(_damage);
                ParticlPlay(_impactParticleSystem, hit.transform);
            }





        }

    }
}