using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameStatus : MonoBehaviour
{
    private bool isPaused = false;
    public Canvas canvas;    
    private GameObject allStatues;
    private GameObject player;
    public bool clearPanel = true;

    private void Start()
    {
        allStatues = GameObject.FindGameObjectWithTag("allStatues");
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        if (allStatues != null && player != null)
        {
            // Assuming allStatues has a script with totalStatues and sealedStatues variables
            allStatues statueManager = allStatues.GetComponent<allStatues>();
            Damageable playerDamageable = player.GetComponent<Damageable>();

            if (statueManager != null && clearPanel)
            {
                // Access the totalStatues and sealedStatues variables
                int totalStatues = statueManager.totalNumber;
                int sealedStatues = statueManager.sealedNumber;

                if(sealedStatues == totalStatues)
                {
                    clearGame();
                }
            }
            else if(playerDamageable != null && !clearPanel)
            {
                int healh = playerDamageable.Health;
                if(healh <= 0)
                {
                    loseGame();
                }
            }
            else
            {
                Debug.LogError("StatueManager script not found on the allStatues GameObject.");
            }
        }

    }

    void clearGame()
    {
        canvas.enabled = true;        
    }
    void loseGame()
    {
        canvas.enabled = true;        
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
        SceneManager.LoadSceneAsync(sceneName);
    }
}
