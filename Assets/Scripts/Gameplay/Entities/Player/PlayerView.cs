using Spark.Gameplay.Entities.Common.Data;
using Spark.Gameplay.Weapons;
using Spark.UI;
using UnityEngine;

namespace Spark.Gameplay.Entities.Player
{
    public class PlayerView : MonoBehaviour
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

        public void UpdateHealtUI(float health)
        {
            _uiController.UpdatePlayerHealthUI(health);
        }

        public void UpdateActiveWeapon(Weapon weapon)
        {
            Transform weaponTransform = transform.Find("Weapon");

            while (weaponTransform.childCount > 0) 
                DestroyImmediate(weaponTransform.GetChild(0).gameObject);

            Transform firePoint = Instantiate(weapon.Prefab, weaponTransform).transform.Find("FirePoint");
            if (firePoint != null) (weapon as RangedWeapon).SetFirePoint(firePoint);
        }    
        public void UpdateActiveWeaponUI(Weapon weapon)
        {
            _uiController.UpdatePlayerWeaponUI(weapon);
        }

        public void UpdateWeaponRangedAmmoUI(RangedWeapon rangedWeapon)
        {
            _uiController.UpdatePlayerRangedWeaponAmmoUI(rangedWeapon);
        }

        public void SetInvulner(bool enabled)
        {
            foreach (var renderer in _playerMeshes)
            {
                if (renderer != null) renderer.material = enabled ? _invulner : _normal;
            }
            gameObject.layer = SortingLayer.NameToID(enabled ? "Enemy" : "Player");
        }

        public void SetTarget(Transform target)
        {
            // todo!
        }

        public void UpdateTargetHealtUI(IDamagable damagable)
        {
            _uiController.UpdateTargetHealthUI(damagable);
        }
    }
}