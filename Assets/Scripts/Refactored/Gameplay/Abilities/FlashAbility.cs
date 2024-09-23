using Spark.Refactored.Gameplay.Entities.Interfaces;
using Spark.Refactored.Gameplay.Entities.Player.MVC;
using System;
using UnityEngine;

namespace Spark.Refactored.Gameplay.Abilities
{
    [CreateAssetMenu(fileName = "FlashAbility", menuName = "Abilities/Flash Ability", order = 71)]
    public class FlashAbility : Ability
    {
        [SerializeField, Range(3.0f, 20.0f)] private float _distance = 5.0f;

        [SerializeField] private CharacterController _controller;
        [SerializeField] private View _playerView;

        public FlashAbility()
        {
            // name = "Flash";
            description = "The player jumps straight";

            cooldownDuration = 5.0f;
        }

        protected override void DoAction()
        {
            var direction = _playerView.direction == Vector3.zero ? _playerView.transform.forward : _playerView.direction;
            _controller.Move(direction * _distance);
        }

        public override void InstantiateForPlayer()
        {
            _playerView = FindFirstObjectByType<View>();
            _controller = _playerView.GetComponent<CharacterController>();
        }
    }
}