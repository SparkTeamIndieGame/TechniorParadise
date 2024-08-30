using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyTEST : MonoBehaviour
{
    public float MaxHP = 100;
    public float HP = 100;
    
    private Image img;

    private void Awake()
    {
        img = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>();
    }

    private void Update()
    {
        print(img.gameObject.name);
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            print("death");
        }
    }
}
