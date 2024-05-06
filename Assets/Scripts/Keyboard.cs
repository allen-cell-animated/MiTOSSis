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


    public void Enable ()
    {
        gameObject.SetActive( true );
        currentText = "";
    }

    void UpdateCurrentText (string newText)
    {
        currentText = newText;
        UIManager.Instance.leaderboardUI.UpdateCurrentPlayerName( currentText );

    }

    public void ClickKey (string character)
    {
        UpdateCurrentText( currentText + character );
    }

    public void Backspace ()
    {
        if (currentText != null && currentText.Length > 0)
        {
            UpdateCurrentText( currentText.Substring( 0, currentText.Length - 1 ) );
        }
    }

    public void Dismiss ()
    {
        gameObject.SetActive( false );
    }

    public void ClearText ()
    {
        UpdateCurrentText( "" );
    }

    public void ToggleClear ()
    {
        clearReady = !clearReady;
        confirmButton.SetActive(clearReady);
        clearButton.GetComponentInChildren<Text>().text = clearReady ? cancelText : clearText;
    }
}
