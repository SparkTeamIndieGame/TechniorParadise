using Spark.Gameplay.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Spark.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameObject _mobileUI;
        
        [SerializeField] private Slider _playerHealthBar;
        [SerializeField] private Text _playerWeapon;
        [SerializeField] private Text _playerAmmo;

        private void Awake()
        {
#if UNITY_STANDALONE_WIN
            _mobileUI.SetActive(false);
#elif UNITY_ANDROID || UNITY_IOS
            _mobileUI.SetActive(true);
#endif

            _playerAmmo.enabled = false;
        }

        public void UpdatePlayerHealthUI(float health)
        {
            _playerHealthBar.value = health;
        }

        public void UpdatePlayerWeaponUI(Weapon weapon)
        {
            _playerAmmo.enabled = (weapon is RangedWeapon);            
            _playerWeapon.text = weapon.Name;
        }

        public void UpdatePlayerRangedWeaponAmmoUI(RangedWeapon rangedWeapon)
        {
            _playerAmmo.enabled = true;
            if (rangedWeapon.IsReloading)
                _playerAmmo.text = $"Reloading: {rangedWeapon.ReloadTimeLeft:f1}";
            
            else
                _playerAmmo.text = "Ammo: " + rangedWeapon.Ammo;
        }
    }
}