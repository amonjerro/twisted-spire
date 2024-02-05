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

    //bool for whether the section of dialog is the last one
    bool lastDialogSection;

    void Start()
    {
        Time.timeScale = 0.0f;
    }

    public void NextSection() {
        if (lastDialogSection)
        {
            Time.timeScale = 1.0f;
            dialogBox.SetActive(false);
            gameUI.SetActive(true);
        }
        
        else
        {
            //Change text for dialog and its shadow
            dialogTextObject.GetComponent<TextMeshProUGUI>().text = "Use the 'A' and 'D' keys to move left and right. Use the Spacebar to jump. Watch out for any enemies or hazards in the tower.";
            dialogTextObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Use the 'A' and 'D' keys to move left and right. Use the Spacebar to jump. Watch out for any enemies or hazards in the tower.";

            //Change text for button and its shadow
            nextButton.GetComponent<TextMeshProUGUI>().text = "CLOSE >";
            nextButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "CLOSE >";

            //Set last section to true;
            lastDialogSection = true;
        }
    }
}
