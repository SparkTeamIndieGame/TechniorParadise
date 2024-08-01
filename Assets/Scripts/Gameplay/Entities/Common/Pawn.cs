using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;


namespace Spark.Gameplay.Entities.Common
{
    [RequireComponent(typeof(NavMeshAgent))]

    public class Pawn : Actor
    {
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private List<Transform> points;
        [SerializeField] private bool npc;

        private int _lastIndexPoint;
        private Vector3 _defaultPoint;

        public void Start()
        {
            if (navMeshAgent == null) navMeshAgent = GetComponent<NavMeshAgent>();

            if (points.Count == 0)
            {
                _defaultPoint = transform.position;
            }
        }

        public void MoveToTarget(Vector3 target)
        {
            navMeshAgent.SetDestination(target);

            //if (Vector3.Distance(target, transform.position) > _radiusView)
            //    _playerDetection = false;
        }

        private void FixedUpdate()
        {

        }

        public void ReturnToPoint()
        {

            if (points.Count == 0)
                navMeshAgent.SetDestination(_defaultPoint);
            else
                Patrol();
        }

        public float DistanceToTarget(Vector3 target)
        {
            return Vector3.Distance(transform.position, target);
        }

        private void Patrol()
        {
            navMeshAgent.SetDestination(points[_lastIndexPoint].position);


            var _distanceEnemyToPoint = Vector3.Distance(points[_lastIndexPoint].position, transform.position);

            if (_distanceEnemyToPoint <= navMeshAgent.stoppingDistance)
            {
                if (_lastIndexPoint == points.Count - 1)
                    _lastIndexPoint = 0;
                else
                    _lastIndexPoint += 1;
            }
        }

    }
}