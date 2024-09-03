using Spark.Gameplay.Entities.RefactoredPlayer.Abilities;
using System;
using UnityEngine;

namespace Spark.Gameplay.Entities.Common.Abilities
{
    [Serializable]
    public class FlashAbility : Ability
    {
        [SerializeField, Range(3.0f, 20.0f)] private float _distance = 5.0f;

        [SerializeField] private CharacterController _controller;
        [SerializeField] private Transform _transform;

        private FlashAbility()
        {
            Name = "Flash";
            Description = "The player jumps straight";

            CooldownDuration = 5.0f;
        }

        protected override void DoAction() => _controller.Move(_transform.forward * _distance);

        public void Intstantiate(CharacterController controller, Transform transform)
        { 
            _controller = controller;
            _transform = transform;
        }
    }
}