using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class WeaponBase : MonoBehaviour
{
    public string Name = "default";
    public int Damage = 1;
    public float ShootPeriod = 0.3f;

    public GameObject GunPrefab;
    public Transform SpawnPoint;
    public GameObject BulletPrefab;
    public float BulletSpeed = 15.0f;
    
    

    
    
    // public virtual void Shoot()
    // {
    //     
    //     GameObject newBullet = Instantiate(BulletPrefab, SpawnPoint.position, Quaternion.identity);
    //     newBullet.GetComponent<Rigidbody>().velocity = SpawnPoint.forward * BulletSpeed;
    // }

}
