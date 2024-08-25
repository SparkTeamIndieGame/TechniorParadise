using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyTEST : MonoBehaviour
{
    public float MaxHP = 100;
    public float HP = 100;
    public void TakeDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            print("death");
        }
    }
}
