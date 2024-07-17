using UnityEngine;
using UnityEngine.InputSystem;

namespace Spark.Gameplay.Entities.Player
{
    [RequireComponent(
        typeof(PlayerView),

        typeof(CharacterController)
    )]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerModel _model;
        [SerializeField] private PlayerView _view;

        [SerializeField] private InputActionReference _moveInput;
        [SerializeField] private InputActionReference _turnInput;

        private float _axisMovement;
        private float _axisTurning;

        private void Awake()
        {

        }

        private void Update()
        {
            _axisMovement = _moveInput.action.ReadValue<float>();
            _axisTurning = _turnInput.action.ReadValue<float>();
        }

        private void FixedUpdate()
        {
            _model.Move(_axisMovement);
            _model.Turn(_axisTurning);
        }
    }
}