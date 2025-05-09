using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FindEnemy : MonoBehaviour
{
    public enemyTEST _closestEnemy;
    private List<enemyTEST> _allFindEnemy = new List<enemyTEST>();

    private void Update()
    {
        if (_allFindEnemy.Count != 0)
        {
            FindClosedEnemy();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _allFindEnemy.Add(other.GetComponent<enemyTEST>());
    }

    private void OnTriggerExit(Collider other)
    {
        _allFindEnemy.Remove(other.GetComponent<enemyTEST>());
        _closestEnemy = null;
    }
    
    public void FindClosedEnemy()
    {
       
        float distance = Mathf.Infinity;
        for (int i = 0; i < _allFindEnemy.Count; i++)
        {
            if (_allFindEnemy[i] != null)
            {
                float currentDis = Vector3.Distance(this.transform.position, _allFindEnemy[i].transform.position);
                if (currentDis < distance)
                {
                    _closestEnemy = _allFindEnemy[i];
                    distance = currentDis;
                }
            }
            else
            {
                _allFindEnemy.Remove(_allFindEnemy[i]);
            }
        }
        
    }
}
