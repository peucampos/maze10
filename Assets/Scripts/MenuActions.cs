using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActions : MonoBehaviour
{
    public void PlayBtn(){
        SceneManager.LoadScene(1);
    }

    public void ExitBtn(){
        Application.Quit();
    }

    public void GameOverBackBtn(){
        SceneManager.LoadScene(0);
    }
}
