using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class ControllerInput : MonoBehaviour 
{
    public RayInteractor rayInteractor;

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

    public bool LeftTriggerDown ()
    {
        return OVRInput.Get( OVRInput.RawButton.LIndexTrigger );
    }

    public bool RightTriggerDown ()
    {
        return OVRInput.Get( OVRInput.RawButton.RIndexTrigger );
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
        rayInteractor.gameObject.SetActive( _active );
    }
}
