using UnityEngine;

namespace Spark.Gameplay.Entities.Player
{
    [RequireComponent(
        typeof(PlayerView),

        typeof(CharacterController)
    )]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed = 100.0f;
        [SerializeField] private float _rotationSpeed = 100.0f;

        [SerializeField] private PlayerModel _model;
        [SerializeField] private PlayerView _view;
        [SerializeField] private PlayerInput _input;

        private void Awake()
        {
            CharacterController controller = GetComponent<CharacterController>();

            _model = new PlayerModel(controller, transform);
        }

        private void Update()
        {
            HandleMovement();
            HandleRotation();
        }

        private void HandleMovement()
        {
            float moveInput = Input.GetAxis("Vertical");
            Vector3 moveDirection = transform.forward * moveInput * Time.deltaTime * _movementSpeed;
            // _controller.Move(moveDirection);
        }

        private void HandleRotation()
        {
            float rotateInput = Input.GetAxis("Horizontal");
            Vector3 rotateDirection = Vector3.up * rotateInput * Time.deltaTime * _rotationSpeed;
            transform.Rotate(rotateDirection);
        }
    }
}