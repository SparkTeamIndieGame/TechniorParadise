using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Vector3 offset;

    private void Awake()
    {
        offset = this.transform.position;
    }

    private void Update()
    {
        transform.position = target.position + offset;
    }
}
