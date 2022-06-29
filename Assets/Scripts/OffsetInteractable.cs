using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class OffsetInteractable : XRGrabInteractable
{
    private enum SelectedHand { None, Left, Right };
    private SelectedHand lastHand = SelectedHand.None;

    private void Update()
    {

        XRDirectInteractor leftInteractor = ControllerInput.Instance.leftInteractor;
        XRDirectInteractor rightInteractor = ControllerInput.Instance.rightInteractor;
        bool leftGrip = ControllerInput.Instance.IsLeftGrip();
        bool rightGrip = ControllerInput.Instance.IsRightGrip();

        if (leftInteractor != null && !leftGrip && leftInteractor.allowSelect == false)
        {
            leftInteractor.allowSelect = true;
        }

        if (rightInteractor != null && !rightGrip && rightInteractor.allowSelect == false)
        {
            rightInteractor.allowSelect = true;
        }

        if (rightInteractor != null && rightGrip && rightInteractor.selectTarget != null)
        {
            ControllerInput.Instance.ToggleRayInteractor(false);
        }
        else
        {
            ControllerInput.Instance.ToggleRayInteractor(true);
        }
    }
    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        MatchAttachPoint(interactor);
        SwapHands();
        base.OnSelectEntering(interactor);
    }
    protected override void OnSelectExiting(XRBaseInteractor interactor)
    {
        base.OnSelectExiting(interactor);
    }

    private void MatchAttachPoint(XRBaseInteractor interactor)
    {
        bool isDirect = interactor is XRDirectInteractor;
        attachTransform.position = isDirect ? interactor.attachTransform.position : transform.position;
        attachTransform.rotation = isDirect ? interactor.attachTransform.rotation : transform.rotation;
    }

    private void SwapHands()
    {
        InputDevice right = ControllerInput.Instance.rightController;
        InputDevice left = ControllerInput.Instance.leftController;
        XRDirectInteractor leftInteractor = ControllerInput.Instance.leftInteractor;
        XRDirectInteractor rightInteractor = ControllerInput.Instance.rightInteractor;
        if (right == null || left == null || leftInteractor == null || rightInteractor == null)
        {
            return;
        }
        bool leftGrip = ControllerInput.Instance.IsLeftGrip();
        bool rightGrip = ControllerInput.Instance.IsRightGrip();
        bool leftTarget = leftInteractor.selectTarget != null;
        bool rightTarget = rightInteractor.selectTarget != null;

        if ((leftGrip && rightGrip) && !(rightTarget && leftTarget))
        {
            if (lastHand == SelectedHand.Right)
            {
                rightInteractor.allowSelect = false;
                lastHand = SelectedHand.Left;
            }
            else if (lastHand == SelectedHand.Left)
            {
                leftInteractor.allowSelect = false;
                lastHand = SelectedHand.Right;
            }
        }
        else if (leftGrip)
        {
            lastHand = SelectedHand.Left;
        }
        else
        {
            lastHand = SelectedHand.Right;
        }

        //leftInteractor.allowSelect = true;
        //rightInteractor.allowSelect = true;

    }
}
