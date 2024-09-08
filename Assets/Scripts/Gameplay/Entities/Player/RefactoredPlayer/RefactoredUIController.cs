using Spark.Gameplay.Entities.RefactoredPlayer.Abilities;
using Spark.Gameplay.Entities.RefactoredPlayer.RefactoredSystems;
using Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Weapons;
using Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Weapons.Ranged;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Spark.Gameplay.Entities.RefactoredPlayer.UI
{
    public class RefactoredUIController : MonoBehaviour
    {
        [SerializeField] private Image _flashIcon;
        [SerializeField] private Image _invulnerIcon;
        [SerializeField] private Image _meditIcon;

        [SerializeField] private Text _flashDriveCount;
        [SerializeField] private Text _flashDriveMaximum;

        [SerializeField] private Text _detailsCount;

        [SerializeField] private Image _reloadIcon;
        [SerializeField] private Text _ammo;
        [SerializeField] private Text _ammoMaximum;

        [SerializeField] private Slider _playerHealthBar;

        #region Abilities & reloading icons
        public void UpdateFlashIcon(RefactoredFlashAbility ability)
        {
            StartCoroutine(UpdateIconCoroutine(
                _flashIcon, 
                () => !ability.isReady, 
                () => ability.cooldown, 
                ability.cooldownDuration
            ));
        }

        public void UpdateInvulnerIcon(RefactoredInvulnerAbility ability)
        {
            StartCoroutine(UpdateIconCoroutine(
                _invulnerIcon,
                () => !ability.isReady,
                () => ability.cooldown,
                ability.cooldownDuration
            ));
        }

        public void UpdateMedKitIcon(RefactoredMedKitAbility ability)
        {
            StartCoroutine(UpdateIconCoroutine(
                _meditIcon,
                () => !ability.isReady,
                () => ability.cooldown,
                ability.cooldownDuration,
                () => !TryToggleMedKitIcon(ability)
            ));
        }

        public void UpdateReloadingIcon(RefactoredRangedWeapon weapon)
        {
            StartCoroutine(UpdateIconCoroutine(
                _meditIcon,
                () => !weapon.isReloading,
                () => weapon.reloading,
                weapon.reloadingDuration,
                () => !TryToggleReloadIcon(weapon)
            ));
        }

        public bool TryToggleMedKitIcon(RefactoredMedKitAbility ability)
        {
            return _meditIcon.enabled = ability.amount > 0;
        }

        public bool TryToggleReloadIcon(bool toggle)
        {
            return _reloadIcon.enabled = toggle;
        }

        private IEnumerator UpdateIconCoroutine(Image icon, Func<bool> condition, Func<float> cooldown, float duration)
        {
            WaitForEndOfFrame waitForEndOfFrame = new();
            while (condition())
            {
                icon.fillAmount = 1.0f - cooldown() / duration;
                yield return waitForEndOfFrame;
            }
            icon.fillAmount = 1.0f;
        }

        private IEnumerator UpdateIconCoroutine(Image icon, Func<bool> condition, Func<float> cooldown, float duration, Func<bool> toggleIconCondition)
        {
            if (toggleIconCondition()) yield return null;
            yield return UpdateIconCoroutine(icon, condition, cooldown, duration);
        }
        #endregion

        public void UpdateFlashDriveUI(FlashDrive flashDrive)
        {
            _flashDriveCount.text = flashDrive.count.ToString();
            _flashDriveMaximum.text = flashDrive.maximum.ToString();
        }

        public void UpdateDetailsUI(float details)
        {
            _detailsCount.text = details.ToString();
        }

        public void UpdatePlayerHealthUI(float health)
        {
            _playerHealthBar.value = health;
        }

        public void UpdateAmmoUI(RefactoredRangedWeapon weapon)
        {

        }
    }
}