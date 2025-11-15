using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public bool sfxPlayed = false;

    public void playBlindBoxRoll(AudioSource audioSource, AudioClip clip)
    {
        if (!sfxPlayed)
        {
            audioSource.PlayOneShot(clip);
            sfxPlayed = true;
        }
    }
}
