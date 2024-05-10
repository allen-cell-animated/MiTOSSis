using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSound : MonoBehaviour
{
    public float timeIncrementSeconds = 15f;
    public AudioSource[] audioSources;

    void Start()
    {
        float timeOffset = 0f;
        foreach (AudioSource audio in audioSources)
        {
            audio.time = timeOffset;
            audio.Play();
            timeOffset += timeIncrementSeconds;
        }
    }
}
