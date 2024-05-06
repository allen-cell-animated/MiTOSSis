using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using cakeslice;

public class ThrowableCell : MonoBehaviour 
{
    [Header("Throwable Cell Settings")]

    public float boundsRadius;
    public Vector3 rotationOffsetAtTarget;
    public Vector3 alignedMitosisPosition;
    public Vector3 alignedMitosisRotation;
    public float alignedMitosisScale;
    public Target attachedTarget;
    public float lastSpawnTime;
    public bool isGrabbable = true;
    public bool isGrabbed; // TODO

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

    public bool isMoving
    {
        get
        {
            return !(body.isKinematic || body.velocity.magnitude < 0.1f);
        }
    }

    public void AttachToController (object args)
    {
        UIManager.Instance.Log(args.ToString());
        // transform.SetParent(args.interactor.transform.GetChild(0));
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
    }

    void BindToTarget (Target target)
    {
        if (gameManager != null)
        {
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
        }

        body.isKinematic = false;
        if (resetVelocity)
        {
            body.velocity = Vector3.zero;
        }
    }
    void Awake()
    {
        CreateAttach();
    }

    private void CreateAttach()
    {
        if (TryGetComponent(out XRGrabInteractable interactable))
        {
            GameObject attachObject = new GameObject("Attach");

            attachObject.transform.SetParent(transform);
            attachObject.transform.localPosition = Vector3.zero;
            attachObject.transform.localRotation = Quaternion.identity;

            interactable.attachTransform = attachObject.transform;
        }
    }
}
