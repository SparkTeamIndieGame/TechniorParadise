using Spark.Gameplay.Entities.Common.Data;
using Spark.Gameplay.Entities.Player;
using System;
using UnityEngine;

namespace Spark.Gameplay.Entities.Common.Abilities
{
    [Serializable]
    public class MedKitAbility : Ability
    {
        [SerializeField] IDamagable _damagable;

        [SerializeField, Min(1)] int _maxAmount = 3;
        [SerializeField, Min(1)] int _amount = 1;
        [SerializeField, Min(20.0f)] float _healingPointsPerUse = 20.0f;

        private MedKitAbility()
        {
            Name = "Med Kit";
            Description = "Healing a player";

            CooldownDuration = 15.0f;
        }

        protected override void DoAction()
        {
            if (_damagable.Health >= _damagable.MaxHealth || _amount <= 0) return;

            _damagable.Heal(_healingPointsPerUse);
            --_amount;
        }

        public void Intstantiate(IDamagable damagable)
        {
            _damagable = damagable;
        }

        public bool Add()
        {
            if (_amount >= _maxAmount) return false;
            
            ++_amount;
            return true;
        }        
    }
}