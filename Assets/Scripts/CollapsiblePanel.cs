using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollapsiblePanel : MonoBehaviour 
{
    public GameObject button;
    public GameObject openLabel;
    public GameObject closeLabel;
    public Animator dataPanel;
    public Animator structurePanel;
    public Text structureTitle;
    public Image structureImage;
    public Text structureText;

    bool open;
    bool isDisplayingStructure;

    public void Enable ()
    {
        if (open) {
            TogglePanel();
        }
        button.SetActive( true );
    }

    public void Disable ()
    {
        if (open) {
            TogglePanel();
        }
        button.SetActive( false );
    }

    public void SetDataContent ()
    {
        Enable();  // enabled by default in lobby
        isDisplayingStructure = false;
    }

    public void SetStructureContent (StructureData structureData)
    {
        Disable();  // disabled until game is over
        structureTitle.text = structureData.structureName;
        structureImage.sprite = structureData.infoImage;
        structureText.text = structureData.description;
        isDisplayingStructure = true;
    }

    public void TogglePanel ()
    {
        if (isDisplayingStructure)
        {
            structurePanel.SetTrigger( open ? "Close" : "Open" );
        }
        else {
            dataPanel.SetTrigger( open ? "Close" : "Open" );
        }
        openLabel.SetActive( open );
        closeLabel.SetActive( !open );
        open = !open;
    }
}
