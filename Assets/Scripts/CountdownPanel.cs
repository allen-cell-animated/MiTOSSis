using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownPanel : MonoBehaviour 
{
    public Animator numbers;

    public void StartCountdown ()
    {
        UIManager.Instance.Log( "CountdownPanel: START countdown" );
        numbers.SetTrigger( "play" );
    }

    public void FinishCountdown ()
    {
        UIManager.Instance.Log( "CountdownPanel: finish countdown" );
        UIManager.Instance.StartTimer();
        gameObject.SetActive( false );
    }
}
