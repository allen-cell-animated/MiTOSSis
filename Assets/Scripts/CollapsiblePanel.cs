using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsiblePanel : MonoBehaviour 
{
    public Animator panel;
    public GameObject openLabel;
    public GameObject closeLabel;
    bool open;

    public void TogglePanel ()
    {
        panel.SetTrigger( open ? "Close" : "Open" );
        openLabel.SetActive( open );
        closeLabel.SetActive( !open );
        open = !open;
    }
}
