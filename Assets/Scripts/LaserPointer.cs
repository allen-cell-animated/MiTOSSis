using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class LaserPointer : MonoBehaviour
{
    public LayerMask collidableLayers;
    public float raycastDistance = 100f;

    RaycastHit hit;
    LaserPointerButton currentButton = null;

    bool clicking
    {
        get
        {
            return (
                ControllerInput.Instance.LeftTriggerDown() 
                || ControllerInput.Instance.RightTriggerDown()
            );
        }
    }

    void Update ()
    {
        Raycast();

        UpdateHover();

        if (currentButton && clicking)
        {
            currentButton.Click();
        }
    }

    void Raycast () 
    {
        Vector3 origin = ControllerInput.Instance.rayInteractor.Origin;
        Vector3 dir = ControllerInput.Instance.rayInteractor.End - origin;
        Physics.Raycast(origin, dir, out hit, raycastDistance, collidableLayers);
    }

    void UpdateHover ()
    {
        LaserPointerButton button = (
            hit.collider != null ? hit.collider.GetComponent<LaserPointerButton>() : null
        );
        if (button != currentButton)
        {
            if (currentButton)
            {
                currentButton.Hover( false );
                currentButton = null;
            }
            if (button)
            {
                button.Hover( true );
                currentButton = button;
            }
        }
    }
}
