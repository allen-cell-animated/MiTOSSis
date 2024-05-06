using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.XR.Interaction.Toolkit;

public class InterphaseCell : MonoBehaviour 
{
    bool inIsolationMode;
    CellStructure highlightedStructure;
    CellStructure selectedStructure;
    public Vector3 lobbyPosition;
    public Vector3 lobbyRotation;
    public float defaultScale;
    float lastSetStructureTime = -1f;
    float waitTimeToHoverStructure = 0.3f;
    // GameObject uiController;
    // XRRayInteractor ray;
    // XRInteractorLineVisual rayLine;
    // Gradient validGradient;
    // Gradient invalidGradient;
    // float rayMaxDistance;
    // bool rayNoHit = true;

    StructureLabel _structureLabel;
    StructureLabel structureLabel
    {
        get
        {
            if (_structureLabel == null)
            {
                _structureLabel = GameObject.FindObjectOfType<StructureLabel>();
            }
            return _structureLabel;
        }
    }

    List<CellStructure> _structures;
    List<CellStructure> structures
    {
        get
        {
            if (_structures == null)
            {
                _structures = new List<CellStructure>( GetComponentsInChildren<CellStructure>() );
            }
            return _structures;
        }
    }

    Transformer _transformer;
    Transformer transformer
    {
        get
        {
            if (_transformer == null)
            {
                _transformer = GetComponent<Transformer>();
            }
            return _transformer;
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

    Animator _animator;
    Animator animator
    {
        get
        {
            if (_animator == null)
            {
                _animator = GetComponentInChildren<Animator>();
            }
            return _animator;
        }
    }

    // List<XRBaseInteractable> _interactables;
    // List<XRBaseInteractable> interactables
    // {
    //     get
    //     {
    //         if (_interactables == null)
    //         {
    //             _interactables = new List<XRBaseInteractable>(GetComponentsInChildren<XRSimpleInteractable>());
    //         }
    //         return _interactables;
    //     }
    // }

    bool canInteract 
    {
        get
        {
            return !transformer.transforming && VisualGuideManager.Instance.currentMode == VisualGuideGameMode.Lobby;
        }
    }

    void Start ()
    {
        structureLabel.Disable();
        SetHighlightedStructure( VisualGuideManager.Instance.nextStructureName );
        // uiController = GameObject.Find("RightUIController");
        // ray = uiController.GetComponent<XRRayInteractor>();
        // rayLine = uiController.GetComponent<XRInteractorLineVisual>();
        // validGradient = rayLine.validColorGradient;
        // invalidGradient = rayLine.invalidColorGradient;
        // rayMaxDistance = ray.maxRaycastDistance;
    }

    // private void Update()
    // {
    //     //Custom behavior to avoid the XRRayInteractor hovering multiple structures at once
    //     if (uiController != null && ray != null && !inIsolationMode)
    //     {
    //         ray.TryGetCurrent3DRaycastHit(out RaycastHit raycastHit);

    //         if (raycastHit.collider != null && raycastHit.collider.gameObject.name != null)
    //         {
    //             ray.maxRaycastDistance = raycastHit.distance;
    //             rayNoHit = false;
    //         }

    //         else
    //         {
    //             rayLine.invalidColorGradient = validGradient;
    //             if (rayNoHit)
    //             {
    //                 RemoveHighlightAndLabel(highlightedStructure);
    //                 rayLine.invalidColorGradient = invalidGradient;
    //             }
    //             else
    //             {
    //                 rayNoHit = true;
    //             }
    //             ray.maxRaycastDistance = rayMaxDistance;
    //         }
    //     }

    //     //Sets the current structure if the XR selection fails.
    //     if (ControllerInput.Instance.IsRightTrigger() && highlightedStructure.structureName != VisualGuideManager.Instance.nextStructureName)
    //     {
    //         SetCurrentStructure();
    //     }
    // }

    public void SetCurrentStructure ()
    {
        VisualGuideManager.Instance.SetStucture(highlightedStructure.structureName);
    }

    public void TransitionToPlayMode (MitosisGameManager currentGameManager)
    {
        float duration = 1f;
        transformer.enabled = false;
        mover.MoveToOverDuration( currentGameManager.GetTargetDistanceFromCenter() * Vector3.forward + currentGameManager.targetHeight * Vector3.up, duration );
        rotator.RotateToOverDuration( Quaternion.Euler( new Vector3( -18f, -60f, 27f) ), duration );
        scaler.ScaleOverDuration( defaultScale, duration );
        StartCoroutine( currentGameManager.TurnOffInterphaseCellTarget( duration ) );
        structureLabel.Disable();
        SetHighlightedStructure( VisualGuideManager.Instance.nextStructureName );
    }

    public void TransitionToLobbyMode ()
    {
        ExitIsolationMode();
        transformer.enabled = true;
        MoveToCenter( 1f );
    }

    public void MoveToCenter (float duration)
    {
        mover.MoveToOverDuration( lobbyPosition, duration );
        Quaternion rotation = Quaternion.Euler(lobbyRotation);
        rotator.RotateToOverDuration( rotation, duration );
        scaler.ScaleOverDuration( defaultScale, duration );
    }

    public void HighlightAndLabelStructure (CellStructure _structure)
    {
        if (canInteract && Time.time - lastSetStructureTime >= waitTimeToHoverStructure)
        {
            structureLabel.SetLabel( _structure.structureName, _structure.nameWidth );
            SetHighlightedStructure( _structure );
        }
    }

    public void RemoveHighlightAndLabel (CellStructure _structure)
    {
        if (structureLabel != null && _structure == highlightedStructure 
            && Time.time - lastSetStructureTime >= waitTimeToHoverStructure)
        {
            structureLabel.Disable();
            SetHighlightedStructure( VisualGuideManager.Instance.nextStructureName );
        }
    }

    public void SetHighlightedStructure (string _structureName)
    {
        CellStructure _structure = structures.Find( s => s.structureName == _structureName );
        if (_structure != null)
        {
            SetHighlightedStructure( _structure );
        }
    }

    public void SetHighlightedStructure (CellStructure _structure)
    {
        highlightedStructure = _structure;
        lastSetStructureTime = Time.time;
        foreach (CellStructure structure in structures)
        {
            structure.colorer.SetColor( structure != highlightedStructure ? 0 : 1);
        }
    }

    public bool SelectStructure (string _structureName)
    {
        CellStructure _structure = structures.Find( s => s.structureName == _structureName );
        if (_structure != null)
        {
            selectedStructure = _structure;
            IsolateSelectedStructure();
            return true;
        }
        return false;
    }

    void IsolateSelectedStructure ()
    {
        if (selectedStructure != null && !inIsolationMode)
        {
            inIsolationMode = true;
            foreach (CellStructure structure in structures)
            {
                if (structure != selectedStructure && !structure.isNucleus)
                {
                    structure.gameObject.SetActive( false );
                }
                else
                {
                    structure.theCollider.enabled = false;
                }
            }
        }
    }

    void ExitIsolationMode ()
    {
        if (inIsolationMode)
        {
            foreach (CellStructure structure in structures)
            {
                structure.gameObject.SetActive( true );
                structure.theCollider.enabled = true;
            }
            inIsolationMode = false;
        }
    }
}
