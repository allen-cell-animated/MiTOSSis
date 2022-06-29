using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour 
{
    string currentText;
    public GameObject clearButton;
    public GameObject confirmButton;
    private bool clearReady = false;
    private string cancelText = "cancel";
    private string clearText = "clear all";


    public void ClickKey (string character)
    {
        currentText += character;
        UIManager.Instance.leaderboard.UpdateCurrentPlayerName( currentText );
    }

    public void Backspace ()
    {
        if (currentText != null && currentText.Length > 0)
        {
            currentText = currentText.Substring(0, currentText.Length - 1);
            UIManager.Instance.leaderboard.UpdateCurrentPlayerName( currentText );
        }
    }

    public void Dismiss ()
    {
        gameObject.SetActive( false );
    }

    public void ClearText ()
    {
        currentText = "";
    }

    public void PromptClear()
    {
        clearButton.GetComponentInChildren<Text>().text = cancelText;
        if (!clearReady)
        {
            confirmButton.SetActive(true);
            clearReady = true;
            clearButton.GetComponentInChildren<Text>().text = cancelText;
        }
        else
        {
            confirmButton.SetActive(false);
            clearReady = false;
            clearButton.GetComponentInChildren<Text>().text = clearText;
        }
    }
}
