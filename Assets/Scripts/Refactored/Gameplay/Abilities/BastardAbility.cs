using Spark.Refactored.Gameplay.Entities.Interfaces;
using Spark.Refactored.Gameplay.Entities.Player.MVC;
using System;
using UnityEngine;

namespace Spark.Refactored.Gameplay.Abilities
{
    [CreateAssetMenu(fileName = "BastardAbility", menuName = "Abilities/Bastard Ability", order = 77)]
    public class BastardAbility : Ability
    {
        [SerializeField] private View _playerView;

        public BastardAbility()
        {
            // name = "Bastard";
            description = "Instantly reloads weapon";

            cooldownDuration = 10.0f;
        }

        public override void InstantiateForPlayer()
        {
            _playerView = FindFirstObjectByType<View>();
        }

        protected override void DoAction()
        {
            Debug.Log("Bastard Ability, not implemented");
        }
    }
}