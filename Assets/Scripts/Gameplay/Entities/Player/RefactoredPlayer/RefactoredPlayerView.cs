using Spark.Gameplay.Entities.Common.Behaviour;
using Spark.Gameplay.Entities.RefactoredPlayer.Abilities;
using Spark.Gameplay.Entities.RefactoredPlayer.UI;
using Spark.Utilities;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Spark.Gameplay.Entities.RefactoredPlayer
{
    public class RefactoredPlayerView : MonoBehaviour, IInvulnerable
    {
        public Action<FlashAbility> OnFlashActivated;
        public Action<InvulnerAbility> OnInvulnerActivated;

        [SerializeField] private RefactoredUIController _ui;

        [SerializeField] private CharacterController _controller;
        
        [SerializeField, Min(.0f)] private float _movementSpeed;
        [SerializeField, Min(.0f)] private float _rotationSpeed;

        public Vector3 direction { private get; set; }
        public Vector3 inspection { private get; set; }

        private void Start()
        {
            Utils.LoadComponent(gameObject, out _ui);
            Utils.LoadComponent(gameObject, out _controller);

            RegisterAbilities();
        }

        void RegisterAbilities()
        {
            OnFlashActivated = (ability) =>
            {
                ability.direction = direction == Vector3.zero ? transform.forward : direction;
                ability.Activate();

                StartCoroutine(_ui.UpdateFlashIcon(ability));
            };

            OnInvulnerActivated = (ability) =>
            {
                ability.Activate();
                StartCoroutine(_ui.UpdateInvulnerIcon(ability));
            };
        }

        private void FixedUpdate()
        {
            HandleMovement();
            HandleInspection();
        }

        void HandleMovement()
        {
            _controller.SimpleMove(direction * _movementSpeed * Time.fixedDeltaTime);
        }

        void HandleInspection()
        {
            if (inspection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(inspection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
            }
        }

        public void SetInvulner(bool toggle)
        {
            var meshes = gameObject.GetComponentsInChildren<MeshRenderer>();

            foreach (var renderer in meshes)
            {
                var color = renderer.material.color;
                if (toggle)
                {
                    renderer.material.SetFloat("_Mode", 3.0f);

                    renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    renderer.material.SetInt("_ZWrite", 0);

                    renderer.material.DisableKeyword("_ALPHATEST_ON");
                    renderer.material.EnableKeyword("_ALPHABLEND_ON");
                    renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");

                    renderer.material.renderQueue = 3000;

                    color.a = 0.1f;
                }
                else
                {
                    renderer.material.SetFloat("_Mode", 0.0f);

                    renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    renderer.material.SetInt("_ZWrite", 1);

                    renderer.material.EnableKeyword("_ALPHATEST_ON");
                    renderer.material.DisableKeyword("_ALPHABLEND_ON");
                    renderer.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");

                    renderer.material.renderQueue = 2000;

                    color.a = 1.0f;
                }
                renderer.material.color = color;
            }
            gameObject.layer = SortingLayer.NameToID(enabled ? "Enemy" : "Player");
        }
    }
}