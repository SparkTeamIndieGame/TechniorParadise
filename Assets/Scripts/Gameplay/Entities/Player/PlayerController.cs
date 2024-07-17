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

        [SerializeField] private InputActionReference _movementAction;
        private Vector2 _movement;


        private void Awake()
        {

        }

        private void Update()
        {
            _movement = _movementAction.action.ReadValue<Vector2>();
            _model.UpdateAbilities();
        }

        private void FixedUpdate()
        {
            _model.Move(new Vector3(_movement.x, .0f, _movement.y));
            _model.Turn(new Vector3(_movement.x, .0f, _movement.y));
        }

        public void OnFlashAbilityButton(InputAction.CallbackContext context) => _model.UseFlashAbility();
        public void OnInvulnerabilityButton(InputAction.CallbackContext context) => _model.UseInvulnerAbility();
    }
}