using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void PlayGame(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
    public void Credit(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void BackMainMenu(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
        Debug.Log("succes");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("QUIT!!!");
    }

    
}
