using Spark.Gameplay.Entities.RefactoredPlayer.Abilities;
using Spark.Gameplay.Entities.RefactoredPlayer.RefactoredSystems;
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

        [SerializeField] private Slider _playerHealthBar;

        #region Abilities
        public IEnumerator UpdateFlashIconCoroutine(FlashAbility ability)
        {
            while (!ability.isReady)
            {
                _flashIcon.fillAmount = 1.0f - ability.cooldown / ability.cooldownDuration;
                yield return new WaitForEndOfFrame();
            }
            _flashIcon.fillAmount = 1.0f;
            yield break;
        }

        public IEnumerator UpdateInvulnerIconCoroutine(InvulnerAbility ability)
        {
            while (!ability.isReady)
            {
                _invulnerIcon.fillAmount = 1.0f - ability.cooldown / ability.cooldownDuration;
                yield return new WaitForEndOfFrame();
            }
            _invulnerIcon.fillAmount = 1.0f;
            yield break;
        }
        public IEnumerator UpdateMedKitIconCoroutine(MedKitAbility ability)
        {
            if (!TryToggleMedKitIcon(ability)) yield return null;

            while (!ability.isReady)
            {
                _meditIcon.fillAmount = 1.0f - ability.cooldown / ability.cooldownDuration;
                yield return new WaitForEndOfFrame();
            }
            _meditIcon.fillAmount = 1.0f;
        }

        public bool TryToggleMedKitIcon(MedKitAbility ability)
        {
            _meditIcon.enabled = ability.amount > 0;
            return _meditIcon.enabled;
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
            _playerHealthBar.value += health;
        }
    }
}