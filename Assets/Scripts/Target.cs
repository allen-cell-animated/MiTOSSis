using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour 
{
    public string goalName;
    public SpriteRenderer visualization;
    public Text label;
    public RandomSoundPlayer successAudio;
    public AudioSource failAudio;

    Collider _collider;
    public Collider theCollider
    {
        get
        {
            if (_collider == null)
            {
                _collider = GetComponent<Collider>();
            }
            return _collider;
        }
    }

    Animator _animator;
    Animator animator
    {
        get
        {
            if (_animator == null)
            {
                _animator = GetComponentInChildren<Animator>();
            }
            return _animator;
        }
    }

    public void SetGoalName (string _goalName)
    {
        goalName = _goalName;
        label.text = _goalName;
    }

    public void Bind ()
    {
        visualization.gameObject.SetActive( false );
        successAudio.Play();
    }

    public void Release ()
    {
        visualization.gameObject.SetActive( true );
    }

    public void Bounce ()
    {
        animator.SetTrigger( "Fail" );
        failAudio.Play();
    }
}
