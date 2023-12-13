using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class gamePaused: MonoBehaviour
{
    private bool isPaused = false;
    public Canvas canvas;
    
    private void Start()
    {
        
    }
    void Update()
    {
        // Check for input to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // Pause the game
        canvas.enabled = true;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game
        canvas.enabled = false;
        isPaused = false;
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        // Get the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Reload the current scene
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void backToMainMenu(string sceneName)
    {
        Time.timeScale = 1f;

        SceneManager.LoadSceneAsync(sceneName);
    }
}
