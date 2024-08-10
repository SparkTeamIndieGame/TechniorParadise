using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroView : MonoBehaviour
{
    public int WaitTime;
    private void Start()
    {
        StartCoroutine(WaitTimePerLvl());
    }
    private IEnumerator WaitTimePerLvl()
    {
        yield return new WaitForSeconds(WaitTime);
        SceneManager.LoadScene(1);
    }
}
