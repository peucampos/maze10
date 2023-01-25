using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private RectTransform fader;

    [SerializeField]
    TMP_Text scoreText;

    [SerializeField]
    TMP_Text levelText;

    [SerializeField]
    AudioClip deathClip;

    void Start()
    {
            Time.timeScale = 1;
            fader.gameObject.SetActive(true);
            LeanTween.alpha(fader, 0f, 1f).setOnComplete(() => {
                fader.gameObject.SetActive(false);
            });

            levelText.text = OpenDoor.level.ToString();
            scoreText.text = OpenDoor.score.ToString();

            OpenDoor.time = 10;
            OpenDoor.level = 1;      
            OpenDoor.score = 0;      

            SoundManager.PlaySound(deathClip);
    }

    
    public void GameOverBackBtn(){
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 1f, 1f).setOnComplete(() => {
            SceneManager.LoadScene(0);            
        });
    }
}
