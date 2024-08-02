using Spark.Gameplay.Entities.Common.Data;
using Spark.Gameplay.Entities.Enemies;
using Spark.Gameplay.Weapons;
using Spark.Gameplay.Items.Interactable;
using Spark.Gameplay.Items.Pickupable;
using Spark.Utilities;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

namespace Spark.Gameplay.Entities.Player
{
    [RequireComponent(
        typeof(PlayerView),
        typeof(CharacterController),
        typeof(Animator)
    )]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerModel _model;
        [SerializeField] private AnimController _animController;
        [SerializeField] private float _distanceView;
        [SerializeField] private PlayerView _view;
        [SerializeField] private Transform _firePoint;

        [SerializeField, Range(0.5f, 5.0f)] private float _zooming = 2.5f;

        [SerializeField] private InputActionReference _movementAction;
        private Vector2 _movement;

        private Transform _target;

        private void OnEnable()
        {
            Enemy.OnEnemyAttack += _model.TakeDamage;

            _model.OnHealthChanged += _view.UpdateHealtUI;
        }
        private void OnDisable()
        {
            Enemy.OnEnemyAttack -= _model.TakeDamage;
            
            _model.OnHealthChanged -= _view.UpdateHealtUI;
        }

        private void Start()
        {
            _view.UpdateActiveWeapon(_model.GetActiveWeapon());
            _view.UpdateActiveWeaponUI(_model.GetActiveWeapon());

            _animController.Animator = GetComponent<Animator>();
            _animController.SwitchAnimForTypeWeapon(_model.GetActiveWeapon());
        }
        private void Update()
        {
            _movement = _movementAction.action.ReadValue<Vector2>();
            _animController.AnimMove(_movement);
            _model.Update();

           


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

        #region Player movement
        private void MovementHasTarget()
        {
            _model.LookAtTarget();
            _model.Move(new Vector3(_movement.x, .0f, _movement.y));
        }

        private void MovementWithoutTarget()
        {
            _model.Move(new Vector3(_movement.x, .0f, _movement.y));
            _model.Turn(new Vector3(_movement.x, .0f, _movement.y));
        }
        #endregion

        #region Update player Model and View
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
        #endregion

        #region Player abilities
        public void OnFlashAbilityButton(InputAction.CallbackContext context) => _model.UseFlashAbility();
        public void OnInvulnerabilityButton(InputAction.CallbackContext context) => _model.UseInvulnerAbility();
        #endregion

        #region Player select target
        private Transform GetTargetFromMouseRightButtonClickPosition()
        {
            Vector2 screenPosition = Vector2.zero;

            if (Mouse.current.rightButton.wasPressedThisFrame)
                screenPosition = Mouse.current.position.ReadValue();
            else if (Touchscreen.current.primaryTouch.tap.wasPressedThisFrame)
                screenPosition = Touchscreen.current.position.ReadValue();
            
            else
                new NullReferenceException("What kind of device is this?");

            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            Physics.Raycast(ray, out var hit, Mathf.Infinity, LayerMask.GetMask("Enemy"));
            return hit.transform;
        }

        public void OnPlayerSelectTarget(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (_target != null && _target.TryGetComponent(out Enemy enemy))
                    enemy.OnHealthChanged -= _view.UpdateTargetHealtUI;

                _target = GetTargetFromMouseRightButtonClickPosition();
                if (_target != null)
                {
                    _model.SetTarget(_target);
                    enemy = _target.GetComponent<Enemy>();

                    enemy.OnHealthChanged += _view.UpdateTargetHealtUI;
                    _view.UpdateTargetHealtUI(_target.GetComponent<IDamagable>());
                }
                else
                {
                    _model.ResetTarget();
                    _view.UpdateTargetHealtUI(null);
                }
            }
        }
        #endregion

        #region Player Attack system
        public void OnPlayerAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _model.Attack();

                if(_animController.GetTypeWeapon())
                {
                    _animController.AnimAttack(_model.GetCurrentMeleeWeapon());
                }

                else
                {
                    _animController.AnimAttack(_model.GetCurrentRangedWeapon());

                }
               // print(_model.GetCurrentMeleeWeapon());
            }
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
                _view.UpdateActiveWeapon(_model.GetActiveWeapon());
                _view.UpdateActiveWeaponUI(_model.GetActiveWeapon());
            }
        }
        public void OnPlayerSwitchWeaponType(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _model.SwitchWeaponType();
                _animController.SwitchAnimForTypeWeapon(_model.GetActiveWeapon());
                _view.UpdateActiveWeapon(_model.GetActiveWeapon());
                _view.UpdateActiveWeaponUI(_model.GetActiveWeapon());

                Camera.main.GetComponent<FollowCamera>()
                    .Zoom(
                        _model.GetActiveWeapon() is MeleeWeapon ? 
                        +_zooming : 
                        -_zooming
                    );
            }
        }
        #endregion

        #region TODO: Items & Interactables
        public void OnPlayerActive(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Collider[] items = Physics.OverlapSphere(
                    transform.position, 
                    1.0f, 
                    Physics.AllLayers, 
                    QueryTriggerInteraction.Collide
                );
                foreach (Collider item in items) 
                { 
                    TryActivateItemTo(item, _model);
                    TryActivateInteractableObject(item);
                }
            }
        }

        private void TryActivateItemTo(Collider item, PlayerModel player)
        {
            item.GetComponent<IPickupable>()?.Activate();
        }
        private void TryActivateInteractableObject(Collider interactableObject)
        {
            interactableObject.GetComponent<IInteractable>()?.Activate();
        }
        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var playerCenter = transform.position;
            var playerAttackRadius = _model.GetActiveWeapon().Range;

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(playerCenter, playerAttackRadius);
        }
#endif
    }
}