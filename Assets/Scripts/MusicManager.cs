using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    AudioClip[] musics; //Pick an audio track to play.

    [SerializeField]
    GameObject obj;

    void Awake ()
    {
        // var source = obj.GetComponent<AudioSource>();
        // source.PlayOneShot(musics[Random.Range(0, musics.Length)], 0.25f); //Plays the audio.
    }
}
