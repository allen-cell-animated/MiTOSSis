using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    public AudioSource[] sounds;

    public void Play()
    {
        randomSound().Play();
    }

    private AudioSource randomSound ()
    {
        return sounds[Random.Range(0, sounds.Length)];
    }

    public void SetVolume (float _volume)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].volume = _volume;
        }
    }
}
