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

        [SerializeField] private Text _flashDriveCount;
        [SerializeField] private Text _flashDriveMaximum;

        [SerializeField] private Text _detailsCount;

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
    }
}