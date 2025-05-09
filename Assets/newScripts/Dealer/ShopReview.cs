using Spark.Gameplay.Weapons;
using System.Collections.Generic;
using Spark.Gameplay.Entities.Player;
using Spark.Gameplay.Weapons.MeleeWeapon;
using Spark.Gameplay.Weapons.RangedWeapon;
using UnityEngine;
using UnityEngine.UI;

public class ShopReview : MonoBehaviour
{
    [SerializeField] private GameObject _uiShop;
    [SerializeField] private Text _detailCountShowText;
    //[SerializeField] private PseudoPlayer _pseudoPlayer;

    [SerializeField] private PlayerController _playerC;
    private PlayerModel _player;
    [SerializeField] private List<Image> _items_imagies;

    [SerializeField] private List<Text> _items_price;
    [SerializeField] private Sprite _alreadyExistsWeapon;
    [SerializeField] private Sprite _notAlreadyExistsWeapon;
    [Space(10)]
    [Header("Range Info Panel")]
    [SerializeField] private GameObject _infoPanelRangeWeapon;
    [SerializeField] private Image _titleInfoRange;
    [SerializeField] private Image _damageCharacteristic;
    [SerializeField] private Image _reloadCharacteristic;
    [SerializeField] private Image _amunitionCountCharacteristic;
    [SerializeField] private Image _shootPeroidCharacteristic;
    [SerializeField] private Image _abilityRangeIcon;
    [SerializeField] private Text _abilityRangeName;
    [SerializeField] private Text _abilityRangeDescription;

    [SerializeField] private Button _buyButtonRange;

    [Space(10)]
    [Header("Melee Info Panel")]
    [SerializeField] private GameObject _infoPanelMeleeWeapon;
    [SerializeField] private Image _titleInfoMelee;
    [SerializeField] private Image _damageCharacteristicMelee;
    [SerializeField] private Image _attackSpeedCharacteristicMelee;
    [SerializeField] private Image _attackRadiusCountCharacteristicMelee;
    [SerializeField] private Image _abilityMeleeIcon;
    [SerializeField] private Text _abilityMeleeName;
    [SerializeField] private Text _abilityMeleeDescription;

    [SerializeField] private Button _buyButtonMelee;

    [Space(10)] [Header("Right Panel")] 
    [SerializeField] private Text _flashDriverPrice;
    [SerializeField] private Text _healthBoxPrice;
    [SerializeField] private Button _buyButtonOtherFlash;
    [SerializeField] private Button _buyButtonOtherHealth;
    
    [Space(10)] [Header("Almanach")] 
    [SerializeField] private GameObject _almanach;
    private MeleeWeaponData currentMeleeWeapon;
    private RangedWeaponData currentRangeWeapon;
    private WeaponData chooseWeaponData;
    private void Start()
    {
        _player = _playerC.GetComponent<PlayerModel>();
        UpdateShopItems.OnUpdate += ShowShopItem;
    }

