using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownCanvas : MonoBehaviour 
{
    public Animator numbers;

    public void StartCountdown ()
    {
        UIManager.Instance.Log( "CountdownCanvas: START countdown" );
        numbers.SetTrigger( "play" );
    }

    public void FinishCountdown ()
    {
        UIManager.Instance.Log( "CountdownCanvas: finish countdown" );
        UIManager.Instance.StartTimer();
        gameObject.SetActive( false );
    }
}
