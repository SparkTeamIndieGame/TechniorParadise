using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class Settings : MonoBehaviour
{
    [SerializeField] private Slider total, music, sound;
    [SerializeField] AudioMixer soundGroup;

    private void Start()
    {
        Load();
    }

    public void OpenSettingAudio()
    {
        StartCoroutine(OpenSetting());
    }

    public void CloseSettingAudio()
    {
        StopAllCoroutines();
    }

    private IEnumerator OpenSetting()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            soundGroup.SetFloat("Master", total.value);
            soundGroup.SetFloat("Music", music.value);
            soundGroup.SetFloat("Sound", sound.value);
            Save();
        }
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("Total", total.value);
        PlayerPrefs.SetFloat("Music", music.value);
        PlayerPrefs.SetFloat("Sound", sound.value);
    }

    private void Load()
    {
        total.value = PlayerPrefs.GetFloat("Total");
        music.value = PlayerPrefs.GetFloat("Music");
        sound.value = PlayerPrefs.GetFloat("Sound");

        soundGroup.SetFloat("Master", total.value);
        soundGroup.SetFloat("Music", music.value);
        soundGroup.SetFloat("Sound", sound.value);
    }
}
