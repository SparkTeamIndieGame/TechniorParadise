using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;


namespace Spark.Gameplay.Entities.Common
{
    [RequireComponent(typeof(NavMeshAgent))]

    public class Pawn : Actor
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private List<Transform> _patrolPoints;

        [SerializeField] private bool _canMove;

        private int _lastIndexPoint;
        private Vector3 _spawnPoint;

        public void Start()
        {
            if (_navMeshAgent == null) _navMeshAgent = GetComponent<NavMeshAgent>();

            _spawnPoint = transform.position;
        }

        public void MoveToTarget(Vector3 target)
        {
            if (_canMove) _navMeshAgent.SetDestination(target);
        }

        public void ReturnToPatrol()
        {
            if (_canMove) Patrol();
        }

        public float DistanceToTarget(Vector3 target)
        {
            return Vector3.Distance(transform.position, target);
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
    }
}