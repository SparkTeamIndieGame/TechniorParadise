using Spark.Gameplay.Entities.RefactoredPlayer.InputSystem;
using Spark.Gameplay.Entities.RefactoredPlayer.UI;
using Spark.Utilities;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.InputSystem;

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

            _inputActions.Enable();

            RegisterMovementHandler();
            RegisterInspectionHandler();

            RegisterFlashAbilityHandler();
            RegisterInvulnerAbilityHandler();
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
        void RegisterMovementHandler()
        {
            _inputActions.Player.Movement.performed += (context) => { _view.direction = context.ReadValue<Vector3>(); };
        }
        void RegisterInspectionHandler()
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

        void RegisterFlashAbilityHandler()
        {
            _inputActions.Player.AbilityFlash.performed += (context) =>
            {
                _view.OnFlashActivated.Invoke(_model.flashAbility);
            };
        }

        void RegisterInvulnerAbilityHandler()
        {
            _inputActions.Player.AbilityInvulner.performed += (context) =>
            {
                _view.OnInvulnerActivated.Invoke(_model.invulnerAbility);
            };
        }
        #endregion
    }
}