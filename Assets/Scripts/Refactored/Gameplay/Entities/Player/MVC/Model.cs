using Spark.Gameplay.Entities.RefactoredPlayer;
using Spark.Gameplay.Entities.RefactoredPlayer.RefactoredSystems;
using Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Weapons.Melee;
using Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Weapons.Ranged;
using Spark.Refactored.Gameplay.Abilities;
using Spark.Refactored.Gameplay.Entities.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spark.Refactored.Gameplay.Entities.Player.MVC
{
    public class Model : IDamagable // todo: ScriptableObject, 
    {
        public FlashAbility flashAbility { get; } = new();
        public InvulnerAbility invulnerAbility { get; } = new();
        public MedKitAbility medkitAbility { get; } = new();

        public FlashDrive flashDrive { get; } = new();
        public float details { get; set; }

        private float _health;
        public float healthMaximum => 100.0f;
        public float health
        {
            get => _health;
            set
            {
                _health = Mathf.Min(value, healthMaximum);
                _health = Mathf.Max(0, value);

                if (_health <= 0) Die();
            }
        }

        public Action<RangedWeaponType> OnFilledAmmo;
        public ExtendedWeaponTypeModel<MeleeWeaponType> meleeTypes = new();
        public ExtendedWeaponTypeModel<RangedWeaponType> rangedTypes = new();

        public void Die()
        {
            throw new NotImplementedException();
        }
    }
}