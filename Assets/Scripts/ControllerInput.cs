using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class ControllerInput : MonoBehaviour 
{
    public RayInteractor leftRay;
    public RayInteractor rightRay;

    static ControllerInput _Instance;
    public static ControllerInput Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = GameObject.FindObjectOfType<ControllerInput>();
            }
            return _Instance;
        }
    }

    public bool GripsDown ()
    {
        return (
            OVRInput.Get( OVRInput.Button.PrimaryHandTrigger )
            && OVRInput.Get( OVRInput.Button.SecondaryHandTrigger )
        );
    }

    public Vector3 LeftControllerPosition()
    {
        if (!OVRInput.GetControllerPositionValid( OVRInput.Controller.LTouch ))
        {
            return Vector3.zero;
        }
        return OVRInput.GetLocalControllerPosition( OVRInput.Controller.LTouch );
    }
    
    public Vector3 RightControllerPosition()
    {
        if (!OVRInput.GetControllerPositionValid( OVRInput.Controller.RTouch ))
        {
            return Vector3.zero;
        }
        return OVRInput.GetLocalControllerPosition( OVRInput.Controller.RTouch );
    }

    public void ToggleRayInteractors(bool _active)
    {
        leftRay.gameObject.SetActive( _active );
        rightRay.gameObject.SetActive( _active );
    }
}
