using System.Collections;
using System.Collections.Generic;
using Spark.Gameplay.Weapons;
using Spark.Gameplay.Weapons.RangedWeapon;
using UnityEngine;

public class PseudoPlayer : MonoBehaviour
{
    public int Details = 1000;
    //можно написать дефолт оружие и в старте проверить на null и присвоить, например нож и пистолет.
    public List<WeaponData> AvailableWeapons; //контроллер имеющихъся оружиц
    
}
