using Spark.Gameplay.Entities.Common.Abilities;
using Spark.Gameplay.Entities.RefactoredPlayer.InputSystem;
using Spark.Gameplay.Entities.RefactoredPlayer.UI;
using Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Weapons.Melee;
using Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Weapons.Ranged;
using System;
using UnityEditor.Playables;
using UnityEngine;

namespace Spark.Gameplay.Entities.RefactoredPlayer
{
    public class RefactoredPlayerController
    {
        private readonly RefactoredPlayerModel _model;
        private readonly RefactoredPlayerView _view;
        private readonly RefactoredUIController _ui;

        private readonly RefactoredPlayerInputActions _inputActions = new();

        public RefactoredPlayerController(RefactoredPlayerModel model, RefactoredPlayerView view, RefactoredUIController ui)
        {
            _model = model;
            _view = view;
            _ui = ui;

            _model.health = _model.healthMaximum;

            InitializeAbilities(_view);
            RegisterInputActions();

            RegisterHealthHandler();

            RegisterPickupHandlers();

            _model.meleeTypes.TryAddNewType(MeleeWeaponType.Knife);
            _model.meleeTypes.TryAddNewType(MeleeWeaponType.Spear);
            _model.meleeTypes.TryAddNewType(MeleeWeaponType.CircularSaw);

            _model.rangedTypes.TryAddNewType(RangedWeaponType.Pistol);
            _model.rangedTypes.TryAddNewType(RangedWeaponType.SubmachineGun);
            _model.rangedTypes.TryAddNewType(RangedWeaponType.AssaultRifle);

            SyncViewWithModel();
        }

        #region Initialize abilities
        private void InitializeAbilities(MonoBehaviour gameObject)
        {
            var controller = gameObject.GetComponent<CharacterController>();

            _model.flashAbility.Intstantiate(controller);
            _model.invulnerAbility.Intstantiate(_view);
            _model.medkitAbility.Intstantiate(_view);
        }
        #endregion

        #region Initialize Player input actions
        private void RegisterInputActions()
        {
            _inputActions.Enable();

            RegisterMovementHandler();
            RegisterInspectionHandler();

            RegisterFlashAbilityActivateHandler();
            RegisterInvulnerAbilityActivateHandler();
            RegisterMedKitAbilityActivateHandler();

            RegisterWeaponActivateHandler();
            RegisterWeaponChangeHandler();
            RegisterWeaponReloadHandler();
        }

        private void RegisterMovementHandler()
        {
            _inputActions.Player.Movement.performed += (context) => { _view.direction = context.ReadValue<Vector3>(); };
        }
        private void RegisterInspectionHandler()
        {
            _inputActions.Player.Inspection.performed += (context) =>
            {
                Vector2 cursorPosition = context.ReadValue<Vector2>();
                Ray ray = Camera.main.ScreenPointToRay(cursorPosition);

                if (Physics.Raycast(ray, out var hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
                {
                    _view.inspection = (hit.point - _view.transform.position);
                }
            };
        }

        private void RegisterFlashAbilityActivateHandler()
        {
            _inputActions.Player.AbilityFlashActivate.performed += (context) =>
            {
                var direction = _view.direction == Vector3.zero ? _view.transform.forward : _view.direction;
                _model.flashAbility.direction = direction;
                _model.flashAbility.Activate();
                _ui.UpdateFlashIcon(_model.flashAbility);
            };
        }
        private void RegisterInvulnerAbilityActivateHandler()
        {
            _inputActions.Player.AbilityInvulnerActivate.performed += (context) =>
            {
                _model.invulnerAbility.Activate();
                _ui.UpdateInvulnerIcon(_model.invulnerAbility);
            };
        }
        private void RegisterMedKitAbilityActivateHandler()
        {
            _inputActions.Player.AbilityMedKitActivate.performed += (context) =>
            {
                if (_model.medkitAbility.amount <= 0) return;

                _model.medkitAbility.Activate();
                _ui.UpdateMedKitIcon(_model.medkitAbility);
            };
        }

        private void RegisterWeaponActivateHandler()
        {
            _inputActions.Player.WeaponActivate.performed += (context) => _view.ActivateWeapon();
            _inputActions.Player.WeaponActivate.canceled += (context) => _view.DeactivateWeapon();
        }

        private void RegisterWeaponChangeHandler()
        {
            _inputActions.Player.WeaponChangeMeleeType.performed += (context) =>
            {
                _view.ChangeMeleeWeapon(_model.meleeTypes);
            };

            _inputActions.Player.WeaponChangeRangedType.performed += (context) =>
            {
                _view.ChangeRangedeWeapon(_model.rangedTypes);
            };

            _inputActions.Player.WeaponChangeCategory.performed += (context) =>
            {
                _view.ChangeWeaponCategory(_model.meleeTypes, _model.rangedTypes);
            };

            _inputActions.Player.WeaponChangeType.performed += (context) =>
            {
                _view.ChangeWeaponType(_model.meleeTypes, _model.rangedTypes);
            };
        }

        private void RegisterWeaponReloadHandler()
        {
            _inputActions.Player.WeaponReload.performed += (context) =>
            {
                _view.TryReloadWeapon();
            };
        }
        #endregion

        private void RegisterHealthHandler()
        {
            _view.OnHealthChanged += (points) =>
            {
                _model.health += points;
                _ui.UpdatePlayerHealthUI(_model.health);
            };
        }

        #region Initialie pickup handlers
        private void RegisterPickupHandlers()
        {
            RegisterFlashDrivePickupHandler();
            RegisterDetailsPickupHandler();
            RegisterMedKitPickupHandler();
        }

        private void RegisterFlashDrivePickupHandler()
        {
            _view.OnFlashDrivePickUped = () =>
            {
                _model.flashDrive.Add();
                _ui.UpdateFlashDriveUI(_model.flashDrive);
            };
        }
        private void RegisterDetailsPickupHandler()
        {
            _view.OnDetailsPickUped = (count) =>
            {
                _model.details += count;
                _ui.UpdateDetailsUI(_model.details);
            };
        }
        private void RegisterMedKitPickupHandler()
        {
            _view.OnMedKitPickUped = (pickup) =>
            {
                if (!_model.medkitAbility.TryCollect()) return;

                pickup.Deactivate();
                _ui.TryToggleMedKitIcon(_model.medkitAbility);
            };
        }
        #endregion

        private void SyncViewWithModel()
        {
            _ui.UpdatePlayerHealthUI(_model.health);
            _ui.UpdateFlashDriveUI(_model.flashDrive);
            _ui.UpdateDetailsUI(_model.details);
        }
    }
}