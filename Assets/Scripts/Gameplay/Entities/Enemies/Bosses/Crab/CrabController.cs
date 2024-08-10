using Spark.Gameplay.Entities.Common.Data;
using Spark.Gameplay.Entities.Enemies;
using Spark.Gameplay.Weapons;
using Spark.Gameplay.Items.Interactable;
using Spark.Gameplay.Items.Pickupable;
using Spark.Utilities;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Spark.Gameplay.Entities.Common;

namespace Spark.Gameplay.Entities.Enemies.Bosses.Crab
{
    [RequireComponent(
        typeof(CrabView),
        typeof(CharacterController)
    )]
    public class CrabController : Pawn
    {
        [SerializeField] private CrabModel _model;
        [SerializeField] private CrabView _view;
        [SerializeField] private float _distanceView;

        private Transform _target;

        private void OnEnable()
        {
            _model.OnHealthChanged += _view.UpdateHealtUI;
        }
        private void OnDisable()
        {            
            _model.OnHealthChanged -= _view.UpdateHealtUI;
        }

        protected override void AfterStart()
        {

        }

        protected override void AfterUpdate()
        {
            SyncModelWithView();
        }

        private void FixedUpdate()
        {
            if (_model.HasTarget) MovementHasTarget();
            else MovementWithoutTarget();
        }

        public void OnRegisterMeleeWeaponEvent()
        {
            _model.Attack();
        }

        #region C.R.A.B. movement
        private void MovementHasTarget()
        {
            _model.LookAtTarget();
            _model.Move(new Vector3(.0f, .0f, .0f));
        }

        private void MovementWithoutTarget()
        {
            _model.Move(new Vector3(.0f, .0f, .0f));
            _model.Turn(new Vector3(.0f, .0f, .0f));
        }
        #endregion

        #region Update C.R.A.B. Model and View
        private void SyncModelWithView()
        {

        }
        #endregion

        #region C.R.A.B. abilities
        public void UseFlashAbilityButton(InputAction.CallbackContext context) => Debug.Log("Hello, I'm C.R.A.B flash");
       #endregion

        #region C.R.A.B. select target

        public void OnCrabSelectTarget(InputAction.CallbackContext context)
        {

        }
        #endregion

        #region C.R.A.B. Attack system
        public void OnCrabAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _model.Attack();
            }
        }
        #endregion

#if UNITY_EDITOR 
        private void OnDrawGizmos()
        {

        } 
#endif
    }
}