    private void OnEnable()
    {
        ShowShopItem();
    }
    private void OnDestroy()
    {
        UpdateShopItems.OnUpdate -= ShowShopItem;
    }
    private Color ColorCheeck(int price, int count)
    {
        if (price > count)
        {
            return Color.red;
        }
     
        return Color.green;
    }
    private bool PriceCheeck(int price, int count)
    {
        if (price > count)
        {
            return false;
        }
     
        return true;
    }
    private void UpdateDetailCountShow()
    {
        //_detailCountShowText.text = _pseudoPlayer.Details.ToString();
        _detailCountShowText.text = _player.Details.ToString();
    }
    private void ShowShopItem() //тут сделать проверку
    {
        UpdateDetailCountShow();
        int temleCount = 0;
        for (int i = 0; i < UpdateShopItems.WeaponDataList.Count; i++)
        {
            _items_imagies[i].sprite = UpdateShopItems.WeaponDataList[i].Icon;
            _items_price[i].text = UpdateShopItems.WeaponDataList[i].Price.ToString();
            _items_price[i].color = ColorCheeck(UpdateShopItems.WeaponDataList[i].Price, _player.Details);
            temleCount = i;
        }

        _items_imagies[temleCount+1].sprite = UpdateShopItems.MeleeWeapon.Icon;
        _items_price[temleCount + 1].text = UpdateShopItems.MeleeWeapon.Price.ToString();
        _items_price[temleCount + 1].color = ColorCheeck(UpdateShopItems.MeleeWeapon.Price, _player.Details);
        
        _flashDriverPrice.color = ColorCheeck(int.Parse(_flashDriverPrice.text), _player.Details);
        _buyButtonOtherFlash.interactable = PriceCheeck(int.Parse(_flashDriverPrice.text), _player.Details);
        _healthBoxPrice.color = ColorCheeck(int.Parse(_healthBoxPrice.text), _player.Details);
        _buyButtonOtherHealth.interactable = PriceCheeck(int.Parse(_healthBoxPrice.text), _player.Details);
    }
    public void OnShowRangeItemInfo(int index)
    {
        chooseWeaponData = null;
        currentRangeWeapon = UpdateShopItems.WeaponDataList[index];
        _infoPanelRangeWeapon.SetActive(true);
        //if (_pseudoPlayer.AvailableWeapons.Contains(currentWeapon)) 
        if (_player._rangedWeaponsData.Contains(currentRangeWeapon))
        {
            _buyButtonRange.image.sprite = _alreadyExistsWeapon;
            _buyButtonRange.interactable = false;
        }
        else
        {
            _buyButtonRange.image.sprite = _notAlreadyExistsWeapon;
            _buyButtonRange.interactable = true;
            _buyButtonRange.interactable = PriceCheeck(UpdateShopItems.WeaponDataList[index].Price, _player.Details);
        }
        
        _titleInfoRange.sprite = UpdateShopItems.WeaponDataList[index].NameSprite;
        _abilityRangeIcon.sprite = UpdateShopItems.WeaponDataList[index].AbilitySprite;
        _abilityRangeName.text = UpdateShopItems.WeaponDataList[index].AbilityName;
        _abilityRangeDescription.text = UpdateShopItems.WeaponDataList[index].AbilityDescription;

        _damageCharacteristic.fillAmount = UpdateShopItems.WeaponDataList[index].Damage / 30.0f; //damage
        _reloadCharacteristic.fillAmount = 1.0f - (UpdateShopItems.WeaponDataList[index].ReloadTime * 0.1f); //reload
        _amunitionCountCharacteristic.fillAmount = UpdateShopItems.WeaponDataList[index].ClipCount / 30.0f;
        _shootPeroidCharacteristic.fillAmount = 1.0f - UpdateShopItems.WeaponDataList[index].ShootPeriod;
        chooseWeaponData = currentRangeWeapon;
    }

    
    public void OnShowMeleeItemInfo()
    {
        chooseWeaponData = null;
        currentMeleeWeapon = UpdateShopItems.MeleeWeapon;
        _infoPanelMeleeWeapon.SetActive(true);
       
        //if (_pseudoPlayer.AvailableWeapons.Contains(currentWeapon))
        if (_player._meleeWeaponsData.Contains(currentMeleeWeapon))
        {
            _buyButtonMelee.image.sprite = _alreadyExistsWeapon;
            _buyButtonMelee.interactable = false;
        }
        else
        {
            _buyButtonMelee.image.sprite = _notAlreadyExistsWeapon;
            _buyButtonMelee.interactable = true;
            _buyButtonMelee.interactable = PriceCheeck(UpdateShopItems.MeleeWeapon.Price, _player.Details);
        }
        
        _titleInfoMelee.sprite = UpdateShopItems.MeleeWeapon.NameSprite;
        _abilityMeleeIcon.sprite = UpdateShopItems.MeleeWeapon.AbilitySprite;
        _abilityMeleeName.text = UpdateShopItems.MeleeWeapon.AbilityName;
        _abilityMeleeDescription.text = UpdateShopItems.MeleeWeapon.AbilityDescription;

        _damageCharacteristicMelee.fillAmount = UpdateShopItems.MeleeWeapon.AttackDamage / 30.0f;
        _attackSpeedCharacteristicMelee.fillAmount = UpdateShopItems.MeleeWeapon.AttackSpeed / 20.0f;
        _attackRadiusCountCharacteristicMelee.fillAmount = UpdateShopItems.MeleeWeapon.AttackRadius / 12.0f;
        chooseWeaponData = currentMeleeWeapon;
    }
    public void OnClosedItemInfo(string WeaponType)
    {
        switch (WeaponType)
        {
            case "melee":
                _infoPanelMeleeWeapon.SetActive(false);
                break;
            case "range":
                _infoPanelRangeWeapon.SetActive(false);
                //currentWeapon = null;
                break;
            case "shop":
                _uiShop.SetActive(false);
                break;
            default:
                break;
        }
    }
    public void OnBuyButton()
    {
        Debug.Log($"Player bought {chooseWeaponData.Name}");
        //_pseudoPlayer.AvailableWeapons.Add(currentWeapon); //реализовать в скрипте игрока
        _player.Details -= chooseWeaponData.Price;
        

        ShowShopItem();
    }

    public void OnClickBuyOtherItem(int index)
    {
        switch (index)
        {
            case 0:
                //проверка
                _player.Details -= int.Parse(_flashDriverPrice.text);
                Debug.Log("Вы купили Флешку");
                break;
            case 1:
                //проверка
                _player.Details -= int.Parse(_healthBoxPrice.text);
                Debug.Log("Вы купили Аптечку");
                break;
            default:
                break;
        }
        ShowShopItem();
    }

    public void OnClickAlmanach()
    {
        _almanach.SetActive(true);
    }
}
