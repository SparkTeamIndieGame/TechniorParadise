using Spark.Refactored.Gameplay.Entities.Interfaces;
using Spark.Refactored.Gameplay.Entities.Player.MVC;
using System;
using UnityEngine;

namespace Spark.Refactored.Gameplay.Abilities
{
    [CreateAssetMenu(fileName = "ExplosiveShotsAbility", menuName = "Abilities/Explosive Shots Ability", order = 75)]
    public class ExplosiveShotsAbility : Ability
    {
        [SerializeField] private View _playerView;

        public ExplosiveShotsAbility()
        {
            // name = "Explosive Shots";
            description = "";

            cooldownDuration = 10.0f;
        }

        public override void InstantiateForPlayer()
        {
            _playerView = FindFirstObjectByType<View>();
        }

        protected override void DoAction()
        {
            Debug.Log("Explosive Shots Ability, not implemented");
        }
    }
}