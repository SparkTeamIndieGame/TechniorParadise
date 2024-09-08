using System;
using UnityEngine;

namespace Spark.Gameplay.Entities.RefactoredPlayer.Abilities
{
    [Serializable]
    public class RefactoredMedKitAbility : RefactoredAbility
    {
        private IHealthable _healthable;

        [SerializeField] public int maximum { get; private set; } = 5;
        [SerializeField] public int amount { get; private set; } = 2;
        [SerializeField] public float healingPointsPerUse => 20.0f;

        public RefactoredMedKitAbility()
        {
            name = "Med Kit";
            description = "Healing a IHealthable";

            cooldownDuration = 8.0f;
        }

        public void Intstantiate(IHealthable healthable)
        {
            _healthable = healthable;
        }

        protected override void DoAction()
        {
            if (amount <= 0) return;
            
            _healthable.health = healingPointsPerUse;
            --amount;
        }
        
        public bool TryCollect()
        {
            if (amount >= maximum) return false;
            
            ++amount;
            return true;
        }
    }
}