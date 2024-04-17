using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsiblePanel : MonoBehaviour 
{
    public Animator panel;
    bool open = false;

    public void TogglePanel ()
    {
        panel.SetTrigger( open ? "Open" : "Close" );
        open = !open;
    }
}
