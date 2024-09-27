using Spark.Refactored.Gameplay.Entities.Interfaces;
using Spark.Refactored.Gameplay.Entities.Player.MVC;
using System;
using UnityEngine;

namespace Spark.Refactored.Gameplay.Abilities
{
    [CreateAssetMenu(fileName = "DoubleGunAbility", menuName = "Abilities/Double Gun Ability", order = 78)]
    public class DoubleGunAbility : Ability
    {
        [SerializeField] private View _playerView;

        public DoubleGunAbility()
        {
            // name = "Double Gun";
            description = "Activates the second barrel";

            cooldownDuration = 10.0f;
        }

        public override void InstantiateForPlayer()
        {
            _playerView = FindFirstObjectByType<View>();
        }

        protected override void DoAction()
        {
            Debug.Log("Double Gun Ability, not implemented");
        }
    }
}