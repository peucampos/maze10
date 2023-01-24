using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenDoor : MonoBehaviour
{
    public static int level = 1;
    public static float time = 10;
    public static int score = 0;

    [SerializeField]
    AudioClip[] audioClipArray;

    private void Awake() {
        if (level > 1)
        {
            SoundManager.PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], 0.2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        level++;
        time += 10f;
        score += level * Mathf.RoundToInt(time);
        SceneManager.LoadScene(1);
    }
}
