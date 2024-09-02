using Spark.Gameplay.Entities.RefactoredPlayer.Abilities;
using Spark.Gameplay.Entities.RefactoredPlayer.RefactoredSystems;
using System;
using UnityEngine;

namespace Spark.Gameplay.Entities.RefactoredPlayer
{
    public class RefactoredPlayerModel : IDamagable
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
                _health = Mathf.Min(_health + value, healthMaximum);
                if (_health <= 0) Die();
            }
        }

        public void Die()
        {
            throw new NotImplementedException();
        }
    }
}