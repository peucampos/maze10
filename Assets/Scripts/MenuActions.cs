using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using TMPro;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class MenuActions : MonoBehaviour
{
    [SerializeField]
    private RectTransform fader;

    [SerializeField]
    private TMP_Text pauseText;

    [SerializeField]
    private TMP_Text versionText;

    [SerializeField]
    private Toggle soundToggle;

    private bool isPaused = false;
        
    private void Start() {
        if (OpenDoor.level == 1)
        {
            fader.gameObject.SetActive(true);
            LeanTween.alpha(fader, 0f, 1f).setOnComplete(() => {
                fader.gameObject.SetActive(false);
            });
        }
        if (soundToggle != null)
            soundToggle.isOn = SoundManager.soundOn;
        
        if (versionText != null)
            versionText.text = "Alpha v" + Application.version;
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
    
    public void SoundBtn(bool value){
        SoundManager.soundOn = value;
    }

    public void PauseBtn()
    {
        RectTransform btn = GetComponent<RectTransform>();
        Time.timeScale = isPaused ? 1 : 0;
        isPaused = !isPaused;
        pauseText.text = isPaused ? "PAUSED" : "";
    }

    public void HighScoresBtn()
    {
        if (GameManager.isConnectedToGooglePlayServices)
        {
            Social.ShowLeaderboardUI();
        }
        else
            Debug.Log(" MenuActions.cs - Not connected to Google Play Services.");
    }

}
