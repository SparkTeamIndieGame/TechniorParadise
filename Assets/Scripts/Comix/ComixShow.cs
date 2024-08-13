using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ComixShow : MonoBehaviour
{
    [SerializeField] private List<GameObject> _pages;
    [SerializeField] private float _timeBetweenChar = 0.1f;
    
    private List<GameObject> labelForText = new List<GameObject>(); // оболочка, через которую ищем все остальное
    private List<Image> backGroundForText = new List<Image>(); //фон текста (отцовский объект)
    private List<Text> titleText = new List<Text>(); // текст 
    private List<String> originalText = new List<String>(); // копируем в одельный лист содержание текста(из инспектора), т.к мы заменяем текст на " "
    private List<Button> buttons = new List<Button>(); //кнопки пропуска
    
    private const string DEFAULT_TITLE = " ";
    
    private float timeAlphaChanged = 10.0f;
    private int currentPage = 0;

    private bool isActiveImage = false;
    private bool isShowFullText = false;
    
    private Coroutine SlideShowCoroutine;
  
    void Start()
    {
        GetAllComponents();
        StartCoroutine(ShowText());
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(backGroundForText.Count);
            Debug.Log(titleText.Count);
            Debug.Log(buttons.Count);
            for (int i = 0; i < 2; i++)
            {
                Debug.Log(titleText[i].text);
                Debug.Log(buttons[i].name);
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            for (int i = 0; i < titleText.Count; i++)
            {
                Debug.Log(originalText[i]);
            }
        }
    }
    private void GetAllComponents() // получаем все ссылки
    {
        for (int i = 0; i < _pages.Count; i++)
        {
            backGroundForText.Add(_pages[i].transform.GetChild(0).GetComponent<Image>());
            titleText.Add(backGroundForText[i].transform.GetChild(0).GetComponent<Text>());
            buttons.Add(backGroundForText[i].transform.GetChild(1).GetComponent<Button>());
            originalText.Add(titleText[i].text);
            titleText[i].text = DEFAULT_TITLE;
        }
    }

    private IEnumerator ShowImage()
    {
        for (float i = 0.0f; i < timeAlphaChanged; i += Time.deltaTime * 2.0f)
        {
            _pages[currentPage].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, i);
            yield return null;
        }
        labelForText[currentPage].gameObject.SetActive(true);

        
    }
    private IEnumerator ShowText()
    {
        for (int i = 0; i < originalText[currentPage].Length; i++)
        {
            titleText[currentPage].text += originalText[currentPage][i].ToString();
            if (i == 10) // можем пропустить после отрисовки 10ого символа
            {
                buttons[currentPage].interactable = true;
            }
            if (i > 300) // в рамку вмещается 50 символов(удобное чтение). Иначе мы начинаем удалять по одному символу с начала строки
            {
                titleText[currentPage].text = titleText[currentPage].text.Remove(0,1);
            }
            yield return new WaitForSeconds(_timeBetweenChar);
        }
    }

    // private IEnumerator ShowImage() // появление изоображения. I
    // {
    //     while (currentPage < _pages.Count)
    //     {
    //         for (float i = 0.0f; i < timeAlphaChanged; i += Time.deltaTime)
    //         {
    //             _pages[currentPage].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, i);
    //         }
    //         labelForText[currentPage].SetActive(true); // активация панели текста
    //     
    //         yield return new WaitForSeconds(1.0f);
    //     
    //         //анимируем текст (выводился по одному символу). II
    //         for (int i = 0; i < originalText[currentPage].Length; i++)
    //         {
    //             titleText[currentPage].text += originalText[currentPage][i].ToString();
    //             if (i == 10) // можем пропустить после отрисовки 10ого символа
    //             {
    //                 buttons[currentPage].interactable = true;
    //             }
    //             if (i > 50) // в рамку вмещается 50 символов(удобное чтение). Иначе мы начинаем удалять по одному символу с начала строки
    //             {
    //                 titleText[currentPage].text = titleText[currentPage].text.Remove(0,1);
    //             }
    //             yield return new WaitForSeconds(_timeBetweenChar);
    //         }
    //         currentPage++;
    //     }
    //     
    //     
    // }
    // private IEnumerator TextAnim() //анимируем текст (выводился по одному символу). II
    // {
    //     for (int i = 0; i < originalText[currentPage].Length; i++)
    //     {
    //         titleText[currentPage].text += originalText[currentPage][i].ToString();
    //         if (i == 10) // можем пропустить после отрисовки 10ого символа
    //         {
    //             buttons[currentPage].interactable = true;
    //         }
    //         if (i > 50) // в рамку вмещается 50 символов(удобное чтение). Иначе мы начинаем удалять по одному символу с начала строки
    //         {
    //             titleText[currentPage].text = titleText[currentPage].text.Remove(0,1);
    //         }
    //         yield return new WaitForSeconds(_timeBetweenChar);
    //     }
    // }

    
}
