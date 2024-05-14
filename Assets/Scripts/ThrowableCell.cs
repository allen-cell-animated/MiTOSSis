using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using cakeslice;
using Oculus.Interaction;

public class ThrowableCell : MonoBehaviour 
{
    public Vector3 rotationOffsetAtTarget;
    public Vector3 alignedMitosisPosition;
    public Vector3 alignedMitosisRotation;
    public float alignedMitosisScale;
    public RandomSoundPlayer cellAudio;

    [HideInInspector]
    public Target attachedTarget;

    [HideInInspector]
    public float lastSpawnTime;

    MitosisGameManager _gameManager;
    MitosisGameManager gameManager
    {
        get
        {
            if (_gameManager == null)
            {
                _gameManager = GetComponentInParent<MitosisGameManager>();
            }
            return _gameManager;
        }
    }

    Rigidbody _body;
    Rigidbody body
    {
        get
        {
            if (_body == null)
            {
                _body = GetComponent<Rigidbody>();
            }
            return _body;
        }
    }

    Mover _mover;
    Mover mover
    {
        get
        {
            if (_mover == null)
            {
                _mover = gameObject.AddComponent<Mover>();
            }
            return _mover;
        }
    }

    Rotator _rotator;
    Rotator rotator
    {
        get
        {
            if (_rotator == null)
            {
                _rotator = gameObject.AddComponent<Rotator>();
            }
            return _rotator;
        }
    }

    Scaler _scaler;
    Scaler scaler
    {
        get
        {
            if (_scaler == null)
            {
                _scaler = gameObject.AddComponent<Scaler>();
            }
            return _scaler;
        }
    }

    Outline _outline;
    public Outline outline
    {
        get
        {
            if (_outline == null)
            {
                _outline = GetComponentInChildren<Outline>();
            }
            return _outline;
        }
    }

    GameObject _grabInteraction;
    public GameObject grabInteraction
    {
        get
        {
            if (_grabInteraction == null)
            {
                _grabInteraction = GetComponentInChildren<Grabbable>().gameObject;
            }
            return _grabInteraction;
        }
    }

    public bool isMoving
    {
        get
        {
            return !(body.isKinematic || body.velocity.magnitude < 0.1f);
        }
    }

    void OnCollisionEnter (Collision collision)
    {
        Target target = collision.gameObject.GetComponent<Target>();
        if (target != null && attachedTarget == null)
        {
            if (target.goalName.Contains( name.Substring( 0, name.Length - 11 ) ))
            {
                BindToTarget( target );
            }
            else
            {
                target.Bounce();
            }
        }
        else 
        {
            cellAudio.Play();
        }
    }

    void BindToTarget (Target target)
    {
        if (gameManager != null)
        {
            grabInteraction.SetActive( false );
            attachedTarget = target;
            attachedTarget.Bind();

            body.isKinematic = true;
            transform.position = target.transform.position;
            transform.rotation = target.transform.rotation * Quaternion.Euler(rotationOffsetAtTarget);

            gameManager.RecordCorrectHit();
            CreateConfetti();
        }
    }

    void CreateConfetti ()
    {
        GameObject prefab = Resources.Load( "Confetti1" ) as GameObject;
        if (prefab == null)
        {
            Debug.LogWarning( "Couldn't load prefab for Confetti1" );
            return;
        }
        Instantiate( prefab, transform.position, Quaternion.identity );
    }

    public void ReleaseFromTarget (bool resetVelocity = false)
    {
        if (attachedTarget != null)
        {
            gameManager.RemoveCorrectHit();
            attachedTarget.Release();
            attachedTarget = null;
            grabInteraction.SetActive( true );
        }

        body.isKinematic = false;
        if (resetVelocity)
        {
            body.velocity = Vector3.zero;
        }
    }
}
