using Spark.Gameplay.Entities.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockOfEnemies : MonoBehaviour
{
    [SerializeField] List<Enemy> _enemies;
    [SerializeField] Transform _target;

    [SerializeField] float _detectionRange;
    [SerializeField, Range(0.1f, 1.0f)] float _scanInterval;

    [SerializeField] bool _targetDetected = false;
    private Vector3 _directionToTarget;

    private void Start()
    {
        StartCoroutine(ScanTarget());
    }

    IEnumerator ScanTarget()
    {        
        while (!_targetDetected)
        {
            DetectTarget();
            yield return new WaitForSeconds(_scanInterval);
        }
    }

    void DetectTarget()
    {
        float distanceToTarget = Vector3.Distance(transform.position, _target.position);
        if (distanceToTarget < _detectionRange)
        {
            if (!Physics.Linecast(transform.position, _directionToTarget))
            {
                Debug.Log("Target detected!");
                _targetDetected = true;
            }
        }
    }

    private void Update()
    {
        _directionToTarget = (_target.position - transform.position).normalized;
    }

    private void FixedUpdate()
    {
        if (_targetDetected)
        {
            _enemies.ForEach(enemy =>
            {
                Vector3 enemyDirectionToTarget = (_target.position - enemy.transform.position).normalized;

                enemy.Move(enemyDirectionToTarget);
                enemy.Turn(enemyDirectionToTarget);
            });
        }
    }

    private void OnDrawGizmos()
    {
        if (_targetDetected) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
        Gizmos.DrawLine(transform.position, _target.position);
    }
}
