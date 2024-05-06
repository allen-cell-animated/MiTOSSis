using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VisualGuideGameMode
{
    Lobby,
    Play,
    Success
}

public class VisualGuideManager : MonoBehaviour 
{
    public VisualGuideData data;
    public VisualGuideGameMode currentMode = VisualGuideGameMode.Lobby;
    public MitosisGameManager currentGameManager;

    MitosisGameManager successGameManager;
    string[] structureNames = { "Microtubules", "Mitochondria", "Endoplasmic Reticulum (ER)", "Golgi Apparatus" };
    int currentStuctureIndex;

    static VisualGuideManager _Instance;
    public static VisualGuideManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = GameObject.FindObjectOfType<VisualGuideManager>();
            }
            return _Instance;
        }
    }

    InterphaseCell _interphaseCell;
    public InterphaseCell interphaseCell
    {
        get
        {
            if (_interphaseCell == null)
            {
                _interphaseCell = GameObject.FindObjectOfType<InterphaseCell>();
            }
            return _interphaseCell;
        }
    }

    public string nextStructureName 
    {
        get
        {
            return structureNames[currentStuctureIndex];
        }
    }

    public void SetNextStructure (int index)
    {
        if (currentMode == VisualGuideGameMode.Lobby)
        {
            currentStuctureIndex = index;

            UIManager.Instance.Log("SELECT " + structureNames[currentStuctureIndex]);

            UIManager.Instance.nextStructureLabel.text = structureNames[currentStuctureIndex];
            UIManager.Instance.nextStructureLabel.GetComponent<Animator>().SetTrigger( "animate" );
        }
    }

    public void SetStucture(string name)
    {
        if (currentMode == VisualGuideGameMode.Lobby)
        {
            currentStuctureIndex = Array.IndexOf(structureNames, name);

            UIManager.Instance.Log("SELECT " + structureNames[currentStuctureIndex]);

            UIManager.Instance.nextStructureLabel.text = structureNames[currentStuctureIndex];
            UIManager.Instance.nextStructureLabel.GetComponent<Animator>().SetTrigger("animate");
        }
    }

    public void SelectNextStructureAndPlay ()
    {
        string structureName = structureNames[currentStuctureIndex];
        if (interphaseCell.SelectStructure( structureName )) {
            StartGame( structureName );
        }
        else {
            UIManager.Instance.Log( "Failed to select structure at index " + currentStuctureIndex );
        }
    }

    public void StartGame (string structureName)
    {
        UIManager.Instance.Log( "Start game with " + structureName );
        currentMode = VisualGuideGameMode.Play;

        UIManager.Instance.EnterPlayMode( data.structureData.Find( s => s.structureName == structureName ) );

        Cleanup();
        currentGameManager = CreateMitosisGameManager();
        currentGameManager.StartGame( structureName, 1.5f );

        interphaseCell.TransitionToPlayMode( currentGameManager );
    }

    MitosisGameManager CreateMitosisGameManager ()
    {
        GameObject prefab = Resources.Load( "MitosisGame" ) as GameObject;
        if (prefab == null)
        {
            UIManager.Instance.Log( "WARNING: Couldn't load prefab for MitosisGame" );
            return null;
        }
        return (Instantiate( prefab ) as GameObject).GetComponent<MitosisGameManager>();
    }

    public void EnterSuccessMode (float elapsedTime)
    {
        currentMode = VisualGuideGameMode.Success;

        UIManager.Instance.EnterSuccessMode( currentGameManager.currentStructureName, elapsedTime );

        AnimateCellSuccess( interphaseCell.gameObject );
        currentGameManager.AnimateCellsForSuccess();
        successGameManager = CreateMitosisGameManager();
        if (successGameManager != null)
        {
            successGameManager.StartCoroutine(successGameManager.SpawnAllThrowables(structureNames));
        }

        currentStuctureIndex++;
        if (currentStuctureIndex >= structureNames.Length)
        {
            currentStuctureIndex = 0;
        }
    }

    public void AnimateCellSuccess (GameObject cell)
    {
        UIManager.Instance.Log( "VisualGuideManager: animate cell success" );
        GameObject prefab = Resources.Load( "CellAnimator" ) as GameObject;
        if (prefab == null)
        {
            Debug.LogWarning( "Couldn't load prefab for CellAnimator" );
            return;
        }
        CellAnimator cellAnimator = (Instantiate( prefab ) as GameObject).GetComponent<CellAnimator>();

        cellAnimator.oldParent = cell.transform.parent;
        cellAnimator.transform.position = cell.transform.position;

        Animator animator = cellAnimator.GetComponentInChildren<Animator>();
        cell.transform.SetParent( animator.transform );
        animator.SetTrigger( "Success" );

        prefab = Resources.Load( "Confetti2" ) as GameObject;
        if (prefab == null)
        {
            Debug.LogWarning( "Couldn't load prefab for Confetti2" );
            return;
        }
        Instantiate( prefab, cell.transform.position, Quaternion.identity );
    }

    public void ReturnToLobby ()
    {
        currentMode = VisualGuideGameMode.Lobby;

        Cleanup();

        interphaseCell.TransitionToLobbyMode();
        UIManager.Instance.EnterLobbyMode( structureNames[currentStuctureIndex] );
    }

    void Cleanup ()
    {

        // foreach (OffsetInteractable offset in Resources.FindObjectsOfTypeAll(typeof(OffsetInteractable)) as OffsetInteractable[])
        // {
        //     offset.colliders.Clear();
        //     if (offset.isSelected)
        //     {
        //         Destroy(offset.gameObject);
        //     }
        // }

        if (currentGameManager != null)
        {
            Destroy( currentGameManager.gameObject );
        }
        if (successGameManager != null)
        {
            successGameManager.StopAllCoroutines();
            Destroy( successGameManager.gameObject );
        }
    }
}
