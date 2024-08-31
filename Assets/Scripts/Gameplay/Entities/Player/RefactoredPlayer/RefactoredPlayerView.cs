using Spark.Utilities;
using UnityEngine;

namespace Spark.Gameplay.Entities.RefactoredPlayer
{
    public class RefactoredPlayerView : MonoBehaviour
    {
        [SerializeField] private CharacterController _controller;
        
        [SerializeField, Min(.0f)] private float _speed;

        public Vector3 direction { private get; set; }

        private void Start()
        {
            Utils.LoadComponent(gameObject, out _controller);
        }

        private void FixedUpdate()
        {
            _controller.SimpleMove(direction * _speed * Time.fixedDeltaTime);
        }
    }
}