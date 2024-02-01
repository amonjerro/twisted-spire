using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenBehavior : MonoBehaviour
{
    bool isPaused = false;
    public GameObject pauseScreen;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    //Toggles the game being paused
    public void PauseGame()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            pauseScreen.gameObject.SetActive(true);
            Time.timeScale = 0.0f;
        }

        else
        {
            pauseScreen.gameObject.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }

    //Goes back to Title Screen
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
