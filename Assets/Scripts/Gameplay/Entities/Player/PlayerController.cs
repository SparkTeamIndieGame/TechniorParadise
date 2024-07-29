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
        [SerializeField] private float _distanceView;
        [SerializeField] private PlayerView _view;
        [SerializeField] private Transform _firePoint;

        [SerializeField, Range(0.5f, 5.0f)] private float _zooming = 2.5f;

        [SerializeField] private InputActionReference _movementAction;
        private Vector2 _movement;
        private Camera mainCamera;
        private RaycastHit hit;
        private bool tracking;

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

            mainCamera = Camera.main;
        }
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                if(Physics.Raycast(ray, out hit))
                {
                    print(hit.transform.name);

                    if(hit.transform.gameObject.GetComponent<Enemy>())
                    {
                        tracking = true;
                    }
                }

            }
            _movement = _movementAction.action.ReadValue<Vector2>();
            _model.Update();

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

            if(tracking && Vector3.Distance(transform.position, hit.transform.position) < _distanceView)
            {
                Transform target = hit.transform;
                target.position = new Vector3(hit.transform.position.x, 0, hit.transform.position.z);
                transform.LookAt(target);
            }

            else
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
        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _distanceView);
        }
#endif

    }
}