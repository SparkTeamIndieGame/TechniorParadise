using Spark.Gameplay.Entities.RefactoredPlayer.InputSystem;
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

            _inputActions.Enable();

            RegisterMovementHandler();
            RegisterInspectionHandler();
        }

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
        #endregion
    }
}