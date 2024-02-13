using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogBoxBehavior : MonoBehaviour
{
    //GameObject references
    public GameObject dialogBox;
    public GameObject gameUI;
    public GameObject nextButton;
    public GameObject dialogTextObject;

    void Start()
    {
        Time.timeScale = 0.0f;
    }

    public void NextSection() {
        //Start the game when the button is pressed on the last dialog section

        Time.timeScale = 1.0f;
        dialogBox.SetActive(false);
        gameUI.SetActive(true);
    }
}
