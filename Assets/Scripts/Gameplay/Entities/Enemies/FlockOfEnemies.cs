using Spark.Gameplay.Entities.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FlockOfEnemies : MonoBehaviour
{
    [SerializeField] List<Enemy> _enemies;
    [SerializeField] Transform _target;

    [SerializeField] float _detectionRange;
    [SerializeField, Range(0.1f, 1.0f)] float _scanInterval;

    [SerializeField] private float _distanceView = 8;
    [SerializeField] private List<int> idEnemy;

    [SerializeField] bool _targetDetected = false;

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
            if (!Physics.Linecast(transform.position, _target.position) && 
                _target.gameObject.layer == SortingLayer.NameToID("Player"))
            {
                Debug.Log("Target detected!");
                _targetDetected = true;
            } 
        }
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {

        if (_targetDetected)
        {
            if (_target.gameObject.layer == SortingLayer.NameToID("Enemy"))
            {
                _targetDetected = false;

                StartCoroutine(ScanTarget());
                return;
            }

            _enemies.ForEach(enemy =>
            {
                enemy.MoveToTarget(_target.position);

            });

            CheackDistanceToTarget();
        }

        else
        {
            _enemies.ForEach(enemy =>
            {
                enemy.ReturnToPoint();
            });
        }

    }

    private void CheackDistanceToTarget()
    {
        _enemies.ForEach(enemy =>
        {
            if (enemy.DistanceToTarget(_target.position) <= _distanceView)
            {
                if (idEnemy.Contains(enemy.GetInstanceID()))
                    return;

                else
                    idEnemy.Add(enemy.GetInstanceID());

            }

            else if (enemy.DistanceToTarget(_target.position) > _distanceView)
            {
                if (idEnemy.Contains(enemy.GetInstanceID()))
                    idEnemy.Remove(enemy.GetInstanceID());

                else
                    return;

            }
        });

        if (idEnemy.Count == 0)
        {
            _targetDetected = false;
            StopAllCoroutines();
            StartCoroutine(ScanTarget());
        }
        else
            _targetDetected = true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_targetDetected) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
        Gizmos.DrawLine(transform.position, _target.position);
    }
#endif
}
