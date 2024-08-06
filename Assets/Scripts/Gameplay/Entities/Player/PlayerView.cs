using Spark.Gameplay.Entities.Common.Behaviour;
using Spark.Gameplay.Entities.Common.Data;
using Spark.Gameplay.Weapons.MeleeWeapon;
using Spark.Gameplay.Weapons.RangedWeapon;
using Spark.Gameplay.Weapons;
using Spark.UI;
using System;
using UnityEngine;

namespace Spark.Gameplay.Entities.Player
{
    public class PlayerView : MonoBehaviour, IInvulnerable
    {
        [SerializeField] UIController _uiController;
        
        [SerializeField] Material _normal;
        [SerializeField] Material _invulner;
        MeshRenderer[] _playerMeshes;

        private void Start()
        {
            if (_uiController == null) _uiController = GetComponent<UIController>();

            _playerMeshes = gameObject.GetComponentsInChildren<MeshRenderer>();
        }

        #region Update Player UI
        public void UpdateHealtUI(float health)
        {
            _uiController.UpdatePlayerHealthUI(health);
        }
        #endregion

        #region Update Target UI
        public void UpdateTargetHealtUI(IDamagable damagable)
        {
            _uiController.UpdateTargetHealthUI(damagable);
        }
        #endregion

        #region Change and display active weapon
        public void UpdateActiveWeapon(Weapon weapon)
        {
            Transform weaponTransform = transform.Find("Weapon");
            DestroyChildrenImmediate(weaponTransform);
            InstatiateWeapon(weapon, weapon.Data, weaponTransform);
        }

        private void DestroyChildrenImmediate(Transform parent)
        {
            while (parent.childCount > 0)
            {
                var child = parent.GetChild(0);
                DestroyImmediate(child.gameObject);
            }
        }

        private void InstatiateWeapon(Weapon weapon, WeaponData weaponData, Transform hand)
        {
            Transform firePoint = Instantiate(weaponData.Prefab, hand).transform.Find("FirePoint");
            if (firePoint != null) (weapon as RangedWeapon).SetFirePoint(firePoint);
            else (weapon as MeleeWeapon).SetHandPoint(hand);
        }
        #endregion

        #region Update weapon UI
        public void UpdateActiveWeaponUI(WeaponData weapon)
        {
            _uiController.UpdatePlayerWeaponUI(weapon);
        }

        public void UpdateWeaponRangedAmmoUI(RangedWeaponData rangedWeapon)
        {
            _uiController.UpdatePlayerRangedWeaponAmmoUI(rangedWeapon);
        }
        #endregion

        public void SetInvulner(bool enabled)
        {
            foreach (var renderer in _playerMeshes)
            {
                if (renderer != null) renderer.material = enabled ? _invulner : _normal;
            }
            gameObject.layer = SortingLayer.NameToID(enabled ? "Enemy" : "Player");
        }
    }
}