using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindEnemy : MonoBehaviour
{
   
    [SerializeField] private GameObject enemyHealthBar;
    [SerializeField] private Image enemyHP;
    public enemyTEST _enemyTest;

    private void Start()
    {
        if (enemyHealthBar.activeSelf == true)
        {
            enemyHealthBar.SetActive(false);
        }

        
    }

    private void FixedUpdate()
    {
        if (_enemyTest != null)
        {
           
            enemyHealthBar.SetActive(true);
            enemyHP.fillAmount = _enemyTest.HP / _enemyTest.MaxHP;
         
        }
        else
        {
            enemyHealthBar.SetActive(false);
            _enemyTest = null;
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _enemyTest = other.GetComponent<enemyTEST>();
    }

    private void OnTriggerExit(Collider other)
    {
        _enemyTest = null;
        print("EXIT ОТРАБОТАЛ!");
    }
}
