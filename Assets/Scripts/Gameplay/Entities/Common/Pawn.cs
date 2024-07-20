using Spark.Gameplay.Entities.Common.Behaviour;
using UnityEngine;

namespace Spark.Gameplay.Entities.Common
{
    public class Pawn : Actor, IMovable, ITurnable
    {
        [SerializeField] private CharacterController _controller;
        
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _turnSpeed;

        public void Move(Vector3 direction)
        {
            _controller.SimpleMove(direction * _moveSpeed * Time.deltaTime);
        }

        public void Turn(Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _turnSpeed * Time.deltaTime);
            }
        }
    }
}