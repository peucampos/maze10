using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public static bool soundOn = true;

    public static void PlaySound(AudioClip clip, float volume = 1f){
        if (soundOn)
        {
            GameObject soundGameObject = new GameObject("Sound");
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.PlayOneShot(clip, volume);
        }
    }
}
