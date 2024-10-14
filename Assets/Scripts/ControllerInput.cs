using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour 
{
    public Transform leftController;
    public Transform rightController;

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
        return Input.GetButton( "XRI_Left_Grip" ) && Input.GetButton( "XRI_Right_Grip" );
    }

    public bool LeftTriggerDown ()
    {
        return Input.GetButton( "XRI_Left_Trigger" );
    }

    public bool RightTriggerDown ()
    {
        return Input.GetButton( "XRI_Right_Trigger" );
    }

    public Vector3 LeftControllerPosition()
    {
        return leftController.position;
    }
    
    public Vector3 RightControllerPosition()
    {
        return rightController.position;
    }

    public void ToggleRayInteractors(bool _active)
    {
        // TODO toggle ray visualization
    }
}
