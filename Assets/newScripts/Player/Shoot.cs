using System.Collections;
using System.Collections.Generic;
using Spark.Gameplay.Entities.Player;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public PlayerArsenal playerArsenal;

    public void Shooting()
    {
        GameObject newBullet = Instantiate(playerArsenal.actualWeapon.BulletPrefab, playerArsenal.actualWeapon.SpawnPoint.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody>().velocity = playerArsenal.actualWeapon.SpawnPoint.forward * playerArsenal.actualWeapon.BulletSpeed;
    }
}
