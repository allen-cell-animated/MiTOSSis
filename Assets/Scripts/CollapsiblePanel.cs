using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsiblePanel : MonoBehaviour 
{
    public GameObject hotspot;
    public Animator panel;

    bool colliding;

    void OnTriggerEnter (Collider other)
    {
        if (other.tag == "MainCamera" && !colliding)
        {
            SetPanel( true );
            colliding = true;
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.tag == "MainCamera" && colliding)
        {
            SetPanel( false );
            colliding = false;
        }
    }

    void SetPanel (bool open)
    {
        panel.SetTrigger( open ? "Open" : "Close" );
        hotspot.SetActive( !open );
    }
}
