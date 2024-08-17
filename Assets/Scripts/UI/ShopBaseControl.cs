using System.Collections;
using System.Collections.Generic;
using Spark.Gameplay.Weapons;
using UnityEngine;

public class ShopBaseControl : MonoBehaviour
{
    [Header("Range Weapon")]
    [SerializeField] private WeaponData _semiAutomat;
    [SerializeField] private WeaponData _rife;
    [SerializeField] private WeaponData _shotgun;
    [SerializeField] private WeaponData _automat;
    [SerializeField] private WeaponData _stormGun;
    [Space(10)]
    [Header("Melee Weapon")]
    [SerializeField] private WeaponData _saw;
    [SerializeField] private WeaponData _katana;

    public WeaponData BuyWeapon(int ID)
    {
        switch (ID)
        {
            case 1:
                return _semiAutomat;
            case 2:
                return _rife;
            case 3:
                return _shotgun;
            case 4:
                return _automat;
            case 5:
                return _stormGun;
            case 6:
                return _saw;
            case 7:
                return _katana;
            default:
                return null;
        }
    }
}
