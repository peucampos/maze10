using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenDoor : MonoBehaviour
{
    public static int level = 3;
    public static float time = 10;

    private void OnTriggerEnter2D(Collider2D other) {
        level++;
        time += 10f;
        SceneManager.LoadScene(1);
    }
}
