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

            RegisterMovementHandler();
        }

        #region Initialize Player input actions
        void RegisterMovementHandler()
        {
            _inputActions.Player.Movement.Enable();
            _inputActions.Player.Movement.performed += (context) => { _view.direction = context.ReadValue<Vector3>(); };
        }
        #endregion
    }
}