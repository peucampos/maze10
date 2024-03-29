using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenDoor : MonoBehaviour
{
    public static int level = 1;
    public static float time = 10;
    public static long score = 0;

    [SerializeField]
    AudioClip[] audioClipArray;

    private void Awake() {
        if (level > 1)
        {
            SoundManager.PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], 0.2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        score += (level * 50) + (Mathf.RoundToInt(time) * level);
        level++;
        time += level + 10;
        SceneManager.LoadScene(1);
    }
}
