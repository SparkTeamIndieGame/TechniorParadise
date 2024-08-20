using UnityEngine;
using UnityEngine.InputSystem;


public class LoadingWindow : MonoBehaviour
{
    
    public ShopBaseControl newActiveTrader;
    
    public GameObject loadingWindow;
    [SerializeField] private PlayerInput _playerInput;
    
    private void OnTriggerEnter(Collider other)
    {
        _playerInput.enabled = false;
        InteractivShopBt._activeTMan = newActiveTrader;
        Time.timeScale = 0.0f;
        loadingWindow.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        this.gameObject.SetActive(false);
    }

    
    

  
}
