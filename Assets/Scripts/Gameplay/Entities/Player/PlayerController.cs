using Spark.Gameplay.Entities.Common.Data;
using Spark.Gameplay.Entities.Enemies;
using Spark.Gameplay.Items.Interactable;
using Spark.Gameplay.Weapons;
using Spark.Items.Pickups;
using Spark.Utilities;
using System;
using TMPro;
using UnityEditor.AdaptivePerformance.Editor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem.UI;

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
        [SerializeField] private Transform _firePoint;

        [SerializeField, Range(0.5f, 5.0f)] private float _zooming = 2.5f;

        [SerializeField] private InputActionReference _movementAction;
        private Vector2 _movement;

        private Transform _target;

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
            if (_model.HasTarget) MovementHasTarget();
            else MovementWithoutTarget();
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
        private void SyncModelWithView()
        {
            _model.Update();

            UpdateRangedWeaponAmmo();
            // GetPlayerRangeAttack();
        }

        private void UpdateRangedWeaponAmmo()
        {
            if (_model.GetActiveWeapon() is RangedWeapon)
                _view.UpdateWeaponRangedAmmoUI(_model.GetActiveWeapon() as RangedWeapon);
        }

        /*private void GetPlayerRangeAttack()
        {
            RangedWeapon weapon = _model.GetActiveWeapon() as RangedWeapon;
            if (weapon == null || weapon.IsReloading) return;

            if (TryClickOnEnemy(out var enemy))
            {
                Debug.Log(enemy.transform);
                _model.RotateTowards(enemy.point);
            }
        }

        private bool TryClickOnEnemy(out RaycastHit hit)
        {
            hit = default;
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Vector2 screenPosition = Mouse.current.position.ReadValue();
                Ray ray = Camera.main.ScreenPointToRay(screenPosition);
                return Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Enemy"));
            }
            return false;
        }*/

        #region Player abilities
        public void OnFlashAbilityButton(InputAction.CallbackContext context) => _model.UseFlashAbility();
        public void OnInvulnerabilityButton(InputAction.CallbackContext context) => _model.UseInvulnerAbility();
        #endregion

        #region Player select target
        private void GetFromMouseClickPositionTarget(out Transform transform)
        {
            transform = null;

            Vector2 screenPosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            Physics.Raycast(ray, out var hit, Mathf.Infinity, LayerMask.GetMask("Enemy"));

            transform = hit.transform;
        }

        public void OnPlayerSelectTarget(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (_target != null && _target.TryGetComponent(out Enemy enemy))
                    enemy.OnHealthChanged -= _view.UpdateTargetHealtUI;
                
                GetFromMouseClickPositionTarget(out _target);
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

        public void OnPlayerAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                /* IDamagable damagable = null;
                if (_target != null)
                {
                    float distance = Vector3.Distance(_firePoint.position, _target.position);
                    bool inWeaponRange = distance <= _model.GetActiveWeapon().Range;

                    if (inWeaponRange && Physics.Linecast(_firePoint.position, _target.position, out var hit))
                    {
                        hit.transform.TryGetComponent(out damagable);
                    }
                }*/
                _model.Attack(null);
                // _view.UpdateTargetHealtUI(damagable);
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
                _view.UpdateActiveWeaponUI(_model.GetActiveWeapon());
            }
        }
        public void OnPlayerSwitchWeaponType(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _model.SwitchWeaponType();
                _view.UpdateActiveWeaponUI(_model.GetActiveWeapon());

                Camera.main.GetComponent<FollowCamera>()
                    .Zoom(_model.GetActiveWeapon() is RangedWeapon ? +_zooming : -_zooming);
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

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawWireSphere(transform.position, _model.GetActiveWeapon().Range);
            Gizmos.DrawRay(transform.position, transform.forward * _model.GetActiveWeapon().Range);
        }
#endif
    }
}