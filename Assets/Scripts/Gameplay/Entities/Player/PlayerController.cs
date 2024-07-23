using Spark.Gameplay.Weapons;
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

        private void Start()
        {
            _view.UpdateWeaponUI(_model.GetActiveWeapon());
        }

        private void Update()
        {
            _movement = _movementAction.action.ReadValue<Vector2>();
            _model.Update();

            SyncModelWithView();
        }

        private void FixedUpdate()
        {
            _model.Move(new Vector3(_movement.x, .0f, _movement.y));
            _model.Turn(new Vector3(_movement.x, .0f, _movement.y));
        }

        private void SyncModelWithView()
        {
            UpdateRangedWeaponAmmo();
        }

        private void UpdateRangedWeaponAmmo()
        {
            if (_model.GetActiveWeapon() is RangedWeapon)
                _view.UpdateWeaponRangedAmmoUI(_model.GetActiveWeapon() as RangedWeapon);
        }

        public void OnFlashAbilityButton(InputAction.CallbackContext context) => _model.UseFlashAbility();
        public void OnInvulnerabilityButton(InputAction.CallbackContext context) => _model.UseInvulnerAbility();

        public void OnPlayerAttack(InputAction.CallbackContext context)
        {
            if (context.performed) _model.Attack(null);
        }
        public void OnPlayerReloadWeapon(InputAction.CallbackContext context)
        {
            if (context.performed) _model.ReloadWeapon();
        }
        public void OnPlayerSwitchWeapon(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _model.SwitchWeapon();
                _view.UpdateWeaponUI(_model.GetActiveWeapon());
            }
        }
        public void OnPlayerSwitchWeaponType(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _model.SwitchWeaponType();
                _view.UpdateWeaponUI(_model.GetActiveWeapon());
            }
        }
    }
}