using Spark.Gameplay.Entities.Common.Data;
using Spark.Gameplay.Weapons.MeleeWeapon;
using Spark.Gameplay.Weapons.RangedWeapon;
using Spark.Gameplay.Weapons;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Spark.Gameplay.Entities.Common.Abilities;
using Spark.Gameplay.Entities.Player;
using System;
using Spark.Gameplay.Items.Pickupable;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Spark.UI
{
    public class UIController : MonoBehaviour
    {
        
        [SerializeField] private GameObject _mobileUI;

        #region Player UI
        [SerializeField] private Slider _playerHealthBar;
        [SerializeField] private Text _playerWeapon;
        [SerializeField] private Text _playerAmmo;
        //Andrei
        [SerializeField] private Image _rangeWeaponIcon;
        [SerializeField] private Image _meleWeaponIcon;
        [SerializeField] private Image _reloadIcon;
        [SerializeField] private Image _medKitIcon;
        [SerializeField] private Text _currentAmmoText;
        [SerializeField] private Text _maxAmmoText;
        
        [SerializeField] private Text _currentDetails;
        #endregion

        #region Target UI
        [SerializeField] private GameObject _targetUI;
        [SerializeField] private Slider _targetHealthBar;
        #endregion

        #region Boss UI
        [SerializeField] private GameObject _bossUI;
        [SerializeField] private Slider _bossHealthBar;
        #endregion

        #region Menu UI
        [SerializeField] private GameObject _restartUI;
        #endregion

        private void Awake()
        {
//#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
//            _mobileUI.SetActive(false);
//#elif UNITY_ANDROID || UNITY_IOS
//            _mobileUI.SetActive(true);
//#endif

            _playerAmmo.enabled = false;
            _restartUI.SetActive(false);
        }

        public void UpdatePlayerHealthUI(float health)
        {
            _playerHealthBar.value = health;
            if(_playerHealthBar.value <= 0)
            {
                _restartUI.SetActive(true);
            }
        }

        public void UpdatePlayerDetailsUI(int details)
        {
            _currentDetails.text = details.ToString();
        }

        public void UpdatePlayerWeaponUI(WeaponData weapon)
        {
            _playerAmmo.enabled = (weapon is RangedWeaponData);            
            //_playerWeapon.text = weapon.Name;
            if (weapon is RangedWeaponData)
            {
                _rangeWeaponIcon.sprite = weapon.Icon;

                _rangeWeaponIcon.color = Color.white;
                _meleWeaponIcon.color = Color.gray;
            }
            else if (weapon is MeleeWeaponData)
            {
                _meleWeaponIcon.sprite = weapon.Icon;

                _meleWeaponIcon.color = Color.white;
                _rangeWeaponIcon.color = Color.gray;
            }
        }

        public void UpdatePlayerRangedWeaponAmmoUI(RangedWeaponData rangedWeapon)
        {
            _playerAmmo.enabled = true;
            _maxAmmoText.text = rangedWeapon.AmmoMax.ToString();
            
            if (rangedWeapon.IsReloading)
            {
                _reloadIcon.fillAmount = rangedWeapon.ReloadTimeLeft / rangedWeapon.ReloadDuration;
                _currentAmmoText.text = rangedWeapon.Ammo.ToString();
            }
            //_playerAmmo.text = $"Reloading: {rangedWeapon.ReloadTimeLeft:f1}";

            else
            {
                _reloadIcon.fillAmount = 1.0f;
                _currentAmmoText.text = rangedWeapon.Ammo.ToString();
            }
                
                //_playerAmmo.text = "Ammo: " + rangedWeapon.Ammo;
        }

        public void UpdatePlayerMedKitButtonUI(MedKitAbility medKit)
        {
            if (!medKit.IsReady) 
                _medKitIcon.fillAmount = medKit.Cooldown / medKit.CooldownDuration;

            else _medKitIcon.fillAmount = 1.0f;
        }
        private void UpdateHealthUI(IDamagable damagable, ref GameObject target, ref Slider bar)
        {
            if (damagable == null)
            {
                target.SetActive(false);
                return;
            }

            bar.maxValue = damagable.MaxHealth;
            bar.value = damagable.Health;
            target.SetActive(damagable.Health > 0);
            
        }
        
        public void UpdateTargetHealthUI(IDamagable damagable) => UpdateHealthUI(damagable, ref _targetUI, ref _targetHealthBar);
        public void UpdateBossHealthUI(IDamagable damagable) => UpdateHealthUI(damagable, ref _bossUI, ref _bossHealthBar);
    }
}