using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour 
{
    public CollapsiblePanel collapsiblePanel;
    public ProgressPanel progressPanel;
    public Leaderboard leaderboardUI;
    public GameObject lobbyPanel;
    public Text nextStructureLabel;
    public CountdownPanel countdownPanel;
    public Text debugText;

    static UIManager _Instance;
    public static UIManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = GameObject.FindObjectOfType<UIManager>();
            }
            return _Instance;
        }
    }

    public void UpdateTime (float startTime)
    {
        progressPanel.time.text = FormatTime( Time.time - startTime );
    }

    public string FormatTime (float timeSeconds)
    {
        float timeMinutes = Mathf.Floor( timeSeconds / 60f );
        timeSeconds = Mathf.Round( timeSeconds - 60f * timeMinutes);

        string timeSecondsStr = timeSeconds.ToString();
        while (timeSecondsStr.Length < 2)
        {
            timeSecondsStr = "0" + timeSecondsStr;
        }

        string timeMinutesStr = timeMinutes.ToString();
        while (timeMinutesStr.Length < 2)
        {
            timeMinutesStr = "0" + timeMinutesStr;
        }

        return timeMinutesStr + ":" + timeSecondsStr;
    }

    public void DisplayScore (float elapsedTime)
    {
        leaderboardUI.RecordNewScore( elapsedTime );
    }

    public void EnterPlayMode (StructureData structureData)
    {
        UIManager.Instance.Log( "UIManager: EnterPlayMode" );
        progressPanel.SetSelected( structureData.structureName, true );
        progressPanel.SetButtonLabel( false );
        progressPanel.gameObject.SetActive(false);
        collapsiblePanel.SetStructureContent( structureData );
        lobbyPanel.gameObject.SetActive( false );
        countdownPanel.gameObject.SetActive( true );
        countdownPanel.StartCountdown();
    }

    public void StartTimer ()
    {
        UIManager.Instance.Log( "UIManager: start timer" );
        VisualGuideManager.Instance.currentGameManager.StartTimer();
        progressPanel.gameObject.SetActive( true );
    }

    public void EnterSuccessMode (string structureName, float timeSeconds)
    {
        DisplayScore( timeSeconds );
        collapsiblePanel.Enable();
        progressPanel.SetButtonLabel( true );
    }

    public void EnterLobbyMode (string currentStructureName)
    {
        leaderboardUI.Close();
        progressPanel.animator.SetTrigger( "Close" );
        collapsiblePanel.SetDataContent();
        lobbyPanel.gameObject.SetActive( true );
        lobbyPanel.GetComponent<Animator>().SetTrigger( "Open" );

        nextStructureLabel.text = (currentStructureName == "Endoplasmic Reticulum (ER)" ? "Endoplasmic\u2008Reticulum\u2008(ER)" :
                                   currentStructureName == "Golgi Apparatus" ? "Golgi\u2008Apparatus" : currentStructureName);
    }

    public void Play ()
    {
        VisualGuideManager.Instance.SelectNextStructureAndPlay();
    }

    public void Log (string message)
    {
        if (debugText.gameObject.activeInHierarchy)
        {
            debugText.text += message + "\n";
        }
    }
}
