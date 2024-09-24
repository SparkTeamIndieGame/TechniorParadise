using Spark.Refactored.Gameplay.Entities.Interfaces;
using Spark.Refactored.Gameplay.Entities.Player.MVC;
using System;
using UnityEngine;

namespace Spark.Refactored.Gameplay.Abilities
{
    [CreateAssetMenu(fileName = "FuryAbility", menuName = "Abilities/Fury Ability", order = 76)]
    public class FuryAbility : Ability
    {
        [SerializeField] private View _playerView;

        public FuryAbility()
        {
            // name = "Fury";
            description = "Increases attack speed";

            cooldownDuration = 10.0f;
        }

        public override void InstantiateForPlayer()
        {
            _playerView = FindFirstObjectByType<View>();
        }

        protected override void DoAction()
        {
            Debug.Log("Fury Ability, not implemented");
        }
    }
}