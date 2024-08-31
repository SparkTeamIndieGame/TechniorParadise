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
        }

        #region Initialize abilities
        private void InitializeAbilities(MonoBehaviour gameObject)
        {
            var controller = gameObject.GetComponent<CharacterController>();

            _model.flashAbility.Intstantiate(controller);
            _model.invulnerAbility.Intstantiate(_view);
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
                    _view.inspection = hit.point - _view.transform.position + Vector3.up;
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
        #endregion
    }
}