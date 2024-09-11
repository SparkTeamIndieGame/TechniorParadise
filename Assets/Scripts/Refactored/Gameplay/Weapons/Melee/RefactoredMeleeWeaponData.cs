using Spark.Gameplay.Entities.Common.Data;
using UnityEngine;

namespace Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Weapons.Melee
{
    public enum MeleeWeaponType
    {
        Knife,
        Spear,
        CircularSaw
    }

    [CreateAssetMenu(fileName = "NewMeleeWeapon", menuName = "Refactored Weapon/New melee weapon", order = 54)]
    public class RefactoredMeleeWeaponData : RefactoredWeaponData
    {
        [field: SerializeField] private MeleeWeaponType _weaponType;
        public override System.Enum type => _weaponType;
    }
}