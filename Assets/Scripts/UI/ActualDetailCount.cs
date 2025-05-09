using System;
using Spark.Gameplay.Entities.Player;
using UnityEngine;
using UnityEngine.UI;

public class ActualDetailCount : MonoBehaviour
{
    [SerializeField] private Text _detailCount;
    [SerializeField] private PlayerController _playerController;

    private void Start()
    {
        _detailCount.text = GetActualDetails().ToString();
    }

    private void OnEnable()
    {
        _detailCount.text = GetActualDetails().ToString();
    }

    private void FixedUpdate()
    {
        _detailCount.text = _playerController.GetDetails().ToString();
        print(GetActualDetails().ToString());
    }

    public int GetActualDetails()
    {
        return _playerController.GetDetails();
    }
}
