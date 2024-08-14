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
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using static UnityEngine.EventSystems.EventTrigger;

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
            _view.NeedCardUI(_model.FlashCard.MaxAmount);

            _flashAbility.Intstantiate(GetComponent<CharacterController>(), transform);
            _invulnerability.Intstantiate(_view);
            _medKitAbility.Intstantiate(_model);

            UpdateActiveWeapon(_model.ActiveWeapon);

            if (_animController.Animator == null) _animController.Animator = GetComponent<Animator>();
            _animController.SwitchAnimForTypeWeapon(_model.ActiveWeapon.Data);

            _model.AudioSystem.Instalize();
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
            _view.UpdateCardUI(FlashCard);

            if (_movement == Vector2.zero)
            {
                _model.AudioSystem.AudioDictinory["Walk"].mute = true;
            }

            else if (_movement != Vector2.zero)
            {
                _model.AudioSystem.AudioDictinory["Walk"].mute = false;
            }

            if(_model.Health <= 0)
            {
                _animController.Animator.SetTrigger("Dead");
                this.enabled = false;
            }

            if(_model.ActiveWeapon.Data is RangedWeaponData reload)
            {
                if (reload.IsReloading)
                    _animController.ReloadAnim(true);
                else
                    _animController.ReloadAnim(false);
            }
            


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
        public void OnPlayerSelectTarget(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (IsPointerOnUI(context.ReadValue<Vector2>())) return;

                if (_target != null && _target.TryGetComponent(out Enemy enemy))
                    enemy.OnHealthChanged -= _view.UpdateTargetHealtUI;

                _target = GetTargetFromClickOrTapPosition(context, out _);
                SetEnemyTargetWithUI(_target);
            }
        }

        private void SetEnemyTargetWithUI(Transform target)
        {
            if (target != null)
            {
                _target = target;

                _model.SetTarget(_target);
                Enemy enemy = _target.GetComponent<Enemy>();

                enemy.OnHealthChanged += _view.UpdateTargetHealtUI;
                _view.UpdateTargetHealtUI(_target.GetComponent<IDamagable>());
            }
            else
            {
                _model.ResetTarget();
                _view.UpdateTargetHealtUI(null);
            }
        }

        private Transform GetTargetFromClickOrTapPosition(InputAction.CallbackContext context, out RaycastHit hit)
        {
            Ray ray = Camera.main.ScreenPointToRay(context.ReadValue<Vector2>());
            Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Enemy"));
            return hit.transform;
        }

        private bool IsPointerOnUI(Vector2 touchPosition)
        {
            PointerEventData pointerEventData = new(EventSystem.current)
            {
                position = touchPosition
            };

            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(pointerEventData, results);

            return results.Count > 0;
        }
        #endregion

        #region Player Attack system
        private List<Collider> _enemiesInAttackRange = new();
        public void OnPlayerAttack(InputAction.CallbackContext context)
        {
            if (_model.ActiveWeapon.Data is RangedWeaponData rangedWeapon)
            {
                if (_target == null)
                {
                    ClearAllNullPointerOfEnemies();
                    ClearAllOutRangeEnemies();
                    FillArrayOfInRangeEnemies();

                    if (_enemiesInAttackRange.Count > 0)
                    {
                        SetEnemyTargetWithUI(_enemiesInAttackRange[0].transform);
                    }
                }

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
        private void ClearAllNullPointerOfEnemies()
        {
            _enemiesInAttackRange.RemoveAll(enemyCollider => enemyCollider == null);
        }

        private void ClearAllOutRangeEnemies()
        {
            _enemiesInAttackRange.RemoveAll(enemy => Vector3.Distance(enemy.transform.position, transform.position) > _model.ActiveWeapon.Data.AttackRange);
        }

        private void FillArrayOfInRangeEnemies()
        {
            _enemiesInAttackRange = Physics.OverlapSphere(_model.ActiveWeapon.transform.position, _distanceView, LayerMask.GetMask("Enemy")).ToList();
        }

        private void StartShooting(float fireRate)
        {
            InvokeRepeating(nameof(DoAttack), 0f, fireRate);
        }

        private void StopShooting()
        {
            CancelInvoke(nameof(DoAttack));
        }

        public void Hit() => _model.Attack();

        private void DoAttack()
        {

            if (_animController.GetTypeWeapon())
            {
                _animController.AnimAttack();
                //_model.Attack();
            }

            else
            {
                _animController.AnimAttack();
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
            _model.AudioSystem.AudioDictinory["TakeItem"].Play();
        }
        private void TryActivateInteractableObject(Collider interactableObject)
        {
            interactableObject.GetComponent<IInteractable>()?.Activate();
            _model.AudioSystem.AudioDictinory["Interact"].Play();
        }
        #endregion


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {

        }
#endif
    }
}