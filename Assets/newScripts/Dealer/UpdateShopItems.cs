using Spark.Gameplay.Weapons;
using Spark.Gameplay.Weapons.MeleeWeapon;
using Spark.Gameplay.Weapons.RangedWeapon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UpdateShopItems : MonoBehaviour
{
    public static List<RangedWeaponData> WeaponDataList = new List<RangedWeaponData>();
    public static MeleeWeaponData MeleeWeapon;
    public static Action OnUpdate;
    [SerializeField] private RangedWeaponData _rangedWeapon_type1;
    [SerializeField] private RangedWeaponData _rangedWeapon_type2;
    [SerializeField] private MeleeWeaponData meleeWeapon_type0;
    public GameObject Ui;

    private void OnTriggerEnter(Collider other)
    {
        if (WeaponDataList.Count != 0)
        {
            WeaponDataList.Clear();
        }
        WeaponDataList.Add(_rangedWeapon_type1);
        WeaponDataList.Add(_rangedWeapon_type2);
        MeleeWeapon = meleeWeapon_type0;
        OnUpdate?.Invoke();
        Ui.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {

        Ui.SetActive(false);
    }
}
