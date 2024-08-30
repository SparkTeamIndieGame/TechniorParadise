using System;
using Spark.Gameplay.Entities.Player;
using UnityEngine;
using UnityEngine.UI;

public class ActualDetailCount : MonoBehaviour
{
    [SerializeField] private Text _detailCount;
    [SerializeField] private PlayerController _playerController;

    private void FixedUpdate()
    {
        _detailCount.text = _playerController.GetDetails().ToString();
    }

    public int GetActualDetails()
    {
        return _playerController.GetDetails();
    }
}
