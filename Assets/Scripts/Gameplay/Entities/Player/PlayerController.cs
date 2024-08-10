using Spark.Gameplay.Entities.Common.Data;
using Spark.Gameplay.Entities.Enemies;
using Spark.Gameplay.Weapons.MeleeWeapon;
using Spark.Gameplay.Weapons.RangedWeapon;
using Spark.Gameplay.Weapons;
using Spark.Gameplay.Items.Interactable;
using Spark.Gameplay.Items.Pickupable;
using Spark.Utilities;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Spark.Gameplay.Entities.Common;
using Spark.Gameplay.Entities.Common.Abilities;

namespace Spark.Gameplay.Entities.Player
{
    [RequireComponent(
        typeof(PlayerView),
        typeof(CharacterController)
    )]
    public class PlayerController : Actor
    {
        [SerializeField] private PlayerModel _model;
        [SerializeField] private AnimController _animController;
        [SerializeField] private float _distanceView;
        [SerializeField] private PlayerView _view;

        [SerializeField] private InputActionReference _movementAction;
        private Vector2 _movement;

        [SerializeField] private FlashAbility _flashAbility;
        [SerializeField] private InvulnerAbility _invulnerability;
        [SerializeField] private MedKitAbility _medKitAbility;

        [Serializable] public enum MovementDirectionSetting { Up, Left, Right, Down }
        [field: SerializeField] public MovementDirectionSetting MovementDirection { get; set; } 

        private Transform _target;

        public FlashCard.FlashCard FlashCard => _model.FlashCard;

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
            if (_view == null) _view = GetComponent<PlayerView>();

            _flashAbility.Intstantiate(GetComponent<CharacterController>(), transform);
            _invulnerability.Intstantiate(_view);
            _medKitAbility.Intstantiate(_model);

            UpdateActiveWeapon(_model.ActiveWeapon);

            if (_animController.Animator == null) _animController.Animator = GetComponent<Animator>();
            _animController.SwitchAnimForTypeWeapon(_model.ActiveWeapon.Data);

            //_model.AudioSystem.Instalize();
        }

        private void UpdateActiveWeapon(Weapon activeWeapon)
        {
            _view.UpdateActiveWeapon(activeWeapon);
            _view.UpdateActiveWeaponUI(activeWeapon.Data);
        }

        private void Update()
        {
            _movement = _movementAction.action.ReadValue<Vector2>();
            _animController.AnimMove(_movement);

            //if (_movement == Vector2.zero)
            //{
            //    _model.AudioSystem.AudioDictinory["Walk"].mute = true;
            //}

            //else if (_movement != Vector2.zero)
            //{
            //    _model.AudioSystem.AudioDictinory["Walk"].mute = false;
            //}

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
            _model.Move(CalculateDirectionFromSide(MovementDirection));
        }

        private void MovementWithoutTarget()
        {
            _model.Move(CalculateDirectionFromSide(MovementDirection));
            _model.Turn(CalculateDirectionFromSide(MovementDirection));
        }

        private Vector3 CalculateDirectionFromSide(MovementDirectionSetting side)
        {
            switch (side)
            {
            case MovementDirectionSetting.Up: return new Vector3(-_movement.y, .0f, _movement.x);
            case MovementDirectionSetting.Left: return new Vector3(-_movement.x, .0f, -_movement.y);
            case MovementDirectionSetting.Right: return new Vector3(_movement.x, .0f, _movement.y); // ok
            case MovementDirectionSetting.Down: return new Vector3(_movement.y, .0f, -_movement.x);
            }
            return Vector3.zero;
        }
        #endregion

        #region Update player Model and View
        private void SyncModelWithView()
        {
            _flashAbility.Update();
            _invulnerability.Update();
            _medKitAbility.Update();

            UpdateRangedWeaponAmmo();
        }
        private void UpdateRangedWeaponAmmo()
        {
            if (_model.ActiveWeapon.Data is RangedWeaponData)
                _view.UpdateWeaponRangedAmmoUI(_model.ActiveWeapon.Data as RangedWeaponData);
        }
        #endregion

        #region Player abilities
        public void OnFlashAbilityButton(InputAction.CallbackContext context) => _flashAbility.Use();
        public void OnInvulnerabilityButton(InputAction.CallbackContext context) => _invulnerability.Use();
        public void OnMedKitAbilityButton(InputAction.CallbackContext context) => _medKitAbility.Use();
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
            if (_model.ActiveWeapon.Data is RangedWeaponData rangedWeapon)
            {
                if (rangedWeapon.IsAutomatic)
                {
                    if (context.performed) StartShooting(rangedWeapon.FireRate);
                    else if (context.canceled) StopShooting();

                    return;
                }
            }

            if (context.performed)
            {
                DoAttack();
            }
        }

        private void StartShooting(float fireRate)
        {
            InvokeRepeating(nameof(DoAttack), 0f, fireRate);
        }

        private void StopShooting()
        {
            CancelInvoke(nameof(DoAttack));
        }
        //public void MeleAttackEvent() => _model.Attack();

        private void DoAttack()
        {

            if (_animController.GetTypeWeapon())
            {
                _animController.AnimAttack(_model.GetCurrentMeleeWeapon());
                _model.Attack();
            }

            else
            {
                _animController.AnimAttack(_model.GetCurrentRangedWeapon());
                _model.Attack();
            }
        }

        public void OnPlayerReloadWeapon(InputAction.CallbackContext context)
        {
            if (context.performed) _model.ReloadWeapon();
        }
        #endregion
        public void OnPlayerSwitchWeapon(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _model.SwitchWeapon();
                UpdateActiveWeapon(_model.ActiveWeapon);
            }
        }
        public void OnPlayerSwitchWeaponType(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _model.SwitchWeaponType();
                _animController.SwitchAnimForTypeWeapon(_model.ActiveWeapon.Data);
                UpdateActiveWeapon(_model.ActiveWeapon);
            }
        }

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
                    TryActivateInteractableObject(item);
                }
            }
        }

        public bool PickUpMedKit() => _medKitAbility.Add();

        private void OnTriggerEnter(Collider other)
        {
            TryActivateItemTo(other, _model);
        }

        private void TryActivateItemTo(Collider item, PlayerModel player)
        {
            item.GetComponent<IPickupable>()?.Activate(player);
        }
        private void TryActivateInteractableObject(Collider interactableObject)
        {
            interactableObject.GetComponent<IInteractable>()?.Activate();
        }
        #endregion


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {

        }
#endif
    }
}