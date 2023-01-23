using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuActions : MonoBehaviour
{
    [SerializeField]
    private RectTransform fader;

    [SerializeField]
    private TMP_Text pauseText;

    private bool isPaused = false;
        
    private void Start() {
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 0f, 1f).setOnComplete(() => {
            fader.gameObject.SetActive(false);
        });
    }

    public void PlayBtn(){
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 1f, 1f).setOnComplete(() => {
            SceneManager.LoadScene(1);            
        });
    }

    public void ExitBtn(){
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 1f, 1f).setOnComplete(() => {
            Application.Quit();
        });        
    }

    public void GameOverBackBtn(){
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 1f, 1f).setOnComplete(() => {
            SceneManager.LoadScene(0);            
        });
    }

    public void PauseBtn()
    {
        RectTransform btn = GetComponent<RectTransform>();
        Time.timeScale = isPaused ? 1 : 0;
        isPaused = !isPaused;
        pauseText.text = isPaused ? "PAUSED" : "";
    }

}
