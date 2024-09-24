using Spark.Refactored.Gameplay.Entities.Interfaces;
using Spark.Refactored.Gameplay.Entities.Player.MVC;
using System;
using UnityEngine;

namespace Spark.Refactored.Gameplay.Abilities
{
    [CreateAssetMenu(fileName = "TornadoAbility", menuName = "Abilities/Tornado Ability", order = 74)]
    public class TornadoAbility : Ability
    {
        [SerializeField] private float _rotationSpeed = 360.0f;
        [SerializeField] private float _rotations = 2.0f;
        [SerializeField] private View _playerView;

        private float _currentRotation = .0f;

        public override float cooldown 
        {
            get
            {
                if (!isReady) Rotate();
                return base.cooldown;
            }
        }

        private void OnValidate()
        {
            _currentRotation = 0;
        }

        public TornadoAbility()
        {
            // name = "Tornado";
            description = "";

            cooldownDuration = 10.0f;
        }

        public override void InstantiateForPlayer()
        {
            _playerView = FindFirstObjectByType<View>();
        }

        protected override void DoAction()
        {
            Debug.Log("Tornado Ability, not implemented");
        }

        private void Rotate()
        {
            float rotationThisFrame = _rotationSpeed * Time.deltaTime;
            _currentRotation += rotationThisFrame;

            if (_currentRotation < 360.0f * _rotations)
            {
                _playerView.transform.Rotate(.0f, rotationThisFrame, .0f);
            }
        }
    }
}