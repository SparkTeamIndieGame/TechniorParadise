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

        [SerializeField, Min(5.0f)] private float _distance = 5.0f;

        [SerializeField] private CharacterController _controller;
        [SerializeField] private Transform _transform;

        // protected override float CooldownDuration { get => base.CooldownDuration; set => base.CooldownDuration = value; }

        public PlayerFlashAbility(CharacterController controller, Transform transform) 
        { 
            _controller = controller;
            _transform = transform;

            CooldownDuration = 10.0f;
        }
        protected override void DoAction() => _controller.Move(_transform.forward * _distance);
    }
}