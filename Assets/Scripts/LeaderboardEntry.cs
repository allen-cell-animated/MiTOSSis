using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardEntry : MonoBehaviour 
{
    public TMP_Text rank;
    public TMP_Text playerName;
    public TMP_Text time;

    public void Populate (int _rank, string _playerName, float _timeSeconds)
    {
        rank.text = "#" + _rank.ToString();
        playerName.text = _playerName;
        time.text = UIManager.Instance.FormatTime( _timeSeconds );
    }

    public void UpdatePlayerName (string _newPlayerName)
    {
        playerName.text = _newPlayerName;
    }
}
