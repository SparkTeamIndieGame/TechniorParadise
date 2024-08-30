using Spark.Gameplay.Entities.Player;
using UnityEngine;
using System.Collections;
using Spark.Gameplay.Entities.Common.Data;

namespace Spark.Gameplay.Entities.Enemies
{
    public class RangedEnemy : Enemy
    {
        [SerializeField] GameObject bullet;
        [SerializeField] Transform[] bulletPoint;
        public float speedB;

        private void Awake()
        {
            canMove = false;
        }

        public override void AnimMoveState()
        {
            if (DistanceToTarget(_target.position) <= _attackRange)
            {
                if (!Physics.Linecast(transform.position, _target.position, layerMask) &&
               _target.gameObject.layer == SortingLayer.NameToID("Player") && !_navMeshAgent.isStopped)
                {
                    _animator.SetTrigger("Attack");
                    transform.LookAt(_target);
                }
            }
        }

        protected override void CalculateHit()
        {
            Debug.DrawRay(transform.position + Vector3.up, transform.forward, Color.red, Mathf.Infinity);

            if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out var hit, _attackRange, layerMask) &&
                hit.transform.TryGetComponent<PlayerController>(out var playerModel))
            {
                var InstBullet = Instantiate(bullet, bulletPoint[Random.Range(0, bulletPoint.Length)].position, Quaternion.identity);
                StartCoroutine(SpawnTrail(InstBullet, hit));

                OnEnemyAttack?.Invoke(_damage);
                ParticlPlay(_impactParticleSystem, hit.transform);
                //Destroy(InstBullet, 1);
            }
        }

        public IEnumerator SpawnTrail(GameObject trail, RaycastHit hit)
        {
            var target = hit.transform.position;

            while(trail.transform.position != hit.transform.position)
            {
                trail.transform.position = Vector3.MoveTowards(trail.transform.position, target, speedB * Time.deltaTime);
                yield return null; 
            }

            Destroy(trail);


        }
    }
}