using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArsenal : MonoBehaviour
{
    public List<WeaponBase> arsenals;

    public WeaponBase actualWeapon;
    private int currentArsenal;
    private void Start()
    {
        actualWeapon = arsenals[0];
        currentArsenal = 0;
    }

    public void AddWeapon(WeaponBase newWeapon)
    {
        arsenals.Add(newWeapon);
    }

    public void ChangedWeapon()
    {
        if (arsenals.Count > ++currentArsenal)
        {
            currentArsenal += 1;
            actualWeapon = arsenals[currentArsenal];
        }
        else
        {
            currentArsenal = 0;
        }
    }
}
