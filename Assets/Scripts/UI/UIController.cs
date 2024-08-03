using Spark.Gameplay.Entities.Common.Data;
using Spark.Gameplay.Weapons;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Spark.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameObject _mobileUI;

        #region Player UI
        [SerializeField] private Slider _playerHealthBar;
        [SerializeField] private Text _playerWeapon;
        [SerializeField] private Text _playerAmmo;
        #endregion

        #region Target UI
        [SerializeField] private GameObject _targetUI;
        [SerializeField] private Slider _targetHealthBar;
        #endregion

        #region Boss UI
        [SerializeField] private GameObject _bossUI;
        [SerializeField] private Slider _bossHealthBar;
        #endregion

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