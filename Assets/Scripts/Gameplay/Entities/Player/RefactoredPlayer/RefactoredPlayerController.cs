using Spark.Gameplay.Entities.RefactoredPlayer.InputSystem;
using UnityEngine;

namespace Spark.Gameplay.Entities.RefactoredPlayer
{
    public class RefactoredPlayerController
    {
        private RefactoredPlayerModel _model;
        private RefactoredPlayerView _view;

        private RefactoredPlayerInputActions _inputActions = new();

        public RefactoredPlayerController(RefactoredPlayerModel model, RefactoredPlayerView view)
        {
            _model = model;
            _view = view;

            InitializeAbilities(_view);
            RegisterInputActions();

            _view.OnHealthUpdated += (points) =>
            {
                _model.health += points;
            };

            RegisterPickupHandlers();

            _model.health = 50.0f;
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

            RegisterFlashAbilityHandler();
            RegisterInvulnerAbilityHandler();
            RegisterMedKitAbilityHandler();
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
                    _view.inspection = hit.point - _view.transform.position + Vector3.zero;
                }
            };
        }

        private void RegisterFlashAbilityHandler()
        {
            _inputActions.Player.AbilityFlash.performed += (context) =>
            {
                _view.OnFlashActivated.Invoke(_model.flashAbility);
            };
        }

        private void RegisterInvulnerAbilityHandler()
        {
            _inputActions.Player.AbilityInvulner.performed += (context) =>
            {
                _view.OnInvulnerActivated.Invoke(_model.invulnerAbility);
            };
        }

        private void RegisterMedKitAbilityHandler()
        {
            _inputActions.Player.AbilityMedKit.performed += (context) =>
            {
                _view.OnMedKitActivated.Invoke(_model.medkitAbility);
            };
        }
        #endregion

        #region 
        private void RegisterPickupHandlers()
        {
            _view.OnFlashDrivePickUped = () =>
            {
                _model.flashDrive.Add();
                _view.UpdateFlashDriveUI(_model.flashDrive);
            };

            _view.OnDetailsPickUped = (count) =>
            {
                _model.details += count;
                _view.UpdateDetailsUI(_model.details);
            };

            _view.OnMedKitPickUped = (pickup) =>
            {
                if (_model.medkitAbility.TryCollect())
                {
                    pickup.Deactivate();
                    _view.TryToggleMedKitIcon(_model.medkitAbility);
                }
            };
        }
        #endregion

        private void SyncViewWithModel()
        {
            _view.UpdateFlashDriveUI(_model.flashDrive);
            _view.UpdateDetailsUI(_model.details);
        }
    }
}