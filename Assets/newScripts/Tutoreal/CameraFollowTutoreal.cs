
using System;
using UnityEngine;

public class CameraFollowTutoreal : MonoBehaviour
{
    public Transform target; // Персонаж, за которым будет следовать камера
    public float smoothSpeed = 0.125f; // Скорость сглаживания
    public Vector3 offset; // Смещение камеры относительно персонажа

    private void Start()
    {
        offset = this.transform.position;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = new Vector3(transform.position.x, transform.position.y, target.position.z + offset.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
