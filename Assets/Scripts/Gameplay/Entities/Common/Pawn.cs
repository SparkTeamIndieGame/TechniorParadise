using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;


namespace Spark.Gameplay.Entities.Common
{
    [RequireComponent
        (typeof(NavMeshAgent),
        (typeof(Animator)))
        ]

    public class Pawn : Actor
    {
        [SerializeField] private List<Transform> _patrolPoints;

        [SerializeField] protected bool canMove = true;

        protected NavMeshAgent _navMeshAgent;
        protected Animator _animator;
        private int _lastIndexPoint;
        private Vector3 _spawnPoint;

        public void Start()
        {
            if (_navMeshAgent == null) _navMeshAgent = GetComponent<NavMeshAgent>();

            _spawnPoint = transform.position;
            _animator = GetComponent<Animator>();
        }

        public virtual void Update()
        {
            AnimMoveState();

        }
        public void MoveToTarget(Vector3 target)
        {
            if (canMove)
            {
                _navMeshAgent.stoppingDistance = 2;
                _navMeshAgent.SetDestination(target);
            }
        }

        public void ReturnToPatrol()
        {
            if (canMove)
            {
                _navMeshAgent.stoppingDistance = 0.5f;
                Patrol();
            }
        }

        public float DistanceToTarget(Vector3 target)
        {
            float distance = Vector3.Distance(transform.position, target);

            if (distance <= _navMeshAgent.stoppingDistance)
                _animator.SetTrigger("Attack");

                return distance;
        }

        private void Patrol()
        {
            // get spawn point or last point position if enemy has points
            var pointPosition = 
                _patrolPoints.Count == 0 ? 
                _spawnPoint :
                _patrolPoints[_lastIndexPoint].position;

            _navMeshAgent.SetDestination(pointPosition);
            if (_patrolPoints.Count == 0) return;

            var _distanceEnemyToPoint = Vector3.Distance(pointPosition, transform.position);
            if (_distanceEnemyToPoint <= _navMeshAgent.stoppingDistance)
                _lastIndexPoint = (_lastIndexPoint + 1) % _patrolPoints.Count;
        }

        public virtual void AnimMoveState()
        {
            if (!_navMeshAgent.hasPath)
            {
                _animator.SetBool("Run", false);
            }
            else
            {
                _animator.SetBool("Run", true);
            }
        }

    }
}