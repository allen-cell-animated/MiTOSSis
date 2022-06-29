using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellStructure : MonoBehaviour 
{
    [Header("Cell Structure Settings")]

    public bool isNucleus;
    public string structureName;
    public float nameWidth = 80f;
    public int structureIndex;

    Collider _collider;
    public Collider theCollider
    {
        get
        {
            if (_collider == null)
            {
                _collider = GetComponent<Collider>();
            }
            return _collider;
        }
    }

    Colorer _colorer;
    public Colorer colorer
    {
        get
        {
            if (_colorer == null)
            {
                _colorer = GetComponent<Colorer>();
            }
            return _colorer;
        }
    }
}
