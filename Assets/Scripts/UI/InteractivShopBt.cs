
using System;
using Spark.Gameplay.Weapons;
using UnityEngine;
using UnityEngine.UI;


public class InteractivShopBt : MonoBehaviour
{
    
    //[SerializeField] private WeaponData _weaponData;
    [SerializeField] private Button _buyButton;
    [SerializeField] private ActualDetailCount _actualDetailCount;
    [SerializeField] private Text _priceText;
    [SerializeField] private Image _itemImage;
    [SerializeField] private int _numberElement;

    private ShopBaseControl _targetMan;


    
    


    private void OnEnable()
    {
        _targetMan = FirstTM.ActualTM;
    }

    private void Start()
    {
        UpdateShopItem(_numberElement);
    }
    
   

    private void UpdateShopItem(int number)
    {
        switch (number)
        {
            case 1:
                _priceText.text = _targetMan._rangedWeapons[0].Price.ToString();
                _itemImage.sprite = _targetMan._rangedWeapons[0].Icon;
                CheckPrice(_targetMan._rangedWeapons[0]);
                break;
            case 2:
                _priceText.text = _targetMan._rangedWeapons[1].Price.ToString();
                _itemImage.sprite = _targetMan._rangedWeapons[1].Icon;
                CheckPrice(_targetMan._rangedWeapons[1]);
                break;
            case 3:
                _priceText.text = _targetMan._meleeWeapon.Price.ToString();
                _itemImage.sprite = _targetMan._meleeWeapon.Icon;
                CheckPrice(_targetMan._meleeWeapon);
                break;
            default:
                var actualCount = _actualDetailCount.GetActualDetails();
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
                return;
        }
    }
    public void CheckPrice(WeaponData weaponData)
    {
        var actualCount = _actualDetailCount.GetActualDetails();
        
        
            if (actualCount >= weaponData.Price)
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
