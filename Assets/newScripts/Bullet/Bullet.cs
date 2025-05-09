
using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void Start()
    {
        Destroy(this.gameObject, 1.5f);
    }

    private void OnCollisionEnter(Collision other)
    {
        other.gameObject.GetComponent<enemyTEST>().TakeDamage(5);
        Destroy(this.gameObject);
    }
}
