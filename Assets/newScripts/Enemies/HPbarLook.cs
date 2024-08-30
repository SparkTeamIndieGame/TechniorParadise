using UnityEngine;

public class HPbarLook : MonoBehaviour
{
    private Camera cam;
    private Vector3 target;
    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
       target = cam.transform.position - this.transform.position;
       transform.rotation = Quaternion.LookRotation(target);
    }
}
