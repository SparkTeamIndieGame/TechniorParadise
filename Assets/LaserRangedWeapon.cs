using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRangedWeapon : MonoBehaviour
{
    [SerializeField] Transform _firePoint;
    [SerializeField] LineRenderer _line;
    [SerializeField] float _distance = 100.0f;

    private void Start()
    {
        if (_line == null) _line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        Vector3 begin = transform.position;
        Vector3 direction = transform.forward;
        Vector3 end = begin + (direction * _distance);

        end = Physics.Raycast(begin, direction, out var hit, _distance) ? hit.point : end;

        var length = Vector3.Distance(begin, end);
        _line.SetPosition(1, Vector3.forward * length);
    }

    public void SetFirePoint(Transform firePoint) => _firePoint = firePoint;
}
