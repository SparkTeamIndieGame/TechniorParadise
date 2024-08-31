using Spark.Gameplay.Entities.RefactoredPlayer.Abilities;
using System;
using UnityEngine;

namespace Spark.Gameplay.Entities.RefactoredPlayer.Abilities
{
    [Serializable]
    public class FlashAbility : RefactoredAbility
    {
        [SerializeField, Range(3.0f, 20.0f)] private float _distance = 5.0f;

        [SerializeField] private CharacterController _controller;

        public Vector3 direction { private get; set; }

        public FlashAbility()
        {
            Name = "Flash";
            Description = "The player jumps straight";

            CooldownDuration = 5.0f;
        }

        protected override void DoAction() => _controller.Move(direction * _distance);

        public void Intstantiate(CharacterController controller)
        { 
            _controller = controller;
        }
    }
}