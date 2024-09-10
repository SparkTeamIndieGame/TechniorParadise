using Spark.Gameplay.Entities.RefactoredPlayer.Abilities;
using Spark.Gameplay.Entities.RefactoredPlayer.RefactoredSystems;
using Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Weapons.Melee;
using Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Weapons.Ranged;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spark.Gameplay.Entities.RefactoredPlayer
{
    public class RefactoredPlayerModel : IDamagable // todo: ScriptableObject, 
    {
        public RefactoredFlashAbility flashAbility { get; } = new();
        public RefactoredInvulnerAbility invulnerAbility { get; } = new();
        public RefactoredMedKitAbility medkitAbility { get; } = new();

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
        public WeaponTypeModelExtended<MeleeWeaponType> meleeTypes = new();
        public WeaponTypeModelExtended<RangedWeaponType> rangedTypes = new();

        public void Die()
        {
            throw new NotImplementedException();
        }
    }

    public class WeaponTypeModel<Type> where Type : System.Enum
    {
        protected List<Type> _data = new();
        protected int _current;

        public Type current => _data[_current % _data.Count];
        public Type next => _data[++_current % _data.Count];
    }

    public class WeaponTypeModelExtended<Type> : WeaponTypeModel<Type> where Type : System.Enum
    {
        public bool TryAddNewType(Type type)
        {
            bool success = _data.Contains(type);
            if (!success) _data.Add(type);
            return !success; // inversion
        }
    }
}