using Spark.Gameplay.Items.Interactable;
using Spark.Gameplay.Weapons;
using Spark.Items.Pickups;
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

        private void OnEnable()
        {
            _model.OnHealthChanged += _view.UpdateHealtUI;
        }

        private void OnDisable()
        {
            _model.OnHealthChanged -= _view.UpdateHealtUI;
        }

        private void Start()
        {
            _view.UpdateActiveWeaponUI(_model.GetActiveWeapon());
        }

        private void Update()
        {
            _movement = _movementAction.action.ReadValue<Vector2>();

            SyncModelWithView();
        }

        private void FixedUpdate()
        {
            _model.Move(new Vector3(_movement.x, .0f, _movement.y));
            _model.Turn(new Vector3(_movement.x, .0f, _movement.y));
        }

        private void SyncModelWithView()
        {
            _model.Update();
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
                _view.UpdateActiveWeaponUI(_model.GetActiveWeapon());
            }
        }
        public void OnPlayerSwitchWeaponType(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _model.SwitchWeaponType();
                _view.UpdateActiveWeaponUI(_model.GetActiveWeapon());
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            TryActivateItemTo(other, _model);
            TryActivateInteractableObject(other);
        }

        private void TryActivateItemTo(Collider item, PlayerModel player)
        {
            item.GetComponent<IPickupable>()?.Activate();
        }
        private void TryActivateInteractableObject(Collider interactableObject)
        {
            interactableObject.GetComponent<IInteractable>()?.Activate();
        }
    }
}