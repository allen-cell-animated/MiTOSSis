using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MitossisBounds : MonoBehaviour
{
    public Transform[] testPoints;

    void Start()
    {
        SpawnFunnel();
    }

    void SpawnFunnel () 
    {
        Vector3[] boundaryPoints = new Vector3[testPoints.Length];
        for (int i = 0; i < testPoints.Length; i++)
        {
            boundaryPoints[i] = testPoints[i].position;
        }

        GameObject prefab = Resources.Load( "FunnelPanel" ) as GameObject;
        if (prefab == null)
        {
            UIManager.Instance.Log( "WARNING: Couldn't load prefab for FunnelPanel" );
            return;
        }

        for (int i = 0; i < boundaryPoints.Length; i++)
        {
            int nextIX = i < boundaryPoints.Length - 1 ? i + 1 : 0;
            Vector3 center = 0.5f * (boundaryPoints[nextIX] + boundaryPoints[i]);
            Vector3 axis = boundaryPoints[nextIX] - boundaryPoints[i];
            float axisLength = axis.magnitude;
            axis.Normalize();
            Vector3 perpendicular = Quaternion.AngleAxis(-90f, Vector3.up) * axis;
            Vector3 sectionPos = center + 1.5f * perpendicular;
            sectionPos.y = 0.5f;
            float sectionAngle = Vector3.Angle(axis, Vector3.right);
            sectionAngle *= Vector3.Dot(axis, Vector3.forward) > 0 ? -1 : 1;

            GameObject section = Instantiate( prefab, sectionPos, Quaternion.Euler( 60f, sectionAngle, 0 ), transform );
            section.transform.localScale = new Vector3( axisLength + 6f, 5f, 0.3f );
        }
    }

    void SpawnWalls ()
    {
        GameObject prefab = Resources.Load( "Wall" ) as GameObject;
        Vector3 wallPosition = (1.5f + 14.5f) * Vector3.forward;
        for (int i = 0; i < 6; i++)
        {
            wallPosition = Quaternion.Euler( 0, 360f / 6f, 0 ) * wallPosition;
            Instantiate( prefab, wallPosition + 20f * Vector3.up, Quaternion.LookRotation( -wallPosition, Vector3.up ), transform );
        }
    }
}
