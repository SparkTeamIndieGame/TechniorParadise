using UnityEngine;

public class ActivateTMwindow : MonoBehaviour
{
    [SerializeField] private GameObject _healBoxBt;
    [SerializeField] private GameObject _activateBoxBt;
    [SerializeField] private AudioSource _playerInteractiveSours;

    private void OnTriggerEnter(Collider other)
    {
        _playerInteractiveSours.mute = true;
        _healBoxBt.SetActive(false);
        _activateBoxBt.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        _healBoxBt.SetActive(true);
        _activateBoxBt.SetActive(false);
        _playerInteractiveSours.mute = false;
    }
}
