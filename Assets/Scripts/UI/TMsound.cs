using System.Collections;
using UnityEngine;

public class TMsound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _openShop;
    [SerializeField] private AudioClip[] _closedShop;
    [SerializeField] private AudioClip _buyItemShop;

    public void PlayOpenShop()
    {
        _audioSource.PlayOneShot(_openShop[Random.Range(0, _openShop.Length)]);
    }
    public void PlayClosedShop(AudioSource globalAudioSource)
    {
        var clip = _closedShop[Random.Range(0, _closedShop.Length)];
        _audioSource.PlayOneShot(clip);
        StartCoroutine(WaitEndSound(globalAudioSource,clip.length));
    }

    public void PlayBuyItemShop()
    {
        _audioSource.PlayOneShot(_buyItemShop);
    }

    private IEnumerator WaitEndSound(AudioSource audioSource, float soundTime)
    {
        yield return new WaitForSeconds(soundTime);
        audioSource.mute = false;
    }
    
}
