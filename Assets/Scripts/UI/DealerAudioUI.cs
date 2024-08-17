using UnityEngine;

public class DealerAudioUI : MonoBehaviour
{
    [SerializeField] private AudioSource _gameplaySound;
    [SerializeField] private TMsound _dealerSound;
    private void OnEnable()
    {
        _gameplaySound.mute = true;
        _dealerSound.PlayOpenShop();
    }

    private void OnDisable()
    {
        _dealerSound.PlayClosedShop(_gameplaySound);
    }
}
