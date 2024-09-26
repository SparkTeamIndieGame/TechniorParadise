using Spark.Gameplay.Entities.Common.Behaviour;
using Spark.Gameplay.Entities.RefactoredPlayer;
using Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Items;
using Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Weapons;
using Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Weapons.Melee;
using Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Weapons.Ranged;
using Spark.Refactored.Gameplay.Entities.Interfaces;
using Spark.Refactored.Gameplay.System.Checkpoint;
using Spark.Utilities;
using System;
using UnityEngine;

namespace Spark.Refactored.Gameplay.Entities.Player.MVC
{
    public class View : MonoBehaviour, IHealthable, IInvulnerable
    {
        public Action OnFlashDrivePickUped;
        public Action<float> OnDetailsPickUped;
        public Action<MedKitPickup> OnMedKitPickUped;
        public Action OnCheckpointPickUped;

        private CharacterController _controller;

        private RefactoredRangedWeapon _rangedWeapon;
        private RefactoredMeleeWeapon _meleeWeapon;
        private IRefactoredWeapon _activeWeapon;

        [SerializeField, Min(.0f)] private float _movementSpeed;
        [SerializeField, Min(.0f)] private float _rotationSpeed;

        public event Action<float> OnHealthChanged;

        public Vector3 direction { get; set; }
        public Vector3 inspection { private get; set; }

        public float health { set => OnHealthChanged.Invoke(value); }

        private bool AbilityReady => _meleeWeapon.AbilityReady && _rangedWeapon.AbilityReady;

        private void Start()
        {
            Utils.LoadComponent(gameObject, out _controller);

            InitializeWeaponSystem();
        }

        private void FixedUpdate()
        {
            HandleMovement();
            HandleInspection();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Items.ICollectable pickupable))
            {
                pickupable.Activate(this);
            }
        }

        private void InitializeWeaponSystem()
        {
            _rangedWeapon = FindAnyObjectByType<RefactoredRangedWeapon>();
            _meleeWeapon = FindAnyObjectByType<RefactoredMeleeWeapon>();

            _activeWeapon = _rangedWeapon;
            _meleeWeapon.DisableAllGameObjectWeapons();


        }

        #region Movement and inspection
        void HandleMovement()
        {
            _controller.SimpleMove(direction * _movementSpeed * Time.fixedDeltaTime);
        }

        void HandleInspection()
        {
            if (inspection != Vector3.zero)
            {
                var targetByAxisY = Quaternion.LookRotation(inspection);
                targetByAxisY.x = targetByAxisY.z = 0.0f;

                transform.rotation = Quaternion.Slerp(transform.rotation, targetByAxisY, _rotationSpeed * Time.fixedDeltaTime);
            }
        }
        #endregion

        #region Invulnerability
        public void SetInvulner(bool toggle)
        {
            var meshes = gameObject.GetComponentsInChildren<MeshRenderer>();

            foreach (var renderer in meshes)
            {
                // Debug.Log(renderer.name);

                var color = renderer.material.color;
                if (toggle)
                {
                    renderer.material.SetFloat("_Mode", 3.0f);

                    renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    renderer.material.SetInt("_ZWrite", 0);

                    renderer.material.DisableKeyword("_ALPHATEST_ON");
                    renderer.material.EnableKeyword("_ALPHABLEND_ON");
                    renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");

                    renderer.material.renderQueue = 3000;

                    color.a = 0.1f;
                }
                else
                {
                    renderer.material.SetFloat("_Mode", 0.0f);

                    renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    renderer.material.SetInt("_ZWrite", 1);

                    renderer.material.EnableKeyword("_ALPHATEST_ON");
                    renderer.material.DisableKeyword("_ALPHABLEND_ON");
                    renderer.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");

                    renderer.material.renderQueue = 2000;

                    color.a = 1.0f;
                }
                renderer.material.color = color;
            }
            gameObject.layer = SortingLayer.NameToID(enabled ? "Enemy" : "Player");
        }
        #endregion

        #region Change Weapon System
        public void ChangeMeleeWeapon(WeaponTypeModel<MeleeWeaponType> type)
        {
            if (!AbilityReady) return;

            if (_activeWeapon is RefactoredMeleeWeapon)
                _meleeWeapon.ChangeWeapon(type.next);

            else
            {
                _rangedWeapon.DisableAllGameObjectWeapons();
                _meleeWeapon.ChangeWeapon(type.current);
                _activeWeapon = _meleeWeapon;
            }
        }
        public void ChangeRangedeWeapon(WeaponTypeModel<RangedWeaponType> type)
        {
            if (!AbilityReady) return;

            if (_activeWeapon is RefactoredRangedWeapon)
                _rangedWeapon.ChangeWeapon(type.next);

            else
            {
                _meleeWeapon.DisableAllGameObjectWeapons();
                _rangedWeapon.ChangeWeapon(type.current);
                _activeWeapon = _rangedWeapon;
            }
        }

        public void ChangeWeaponCategory(WeaponTypeModel<MeleeWeaponType> melee, WeaponTypeModel<RangedWeaponType> ranged)
        {
            if (!AbilityReady) return;

            if (_activeWeapon is RefactoredRangedWeapon)
            {
                _rangedWeapon.DisableAllGameObjectWeapons();
                _meleeWeapon.ChangeWeapon(melee.current);
                _activeWeapon = _meleeWeapon;
            }
            else
            {
                _meleeWeapon.DisableAllGameObjectWeapons();
                _rangedWeapon.ChangeWeapon(ranged.current);
                _activeWeapon = _rangedWeapon;
            }
        }

        public void ChangeWeaponType(WeaponTypeModel<MeleeWeaponType> melee, WeaponTypeModel<RangedWeaponType> ranged)
        {
            if (!AbilityReady) return;

            if (_activeWeapon is RefactoredRangedWeapon)
            {
                _rangedWeapon.ChangeWeapon(ranged.next);
                _activeWeapon = _rangedWeapon;
            }
            else
            {
                _meleeWeapon.ChangeWeapon(melee.next);
                _activeWeapon = _meleeWeapon;
            }
        }

        public void TryReloadWeapon()
        {
            if (_activeWeapon is RefactoredRangedWeapon)
                _rangedWeapon.Reload();
        }
        #endregion

        public void ActivateWeapon() => _activeWeapon.Activate();
        public void DeactivateWeapon() => _activeWeapon.Deactivate();

        public void ActivateWeaponAbility() => _activeWeapon.ActivateAbility();

        public void FillAmmo(RangedWeaponType weaponType)
        {
            _rangedWeapon.FillAmmo(weaponType);
        }
    }
}