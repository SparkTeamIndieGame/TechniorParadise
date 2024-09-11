using Spark.Gameplay.Entities.Common.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.HID;

namespace Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Weapons.Ranged
{
    public enum RangedWeaponType
    {
        Pistol,
        AssaultRifle,
        SniperRifle,
        SubmachineGun,
        MachineGun,
    }

    [CreateAssetMenu(fileName = "NewRangedWeapon", menuName = "Refactored Weapon/New ranged weapon", order = 53)]
    public class RefactoredRangedWeaponData : RefactoredWeaponData
    {
        [field: SerializeField] private RangedWeaponType _weaponType;
        public override System.Enum type => _weaponType;

        [field: SerializeField] public TrailRenderer bulletTrail { get; private set; }
        [field: SerializeField] public Vector3 bulletSpreadRange { get; private set; } = new(.0f, .0f, .0f);

        [field: SerializeField, Min(0.5f)] public float reloadDuration { get; private set; }
        [field: SerializeField, Min(1)] public int ammoMaximum { get; private set; }
        [field: SerializeField, Min(1)] public int ammoPerShot { get; private set; }
        [field: SerializeField] public bool isAutomatic { get; private set; }
    }
}