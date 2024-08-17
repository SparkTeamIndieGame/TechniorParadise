
using System;
using Spark.Gameplay.Weapons;
using UnityEngine;
using UnityEngine.UI;


public class InteractivShopBt : MonoBehaviour
{
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private Button _buyButton;
    [SerializeField] private ActualDetailCount _actualDetailCount;
    [SerializeField] private Text _priceText;
    [SerializeField] private Image _itemImage;

    private void Start()
    {
        if (_weaponData != null)
        {
            _priceText.text = _weaponData.Price.ToString();
        }
        if (_itemImage != null)
        {
            _itemImage.sprite = _weaponData.Icon;
        }
        
        CheckPrice();
    }

    public void CheckPrice()
    {
        var actualCount = _actualDetailCount.GetActualDetails();
        if (_weaponData != null)
        {
            if (actualCount >= _weaponData.Price)
            {
                _buyButton.interactable = true;
                _priceText.color = Color.green;
            }
            else
            {
                _buyButton.interactable = false;
                _priceText.color = Color.red;
            }
        }
        else
        {
            if (actualCount >= int.Parse(_priceText.text))
            {
                _buyButton.interactable = true;
                _priceText.color = Color.green;
            }
            else
            {
                _buyButton.interactable = false;
                _priceText.color = Color.red;
            }
        }
    }
    
}
