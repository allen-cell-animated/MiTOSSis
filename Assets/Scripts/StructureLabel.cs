using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StructureLabel : MonoBehaviour 
{
    public GameObject label;
    public Text text;
    public RectTransform panel;
    public Transform cursor;

    public void SetLabel (string structureName, float nameWidth)
    {
        text.text = structureName;
        SetPanelSize( nameWidth );
        label.SetActive( true );
    }

    public void Disable ()
    {
        label.SetActive( false );
    }

    void SetPanelSize (float _width)
    {
        panel.sizeDelta = new Vector2( _width, 20f );
    }
}
