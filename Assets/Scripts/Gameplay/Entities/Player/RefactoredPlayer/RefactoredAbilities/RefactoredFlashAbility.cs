using System;
using UnityEngine;

namespace Spark.Gameplay.Entities.RefactoredPlayer.Abilities
{
    [Serializable]
    public class RefactoredFlashAbility : RefactoredAbility
    {
        [SerializeField, Range(3.0f, 20.0f)] private float _distance = 5.0f;

        [SerializeField] private CharacterController _controller;

        public Vector3 direction { private get; set; }

        public RefactoredFlashAbility()
        {
            name = "Flash";
            description = "The player jumps straight";

            cooldownDuration = 5.0f;
        }

        protected override void DoAction() => _controller.Move(direction * _distance);

        public void Intstantiate(CharacterController controller)
        { 
            _controller = controller;
        }
    }
}