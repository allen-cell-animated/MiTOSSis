using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerLabeller : MonoBehaviour 
{
    public GameObject scaleButtonLabelRight;
    public GameObject scaleButtonLabelLeft;

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
            ShowObject( scaleButtonLabelRight, !rightGripDown);
            ShowObject( scaleButtonLabelLeft, !leftGripDown);
        }
        else
        {
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
