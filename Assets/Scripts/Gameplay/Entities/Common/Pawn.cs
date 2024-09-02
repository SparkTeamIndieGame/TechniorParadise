using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Spark.Utilities;


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
        [SerializeField] protected AudioSystem _audioSystem;
        [SerializeField] protected float _stopDic;

        protected Outline _outlineThisEnemy;
        protected NavMeshAgent _navMeshAgent;
        protected Animator _animator;
        private int _lastIndexPoint;
        private Vector3 _spawnPoint;

        private void Awake()
        {
            _outlineThisEnemy = GetComponent<Outline>();
        }

        public void Start()
        {
           
            if (_navMeshAgent == null) _navMeshAgent = GetComponent<NavMeshAgent>();

            _spawnPoint = transform.position;
            _animator = GetComponent<Animator>();
            _audioSystem.Instalize();

            AfterStart();
        }

        protected virtual void AfterStart() { }

        public virtual void Update()
        {
            AnimMoveState();
            AfterUpdate();
        }
        protected virtual void AfterUpdate() { }

        public void MoveToTarget(Vector3 target)
        {
            if (canMove)
            {
                _navMeshAgent.stoppingDistance = _stopDic;
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

            if (distance <= _navMeshAgent.stoppingDistance && !_navMeshAgent.isStopped)
            {
                _animator.SetTrigger("Attack");
                _outlineThisEnemy.OutlineWidth = 8.0f;
                _outlineThisEnemy.OutlineColor = Color.red;
            }
           
                

            return distance;
        }

        private void Patrol()
        {
            // get spawn point or last point position if enemy has points
            _outlineThisEnemy.OutlineWidth = 5.0f;
            _outlineThisEnemy.OutlineColor = Color.green;
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
                _audioSystem.AudioDictinory["Move"].mute = true;
            }
            else
            {
                _animator.SetBool("Run", true);
                _audioSystem.AudioDictinory["Move"].mute = false;
            }
        }

    }
}