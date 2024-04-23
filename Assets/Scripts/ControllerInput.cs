using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllerInput : MonoBehaviour 
{
    public InputDevice leftController;
    public InputDevice rightController;
    public GameObject rightUIController;
    public XRDirectInteractor leftInteractor;
    public XRDirectInteractor rightInteractor;

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

    void Start()
    {
        // var leftHandedControllers = new List<UnityEngine.XR.InputDevice>();
        // var desiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Left | UnityEngine.XR.InputDeviceCharacteristics.Controller;
        // UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, leftHandedControllers);

        // foreach (var device in leftHandedControllers)
        // {
        //     Debug.Log(string.Format("Device name '{0}' has characteristics '{1}'", device.name, device.characteristics.ToString()));
        // }
        // leftController = leftHandedControllers[0];

        // var rightHandedControllers = new List<UnityEngine.XR.InputDevice>();
        // desiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Right | UnityEngine.XR.InputDeviceCharacteristics.Controller;
        // UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, rightHandedControllers);

        // foreach (var device in rightHandedControllers)
        // {
        //     Debug.Log(string.Format("Device name '{0}' has characteristics '{1}'", device.name, device.characteristics.ToString()));
        // }
        // rightController = rightHandedControllers[0];

        // rightUIController = GameObject.Find("RightUIController");

        // leftInteractor = GameObject.Find("LeftGrabController").GetComponent<XRDirectInteractor>();
        // rightInteractor = GameObject.Find("RightGrabController").GetComponent<XRDirectInteractor>();
        // Debug.Log(string.Format("Interactor name '{0}'", leftInteractor.name));
        // Debug.Log(string.Format("Interactor name '{0}'", rightInteractor.name));
    }

    public bool IsLeftTrigger()
    {
        // leftController.TryGetFeatureValue(CommonUsages.triggerButton, out bool leftDown);
        // return leftDown;
        return false;
    }
    public bool IsRightTrigger()
    {
        // rightController.TryGetFeatureValue(CommonUsages.triggerButton, out bool rightDown);
        // return rightDown;
        return false;
    }
    public bool IsLeftGrip()
    {
        // leftController.TryGetFeatureValue(CommonUsages.gripButton, out bool leftDown);
        // return leftDown;
        return false;
    }
    public bool IsRightGrip()
    {
        // rightController.TryGetFeatureValue(CommonUsages.gripButton, out bool rightDown);
        // return rightDown;
        return false;
    }
    public Vector3 LeftControllerPosition()
    {
        // leftController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 leftPos);
        // return leftPos;
        return Vector3.zero;
    }
    public Vector3 RightControllerPosition()
    {
        // rightController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 rightPos);
        // return rightPos;
        return Vector3.zero;
    }

    public void ToggleRayInteractor(bool _active)
    {
        if(rightUIController != null)
        {
            rightUIController.SetActive(_active);
        }
    }
}
