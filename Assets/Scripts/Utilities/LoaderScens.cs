using UnityEngine;
using UnityEngine.SceneManagement;


public class LoaderScens : MonoBehaviour
{
    

    public void LoadScene(int countScene)
    {
        SceneManager.LoadScene(countScene);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        var NumberScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(NumberScene);
    }

    public void NextScene()
    {
        var NumberScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(NumberScene + 1);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
