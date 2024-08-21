
using System;
using UnityEngine;

public class FirstTM : MonoBehaviour
{
    public ShopBaseControl FirstTMBase;
    public static ShopBaseControl ActualTM;

    private void Awake()
    {
        ActualTM = FirstTMBase;
    }
    private void Start()
    {
        ActualTM = FirstTMBase;
    }
}
