using UnityEngine;

public class ActivateDeactivateBT : MonoBehaviour
{
    [SerializeField] private GameObject _healBoxBt;
    [SerializeField] private GameObject _activateBoxBt;

    private void OnTriggerEnter(Collider other)
    {
        _healBoxBt.SetActive(false);
        _activateBoxBt.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        _healBoxBt.SetActive(true);
        _activateBoxBt.SetActive(false);
    }
}
