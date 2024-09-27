using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class enemyTEST : MonoBehaviour
{
    public float MaxHP = 100;
    public float HP = 100;
    
    private Image img;
    private GameObject hp_bar;
    private bool isShowHP = false;
    
    //временно
    public FindPlayer findPlayer;
    public LayerMask playerLayer;
    public static bool isFindPlayer = false;
    [SerializeField] private NavMeshAgent agent;
    
    //public Transform target;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        hp_bar = transform.GetChild(0).gameObject;
        img = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>();
    }
    
    private void Start()
    {
        if (hp_bar.activeSelf == true)
        {
            hp_bar.SetActive(false);
        }
    }
    public void ShowHp()
    {
        hp_bar.SetActive(true);
        isShowHP = true;
    }

    public void ClosedHp()
    {
        isShowHP = false;
        hp_bar.SetActive(false);
    }
    private void Update()
    {
        if (isFindPlayer == true)
        {
            agent.SetDestination(findPlayer._player.transform.position);
        }
        
        
        
        if (isShowHP == true)
        {
            img.fillAmount = HP / MaxHP;
        }
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Destroy( this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ShowHp();
    }

    private void OnTriggerExit(Collider other)
    {
        ClosedHp();
    }
}
