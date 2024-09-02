using System;
using Spark.Gameplay.Entities.Enemies;
using UnityEngine;
using UnityEngine.UI;

public class HPbarLook : MonoBehaviour
{
    private Camera cam;
    private Vector3 target;
    private GameObject _hpBarObject;
    
    //enemy
    private Image _actualHp;
    private Enemy _thisEnemey;

    private void Awake()
    {
        _hpBarObject = transform.GetChild(0).gameObject;
        _actualHp = _hpBarObject.transform.GetChild(1).GetComponent<Image>();
        _thisEnemey = transform.GetComponentInParent<Enemy>();
    }

    private void Start()
    {
        cam = Camera.main;
        if (_hpBarObject.activeSelf == true)
        {
            _hpBarObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (_hpBarObject.activeSelf == false)
        {
            return;
        }
        
        transform.eulerAngles = cam.transform.eulerAngles;
        _actualHp.fillAmount = _thisEnemey.Health / _thisEnemey.MaxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        _hpBarObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        _hpBarObject.SetActive(false);
    }
}
