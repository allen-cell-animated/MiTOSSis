using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressCanvas : MonoBehaviour 
{
    public Text time;
    public GameObject selectedER;
    public GameObject selectedGolgi;
    public GameObject selectedMTs;
    public GameObject selectedMitos;
    public Text titleLabel;
    public GameObject backLabel;
    public GameObject nextLabel;

    Animator _animator;
    public Animator animator
    {
        get
        {
            if (_animator == null)
            {
                _animator = gameObject.GetComponent<Animator>();
            }
            return _animator;
        }
    }

    public void SetSelected (string structureName, bool selected)
    {
        selectedER.SetActive( structureName == "Endoplasmic Reticulum (ER)" );
        selectedGolgi.SetActive( structureName == "Golgi Apparatus" );
        selectedMTs.SetActive( structureName == "Microtubules" );
        selectedMitos.SetActive( structureName == "Mitochondria" );
    }

    public void SetButtonLabel (bool next)
    {
        backLabel.SetActive( !next );
        nextLabel.SetActive( next );
    }

    public void GoBack ()
    {
        VisualGuideManager.Instance.ReturnToLobby();
    }
}
