using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverBehavior : MonoBehaviour
{
    public PlayerController player;
    public GameObject gameOverScreen;
    public GameObject gameUI;

    private void Update()
    {
        //Checks if player is dead and activates the Game Over Screen if they are dead
        if (player.isDead)
        {
            gameOverScreen.SetActive(true);
            gameUI.SetActive(false);
            Time.timeScale = 0.0f;
        }
    }

    //Restarts the gamy by loading the scene again
    public void Restart()
    {
        Time.timeScale = 1.0f;
        player.isDead = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
