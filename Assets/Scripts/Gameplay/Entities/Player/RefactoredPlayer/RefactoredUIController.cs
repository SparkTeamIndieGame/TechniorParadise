using Spark.Gameplay.Entities.RefactoredPlayer.Abilities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
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
            while (!ability.IsReady)
            {
                _flashIcon.fillAmount = 1.0f - ability.Cooldown / ability.CooldownDuration;
                yield return new WaitForEndOfFrame();
            }
            _flashIcon.fillAmount = 1.0f;
            yield break;
        }

        public IEnumerator UpdateInvulnerIcon(InvulnerAbility ability)
        {
            while (!ability.IsReady)
            {
                _invulnerIcon.fillAmount = 1.0f - ability.Cooldown / ability.CooldownDuration;
                yield return new WaitForEndOfFrame();
            }
            _invulnerIcon.fillAmount = 1.0f;
            yield break;
        }
    }
}