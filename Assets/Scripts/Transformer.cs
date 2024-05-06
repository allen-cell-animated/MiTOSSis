using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformer : MonoBehaviour 
{
    public bool transforming;
    public bool canScale = true;
    public bool canRotate = true;

    Vector3 startScale;
    float startControllerDistance;
    Quaternion startRotation;
    Vector3 startControllerVector;
    Vector3 startPositiveVector;
    Vector3 minScale = new Vector3( 0.2f, 0.2f, 0.2f );
    Vector3 maxScale = new Vector3( 3f, 3f, 3f );
    Vector3[] linePoints = new Vector3[2];

    LineRenderer _scaleLine;
    LineRenderer scaleLine
    {
        get
        {
            if (_scaleLine == null)
            {
                GameObject prefab = Resources.Load( "ScaleLine" ) as GameObject;
                if (prefab == null)
                {
                    UIManager.Instance.Log( "WARNING: Couldn't load prefab for ScaleLine" );
                    return null;
                }
                _scaleLine = Instantiate( prefab ).GetComponent<LineRenderer>();
            }
            return _scaleLine;
        }
    }

    void Update ()
    {
        UpdateTransforming();
    }

    // TRANSLATING --------------------------------------------------------------------------------------------------

    void UpdateTransforming ()
    {
        if (ControllerInput.Instance.GripsDown())
        {
            if (!transforming)
            {
                transforming = true;
                ControllerInput.Instance.ToggleRayInteractors( false );
                StartScaling();
                StartRotating();
            }
            else
            {
                UpdateScale();
                UpdateRotation();
            }
            ToggleLine( true );
        }
        else if (transforming)
        {
            ToggleLine( false );
            ControllerInput.Instance.ToggleRayInteractors( true );
            transforming = false;
        }
    }

    void StartScaling ()
    {
        if (canScale)
        {
            startScale = transform.localScale;
            Vector3 left = ControllerInput.Instance.LeftControllerPosition();
            Vector3 right = ControllerInput.Instance.RightControllerPosition();
            if (left != null && right != null)
            {
                startControllerDistance = Vector3.Distance(right, left);
            }
        }
    }

    void UpdateScale ()
    {
        if (canScale)
        {
            Vector3 left = ControllerInput.Instance.LeftControllerPosition();
            Vector3 right = ControllerInput.Instance.RightControllerPosition();
            if (left != null && right != null)
            {
                float scale = Vector3.Distance(right, left) / startControllerDistance;
                transform.localScale = ClampScale(scale * startScale);
            }
        }
    }

    Vector3 ClampScale (Vector3 _scale)
    {
        if (_scale.magnitude > maxScale.magnitude)
        {
            return maxScale;
        }
        else if (_scale.magnitude < minScale.magnitude)
        {
            return minScale;
        }
        else
        {
            return _scale;
        }
    }

    void StartRotating ()
    {
        if (canRotate)
        {
            startRotation = transform.localRotation;
            Vector3 left = ControllerInput.Instance.LeftControllerPosition();
            Vector3 right = ControllerInput.Instance.RightControllerPosition();
            if (left != null && right != null)
            {
                startControllerVector = right - left;
                startControllerVector.y = 0;
                startPositiveVector = Vector3.Cross(startControllerVector, Vector3.up);
            }
        }
    }

    void UpdateRotation ()
    {
        if (canRotate)
        {
            Vector3 left = ControllerInput.Instance.LeftControllerPosition();
            Vector3 right = ControllerInput.Instance.RightControllerPosition();
            if (left != null && right != null)
            {
                Vector3 controllerVector = right - left;
                controllerVector.y = 0;
                float direction = GetArcCosineDegrees(Vector3.Dot(startPositiveVector.normalized, controllerVector.normalized)) >= 90f ? 1f : -1f;
                float dAngle = direction * GetArcCosineDegrees(Vector3.Dot(startControllerVector.normalized, controllerVector.normalized));

                transform.localRotation = startRotation * Quaternion.AngleAxis(dAngle, Vector3.up);
            }
        }
    }

    float GetArcCosineDegrees (float cosine)
    {
        if (cosine > 1f - float.Epsilon)
        {
            return 0;
        }
        if (cosine < -1f + float.Epsilon)
        {
            return 180f;
        }
        return Mathf.Acos( cosine ) * Mathf.Rad2Deg;
    }

    void ToggleLine (bool _active)
    {
        if (_active)
        {
            Vector3 left = ControllerInput.Instance.LeftControllerPosition();
            Vector3 right = ControllerInput.Instance.RightControllerPosition();
            if (!scaleLine.gameObject.activeSelf)
            {
                scaleLine.gameObject.SetActive( true );
            }
            linePoints[0] = right;
            linePoints[1] = left;
            scaleLine.SetPositions( linePoints );
        }
        else
        {
            scaleLine.gameObject.SetActive( false );
        }
    }
}
