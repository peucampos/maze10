using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenDoor : MonoBehaviour
{
    public static int level = 3;
    public static float time = 10;

    [SerializeField]
    AudioClip[] audioClipArray;

    private void Awake() {
        if (level > 3)
        {
            SoundManager.PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], 0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        level++;
        time += 10f;
        SceneManager.LoadScene(1);
    }
}
