using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour 
{
    public GameObject dataInfoPanel;
    public InfoCanvas structureInfoPanel;
    public ProgressCanvas progressPanelCanvas;
    public Leaderboard leaderboardPanelCanvas;
    public GameObject playPanelCanvas;
    public Text nextStructureLabel;
    public CountdownCanvas countdownCanvas;
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
        progressPanelCanvas.time.text = FormatTime( Time.time - startTime );
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
        leaderboardPanelCanvas.transform.parent.gameObject.SetActive( true );

        leaderboardPanelCanvas.RecordNewScore( elapsedTime );
    }

    public void EnterPlayMode (StructureData structureData)
    {
        UIManager.Instance.Log( "UIManager: EnterPlayMode" );
        progressPanelCanvas.SetSelected( structureData.structureName, true );
        progressPanelCanvas.SetButtonLabel( false );
        progressPanelCanvas.transform.parent.gameObject.SetActive(false);
        structureInfoPanel.SetContent( structureData );
        dataInfoPanel.gameObject.SetActive( false );
        // playPanelCanvas.transform.parent.gameObject.SetActive( false );
        countdownCanvas.gameObject.SetActive( true );
        countdownCanvas.StartCountdown();
    }

    public void StartTimer ()
    {
        UIManager.Instance.Log( "UIManager: start timer" );
        VisualGuideManager.Instance.currentGameManager.StartTimer();
        progressPanelCanvas.transform.parent.gameObject.SetActive( true );
    }

    public void EnterSuccessMode (string structureName, float timeSeconds)
    {
        DisplayScore( timeSeconds );
        structureInfoPanel.gameObject.SetActive( true );
        progressPanelCanvas.SetButtonLabel( true );
    }

    public void EnterLobbyMode (string currentStructureName)
    {
        leaderboardPanelCanvas.Close();
        progressPanelCanvas.animator.SetTrigger( "Close" );
        dataInfoPanel.gameObject.SetActive( true );
        structureInfoPanel.gameObject.SetActive( false );
        playPanelCanvas.transform.parent.gameObject.SetActive( true );
        playPanelCanvas.GetComponent<Animator>().SetTrigger( "Open" );

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
