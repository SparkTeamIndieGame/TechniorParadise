using UnityEngine;
using UnityEngine.InputSystem;

public class UnScaledGame : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    public void UnscaledGame()
    {
        Time.timeScale = 1.0f;
        _playerInput.enabled = true;
        this.gameObject.SetActive(false);
    }
}
