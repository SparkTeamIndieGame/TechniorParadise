using Spark.Gameplay.Entities.Common;
using System;
using UnityEngine;

namespace Spark.Gameplay.Entities.Player.Abilities
{
    [Serializable]
    public class PlayerFlashAbility : Ability
    {
        [SerializeField] public override string Name => "Flash";
        [SerializeField] public override string Description => "The player jumps straight";

        [SerializeField] private float _distance;

        [SerializeField] private CharacterController _controller;
        [SerializeField] private Transform _transform;

        public PlayerFlashAbility(CharacterController controller, Transform transform) 
        { 
            _controller = controller;
            _transform = transform;
        }
        protected override void DoAction() => _controller.Move(_transform.forward * _distance);
    }
}