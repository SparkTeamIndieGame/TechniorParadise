
using UnityEngine;

public class FirstTM : MonoBehaviour
{
    public ShopBaseControl FirstTMBase;

    private void Awake()
    {
        InteractivShopBt._activeTMan = FirstTMBase;
    }
}
