using Spark.Gameplay.Entities.Common.Data;
using UnityEngine;

namespace Spark.Gameplay.Weapons.MeleeWeapon
{
    public enum MeleeWeaponType
    {
        Knife,
        Katana,
        Sword
    }

    [CreateAssetMenu(fileName = "NewMeleeWeapon", menuName = "Weapon/New melee weapon", order = 51)]
    public class MeleeWeaponData : WeaponData
    {
        [field: SerializeField] private MeleeWeaponType _weaponType;

        [field: SerializeField, Min(.1f)] public float SwingDelay { get; private set; }

    }
}