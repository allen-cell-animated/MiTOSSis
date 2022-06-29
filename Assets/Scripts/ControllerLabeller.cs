using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerLabeller : MonoBehaviour 
{
    public GameObject scaleButtonLabelRight;
    public GameObject gripRight;

    public GameObject scaleButtonLabelLeft;
    public GameObject gripLeft;

    public float gripScaleMultiplier;

    float gripStartScale;

    void Awake ()
    {
        gripStartScale = gripLeft.transform.localScale.x;
    }

    void Update ()
    {
        UpdateButtonLabels();
    }

    void UpdateButtonLabels ()
    {
        if (VisualGuideManager.Instance.currentMode == VisualGuideGameMode.Lobby)
        {
            bool rightGripDown = ControllerInput.Instance.IsRightGrip();
            bool leftGripDown = ControllerInput.Instance.IsLeftGrip();

            if (rightGripDown)
            {
                gripRight.transform.localScale = gripScaleMultiplier * gripStartScale * Vector3.one;
            }
            else
            {
                gripRight.transform.localScale = gripStartScale * Vector3.one;
            }

            if (leftGripDown)
            {
                gripLeft.transform.localScale = gripScaleMultiplier * gripStartScale * Vector3.one;
            }
            else
            {
                gripLeft.transform.localScale = gripStartScale * Vector3.one;
            }

            ShowObject( scaleButtonLabelRight, !rightGripDown);
            ShowObject( scaleButtonLabelLeft, !leftGripDown);

            ShowObject( gripRight, true );
            ShowObject( gripLeft, true );
        }
        else
        {
            ShowObject( gripRight, false );
            ShowObject( gripLeft, false );
            ShowObject( scaleButtonLabelRight, false );
            ShowObject( scaleButtonLabelLeft, false );
        }
    }

    void ShowObject (GameObject obj, bool show)
    {
        if (obj != null)
        {
            if (show && !obj.activeSelf)
            {
                obj.SetActive( true );
            }
            else if (!show && obj.activeSelf)
            {
                obj.SetActive( false );
            }
        }
    }
}
