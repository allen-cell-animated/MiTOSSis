using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class LaserPointerButton : MonoBehaviour
{
    bool hovering;
    bool pressed;
    float waitBetweenPresses = 0.3f;

    Button _button;
    Button button
    {
        get
        {
            if (_button == null)
            {
                _button = gameObject.GetComponent<Button>();
            }
            return _button;
        }
    }

    Image _image;
    Image image
    {
        get
        {
            if (_image == null)
            {
                _image = gameObject.GetComponent<Image>();
            }
            return _image;
        }
    }

    void Start ()
    {
        Vector2 size = gameObject.GetComponent<RectTransform>().sizeDelta;
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.size = new Vector3( size.x, size.y, 0.01f / transform.lossyScale.x );
    }

    public void Hover (bool enable)
    {
        hovering = enable;
        UpdateColor();
    }

    public void Click () 
    {
        if (pressed) { return; }

        pressed = true;
        UpdateColor();
        StartCoroutine( ReleasePress() );

        button.onClick.Invoke();
    }

    IEnumerator ReleasePress ()
    {
        yield return new WaitForSeconds( waitBetweenPresses );

        pressed = false;
        UpdateColor();
    }

    void UpdateColor ()
    {
        if (pressed)
        {
            image.color = button.colors.pressedColor;
        }
        else if (hovering)
        {
            image.color = button.colors.highlightedColor;
        }
        else
        {
            image.color = button.colors.normalColor;
        }
    }

    public void Reset ()
    {
        pressed = false;
        hovering = false;
        UpdateColor();
    }
}
