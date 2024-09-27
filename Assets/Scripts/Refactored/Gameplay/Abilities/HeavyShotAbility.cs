using Spark.Refactored.Gameplay.Entities.Interfaces;
using Spark.Refactored.Gameplay.Entities.Player.MVC;
using System;
using UnityEngine;

namespace Spark.Refactored.Gameplay.Abilities
{
    [CreateAssetMenu(fileName = "HeavyShotAbility", menuName = "Abilities/Heavy Shot Ability", order = 78)]
    public class HeavyShotAbility : Ability
    {
        [SerializeField] private View _playerView;

        public HeavyShotAbility()
        {
            // name = "Heavy Shot";
            description = "Triple Cone Punch";

            cooldownDuration = 10.0f;
        }

        public override void InstantiateForPlayer()
        {
            _playerView = FindFirstObjectByType<View>();
        }

        protected override void DoAction()
        {
            Debug.Log("Heavy Shot Ability, not implemented");
        }
    }
}