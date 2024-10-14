using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InterphaseCell : MonoBehaviour 
{
    public Vector3 lobbyPosition;
    public Vector3 lobbyRotation;
    public float defaultScale;
    public AudioSource highlightAudio;
    public AudioSource selectAudio;
    
    bool inIsolationMode;
    CellStructure highlightedStructure;
    CellStructure selectedStructure;
    float lastSetStructureTime = -1f;
    float waitTimeToHoverStructure = 0.3f;
    List<CellStructure> hoveredStructures = new List<CellStructure>();

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
    }

    public void HoverStructure (CellStructure structure)
    {
        if (hoveredStructures.IndexOf( structure ) < 0)
        {
            hoveredStructures.Add( structure );
        }
    }

    public void UnhoverStructure (CellStructure structure)
    {
        if (hoveredStructures.IndexOf( structure ) >= 0)
        {
            hoveredStructures.Remove( structure );
        }
    }

    private void Update() 
    {
        if (!inIsolationMode)
        {
            if (hoveredStructures.Count == 1)
            {
                HighlightAndLabelStructure( hoveredStructures[0] );
            }
            else
            {
                RemoveHighlightAndLabel( highlightedStructure );
            }
        }

        if (
            ControllerInput.Instance.RightTriggerDown()
            && !highlightedStructure.isNucleus
            && highlightedStructure.structureName != VisualGuideManager.Instance.nextStructureName
        )
        {
            SetCurrentStructure();
        }
    }

    public void SetCurrentStructure ()
    {
        VisualGuideManager.Instance.SetStucture(highlightedStructure.structureName);
        selectAudio.Play();
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

    void HighlightAndLabelStructure (CellStructure _structure)
    {
        if (canInteract && structureLabel != null && _structure != highlightedStructure
            && Time.time - lastSetStructureTime >= waitTimeToHoverStructure)
        {
            structureLabel.SetLabel( _structure.structureName, _structure.nameWidth );
            SetHighlightedStructure( _structure );
            highlightAudio.Play();
        }
    }

    void RemoveHighlightAndLabel (CellStructure _structure)
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
