using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Almanach : MonoBehaviour
{
    [Header("Almanach Left Page")]
    [SerializeField] Image almanachLeftPageImage;
    [SerializeField] Text almanachLeftPageDescription;
    [Space(10)]
    [Header("Almanach Right Page")]
    [SerializeField] Image almanachRightPageImage;
    [SerializeField] Text almanachRightPageDescription;
    [Space(10)]
    [Header("Almanach Data")]
    [SerializeField] private List<Sprite> _almanachIcon;
    [SerializeField] private List<string> _almanachDecription;
    [Space(10)]
    [Header("Buttons")]
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _backButton;
    
    private int currentPage;

    private void Start()
    {
        currentPage = 0;
        ShowAlmanachMainPage();
    }

    

    private void ShowAlmanachMainPage()
    {
        almanachLeftPageImage.sprite = _almanachIcon[currentPage];
        almanachLeftPageDescription.text = _almanachDecription[currentPage];
        currentPage++;
        almanachRightPageImage.sprite = _almanachIcon[currentPage];
        almanachRightPageDescription.text = _almanachDecription[currentPage];
        _backButton.interactable = false;
    }

    public void NextPage()
    {
        currentPage++;
        almanachLeftPageImage.sprite = _almanachIcon[currentPage];
        almanachLeftPageDescription.text = _almanachDecription[currentPage];
        currentPage++;
        almanachRightPageImage.sprite = _almanachIcon[currentPage];
        almanachRightPageDescription.text = _almanachDecription[currentPage];
        _backButton.interactable = true;

        if (currentPage + 2 >= _almanachIcon.Count)
        {
            _nextButton.interactable = false;
        }
    }
    public void BackPage()
    {
        currentPage = currentPage - 3;
        almanachLeftPageImage.sprite = _almanachIcon[currentPage];
        almanachLeftPageDescription.text = _almanachDecription[currentPage];
        currentPage++;
        almanachRightPageImage.sprite = _almanachIcon[currentPage];
        almanachRightPageDescription.text = _almanachDecription[currentPage];
        _nextButton.interactable = true;

        if (currentPage - 1 == 0)
        {
            _backButton.interactable = false;
        }
    }
   
}
