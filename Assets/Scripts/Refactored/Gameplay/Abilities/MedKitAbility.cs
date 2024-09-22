using Spark.Gameplay.Entities.Common.Behaviour;
using Spark.Refactored.Gameplay.Entities.Interfaces;
using Spark.Refactored.Gameplay.Entities.Player.MVC;
using System;
using UnityEngine;

namespace Spark.Refactored.Gameplay.Abilities
{
    [CreateAssetMenu(fileName = "MedKitAbility", menuName = "Abilities/Med Kit Ability", order = 73)]
    public class MedKitAbility : Ability
    {
        private IHealthable _healthable;

        [SerializeField] public int maximum { get; private set; } = 5;
        [SerializeField] public int amount { get; private set; } = 2;
        [SerializeField] public float healingPointsPerUse => 20.0f;

        public MedKitAbility()
        {
            // name = "Med Kit";
            description = "Healing a IHealthable";

            cooldownDuration = 8.0f;
        }

        public void Intstantiate(IHealthable healthable)
        {
            _healthable = healthable;
        }

        public override void InstantiateForPlayer()
        {
            _healthable = FindAnyObjectByType<View>();
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