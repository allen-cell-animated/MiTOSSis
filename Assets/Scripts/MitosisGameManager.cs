﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MitosisGameManager : MonoBehaviour 
{
    public string currentStructureName;
    public float waitBetweenThrowableSpawn = 0.2f;
    public float throwableSpawnHeight = 3f;
    public Vector2 throwableSpawnRingExtents = new Vector2( 0.5f, 0.6f );
    public float throwableSpawnScale = 0.4f;
    public float targetHeight = 1.5f;
    public float funnelRadius = 4f;

    string[] throwableNames = { "Prophase", "Prometaphase", "Metaphase", "Anaphase", "Telophase"};
    ThrowableCell[] throwableCells;
    Target[] targets;
    GameObject[] arrows;
    float lastThrowableCheckTime = 5f;
    float timeBetweenThrowableChecks = 0.1f;
    int correctlyPlacedThrowables;
    int animationPhase;
    bool destroyWhenOutOfBounds;
    float startTime;
    GameObject wallParent;
    float defaultThrowableBoundsRadius = 1.5f;
    float throwableBoundsRadius = 1.5f;
    float defaultTargetDistanceFromBounds = 1f;
    float targetDistanceFromCenter = 2f;
    int nSuccessCells = 6;

    public float GetTargetDistanceFromCenter ()
    {
        return targetDistanceFromCenter;
    }

    public void StartGame (string _structureName, float timeBeforeCellDrop)
    {
        Resources.UnloadUnusedAssets();
        correctlyPlacedThrowables = 0;
        currentStructureName = _structureName;
        throwableBoundsRadius = GetBoundaryRadius();
        targetDistanceFromCenter = throwableBoundsRadius + defaultTargetDistanceFromBounds;
        SpawnWalls();
        SpawnTargetsAndArrows();
        StartCoroutine( SpawnThrowables( currentStructureName, timeBeforeCellDrop, 2f ) );
        startTime = Time.time;
    }

    public void StartTimer ()
    {
        startTime = Time.time;
    }

    void Update ()
    {
        CheckIfThrowablesOutOfBounds();

        if (VisualGuideManager.Instance.currentMode == VisualGuideGameMode.Play)
        {
            UIManager.Instance.UpdateTime( startTime );
        }
    }

    Vector3 randomPositionInThrowableSpawnArea
    {
        get
        {
            return Quaternion.Euler( 0, Random.Range( 0, 360f ), 0 ) * (Random.Range( throwableSpawnRingExtents.x, throwableSpawnRingExtents.y ) * Vector3.forward) + throwableSpawnHeight * Vector3.up;
        }
    }

    public IEnumerator SpawnSuccessThrowables (string[] structureNames)
    {
        yield return new WaitForSeconds( 2f );
        
        float spawnProbability = (float)nSuccessCells / (float)(structureNames.Length * throwableNames.Length);
        destroyWhenOutOfBounds = true;
        for (int i = 0; i < structureNames.Length; i++)
        {
            StartCoroutine( SpawnThrowables( structureNames[i], i * structureNames.Length * waitBetweenThrowableSpawn, spawnProbability ) );
        }

        yield return new WaitForSeconds( structureNames.Length * throwableNames.Length * waitBetweenThrowableSpawn );

        throwableCells = GetComponentsInChildren<ThrowableCell>();
    }

    IEnumerator SpawnThrowables (string structureName, float waitTime, float spawnProbability)
    {
        yield return new WaitForSeconds( waitTime );

        GameObject prefab;
        throwableCells = new ThrowableCell[throwableNames.Length];
        for (int i = 0; i < throwableNames.Length; i++)
        {
            if (Random.value > spawnProbability)
            {
                continue;
            }

            prefab = Resources.Load( structureName + "/" + throwableNames[i] + "Cell" ) as GameObject;
            if (prefab == null)
            {
                UIManager.Instance.Log( "WARNING: Couldn't load prefab for " + structureName + " " + throwableNames[i] );
                continue;
            }

            throwableCells[i] = (Instantiate( prefab, transform ) as GameObject).GetComponent<ThrowableCell>();
            throwableCells[i].outline.enabled = true;
            throwableCells[i].transform.position = transform.position + randomPositionInThrowableSpawnArea;
            throwableCells[i].transform.rotation = Random.rotation;
            throwableCells[i].transform.localScale = throwableSpawnScale * Vector3.one;
            throwableCells[i].lastSpawnTime = Time.time;

            yield return new WaitForSeconds( waitBetweenThrowableSpawn );
        }
    }

    IEnumerator PlaceThrowable (Transform throwable, float waitTime)
    {
        yield return new WaitForSeconds( waitTime );

        throwable.position = transform.position + randomPositionInThrowableSpawnArea;
        throwable.rotation = Random.rotation;
        throwable.localScale = throwableSpawnScale * Vector3.one;
    }

    bool ThrowableIsOutOfBounds (Transform throwable)
    {
        Vector3 throwablePositionOnFloor = throwable.position - transform.position;
        throwablePositionOnFloor.y = 0;
        return throwablePositionOnFloor.magnitude > throwableBoundsRadius;
    }

    void CheckIfThrowablesOutOfBounds ()
    {
        if (throwableCells != null && Time.time - lastThrowableCheckTime > timeBetweenThrowableChecks)
        {
            foreach (ThrowableCell throwableCell in throwableCells)
            {
                if (throwableCell != null && throwableCell.attachedTarget == null 
                    && Time.time - throwableCell.lastSpawnTime > 2f && (!throwableCell.isMoving || throwableCell.transform.position.y < -1f)
                    && ThrowableIsOutOfBounds( throwableCell.transform ))
                {
                    if (destroyWhenOutOfBounds)
                    {
                        Destroy( throwableCell.gameObject );
                    }
                    else
                    {
                        throwableCell.ReleaseFromTarget( true );
                        throwableCell.lastSpawnTime = Time.time;
                        StartCoroutine( PlaceThrowable( throwableCell.transform, 1f ) );
                    }
                }
            }
            lastThrowableCheckTime = Time.time;
        }
    }

    void SpawnTargetsAndArrows ()
    {
        GameObject targetPrefab = Resources.Load( "Target" ) as GameObject;
        if (targetPrefab == null)
        {
            UIManager.Instance.Log( "WARNING: Couldn't load prefab for Target" );
            return;
        }
        GameObject arrowPrefab = Resources.Load( "Arrow" ) as GameObject;
        if (arrowPrefab == null)
        {
            UIManager.Instance.Log( "WARNING: Couldn't load prefab for Arrow" );
            return;
        }

        targets = new Target[throwableNames.Length + 1];
        arrows = new GameObject[throwableNames.Length + 1];
        Vector3 position = targetDistanceFromCenter * Vector3.forward;
        Quaternion dRotation = Quaternion.Euler( 0, 180f / (throwableNames.Length + 1f), 0 );
        position = dRotation * position;
        for (int i = 0; i < throwableNames.Length + 1; i++)
        {
            position = dRotation * position;
            targets[i] = (Instantiate( targetPrefab, position + targetHeight * Vector3.up, Quaternion.LookRotation( -position, Vector3.up ), transform ) as GameObject).GetComponent<Target>();
            if (i < throwableNames.Length)
            {
                targets[i].SetGoalName( throwableNames[i] );
            }
            else //interphase cell target
            {
                targets[i].theCollider.enabled = false;
            }

            position = dRotation * position;
            arrows[i] = Instantiate( arrowPrefab, position + targetHeight * Vector3.up, Quaternion.LookRotation( -position, Vector3.up ), transform );
        }
    }

    public IEnumerator TurnOffInterphaseCellTarget (float waitTime)
    {
        yield return new WaitForSeconds( waitTime );

        targets[throwableNames.Length].Bind();
    }

    void SpawnWalls ()
    {
        GameObject prefab = Resources.Load( "Wall" ) as GameObject;
        if (prefab == null)
        {
            UIManager.Instance.Log( "WARNING: Couldn't load prefab for Wall" );
            return;
        }

        Vector3 wallPosition = 16f * Vector3.forward;
        for (int i = 0; i < 6; i++)
        {
            wallPosition = Quaternion.Euler( 0, 360f / 6f, 0 ) * wallPosition;
            Instantiate( prefab, wallPosition + 20f * Vector3.up, Quaternion.LookRotation( -wallPosition, Vector3.up ), transform );
        }
    }

    Vector3[] GetBoundaryPoints ()
    {
        bool configured = OVRManager.boundary.GetConfigured();
        if (configured)
        {
            return OVRManager.boundary.GetGeometry( OVRBoundary.BoundaryType.PlayArea );
        }
        else
        {
            UIManager.Instance.Log( "WARNING: Boundary not configured." );
            return null;
        }
    }

    float GetBoundaryRadius ()
    {
        Vector3[] boundaryPoints = GetBoundaryPoints();
        if (boundaryPoints == null || boundaryPoints.Length < 3)
        {
            return defaultThrowableBoundsRadius;
        }
        Vector3 center = Vector3.zero;
        for (int i = 0; i < boundaryPoints.Length; i++)
        {
            center += boundaryPoints[i];
        }
        center /= boundaryPoints.Length;
        float distances = 0;
        for (int i = 0; i < boundaryPoints.Length; i++)
        {
            distances += (boundaryPoints[i] - center).magnitude;
        }
        return distances / boundaryPoints.Length;
    }

    public void RecordCorrectHit ()
    {
        correctlyPlacedThrowables++;

        if (correctlyPlacedThrowables >= throwableNames.Length)
        {
            VisualGuideManager.Instance.EnterSuccessMode( Time.time - startTime );
            foreach (ThrowableCell throwableCell in throwableCells)
            {
                throwableCell.grabInteraction.SetActive( false );
            }
        }
    }

    public void RemoveCorrectHit ()
    {
        if (correctlyPlacedThrowables > 0)
        {
            correctlyPlacedThrowables--;
        }
    }

    public void AnimateCellsForSuccess ()
    {
        for (int i = 0; i < throwableCells.Length; i++)
        {
            VisualGuideManager.Instance.AnimateCellSuccess( throwableCells[i].gameObject );
        }
    }

    public void PlayTargetSuccessAudio ()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].successAudio.Play();
        }
    }
}
