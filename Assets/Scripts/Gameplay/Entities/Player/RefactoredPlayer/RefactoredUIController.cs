using Spark.Gameplay.Entities.RefactoredPlayer.Abilities;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Spark.Gameplay.Entities.RefactoredPlayer.UI
{
    public class RefactoredUIController : MonoBehaviour
    {
        [SerializeField] private Image _flashIcon;
        [SerializeField] private Image _invulnerIcon;

        private void Start()
        {

        }

        public IEnumerator UpdateFlashIcon(FlashAbility ability)
        {
            while (!ability.isReady)
            {
                _flashIcon.fillAmount = 1.0f - ability.cooldown / ability.cooldownDuration;
                yield return new WaitForEndOfFrame();
            }
            _flashIcon.fillAmount = 1.0f;
            yield break;
        }

        public IEnumerator UpdateInvulnerIcon(InvulnerAbility ability)
        {
            while (!ability.isReady)
            {
                _invulnerIcon.fillAmount = 1.0f - ability.cooldown / ability.cooldownDuration;
                yield return new WaitForEndOfFrame();
            }
            _invulnerIcon.fillAmount = 1.0f;
            yield break;
        }
    }
}