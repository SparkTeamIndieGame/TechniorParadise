using Spark.Utilities;
using UnityEngine;

namespace Spark.Gameplay.Entities.RefactoredPlayer
{
    public class RefactoredPlayerView : MonoBehaviour
    {
        [SerializeField] private CharacterController _controller;
        
        [SerializeField, Min(.0f)] private float _movementSpeed;
        [SerializeField, Min(.0f)] private float _rotationSpeed;

        public Vector3 direction { private get; set; }
        public Vector3 inspection { private get; set; }

        private void Start()
        {
            Utils.LoadComponent(gameObject, out _controller);
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
    }
}