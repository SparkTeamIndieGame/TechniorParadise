using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ComixShow : MonoBehaviour
{
    [SerializeField] private LoaderScens _loaderScens;
    [SerializeField] private List<GameObject> _pages;
    [SerializeField] private float _timeBetweenChar = 0.07f;
    
    private List<Image> backGroundForText = new List<Image>(); //фон текста (отцовский объект)
    private List<Text> titleText = new List<Text>(); // текст 
    private List<String> originalText = new List<String>(); // копируем в одельный лист содержание текста(из инспектора), т.к мы заменяем текст на " "
    private List<Button> skipTextBt = new List<Button>(); //кнопки пропуска текста
    private List<Button> nextSlideBt = new List<Button>(); //кнопки перехода к следующему слайду
    
    private const string DEFAULT_TITLE = " ";
    
    private int currentPage = 0;
    private int currentSceneNumber;
    private int allSceneNumber;
    
    private void Start()
    {
        currentSceneNumber = SceneManager.GetActiveScene().buildIndex;
        allSceneNumber = SceneManager.sceneCountInBuildSettings - 1;
        GetAllComponents();
    }
    
    private void GetAllComponents() // получаем все ссылки
    {
        for (int i = 0; i < _pages.Count; i++)
        {
            backGroundForText.Add(_pages[i].transform.GetChild(0).GetComponent<Image>());
            titleText.Add(backGroundForText[i].transform.GetChild(0).GetComponent<Text>());
            skipTextBt.Add(backGroundForText[i].transform.GetChild(1).GetComponent<Button>());
            nextSlideBt.Add(backGroundForText[i].transform.GetChild(2).GetComponent<Button>());
            originalText.Add(titleText[i].text);
            titleText[i].text = DEFAULT_TITLE;
        }
    }

    private IEnumerator ShowImage()
    {
        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime * 1.5f)
        {
            _pages[currentPage].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, i);
            yield return null;
        }
        backGroundForText[currentPage].gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(ShowText());
    }
    private IEnumerator ShowText()
    {
        for (int i = 0; i < originalText[currentPage].Length; i++)
        {
            titleText[currentPage].text += originalText[currentPage][i].ToString();
            if (i == 10) // можем пропустить после отрисовки 10ого символа
            {
                skipTextBt[currentPage].interactable = true;
            }
            yield return new WaitForSeconds(_timeBetweenChar);
        }

        if (currentPage +1 >= _pages.Count)
        {
            yield return new WaitForSeconds(3.0f);
            if (currentSceneNumber == allSceneNumber)
            {
                _loaderScens.LoadScene(0); //номер сцены
            }
            else
            {
                _loaderScens.NextScene(); //номер сцены
            }
        }
        else
        {
            currentPage++;
            yield return new WaitForSeconds(1.0f);
            StartCoroutine(ShowImage());
        }
    }
    private IEnumerator WaitForNextSlide()
    {
        yield return new WaitForSeconds(1.5f);
        nextSlideBt[currentPage - 1].interactable = true;
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(ShowImage());
    }
    private IEnumerator EndComix()
    {
        yield return new WaitForSeconds(3.0f);
        if (currentSceneNumber == allSceneNumber)
        {
            _loaderScens.LoadScene(0); //номер сцены
        }
        else
        {
            _loaderScens.NextScene(); //номер сцены
        }
        
        
    }
    public void OnSkipTextClicked()
    {
        skipTextBt[currentPage].gameObject.SetActive(false);
        StopAllCoroutines();
        titleText[currentPage].text = originalText[currentPage];
        if (currentPage + 1 != _pages.Count)
        {
            nextSlideBt[currentPage].gameObject.SetActive(true);
            currentPage++;
            StartCoroutine(WaitForNextSlide());
        }
        else
        {
            StartCoroutine(EndComix());
        }
    }
    public void OnNextSlideClicked()
    {
        StopAllCoroutines();
        StartCoroutine(ShowImage());
    }
    public void RunComix()
    {
        StartCoroutine(ShowImage());
    }
    public void OnFullSkipClicked()
    {
        StopAllCoroutines();
        if (currentSceneNumber == allSceneNumber)
        {
            _loaderScens.LoadScene(0); //номер сцены
        }
        else
        {
            _loaderScens.NextScene(); //номер сцены
        }
    }
}
