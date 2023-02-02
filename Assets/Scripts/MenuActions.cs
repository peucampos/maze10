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

// COLOR PALLETE
//Button Collor - 79381E
//Gradient - B93908 to FFA35C
//Text - FFA35C

public class MenuActions : MonoBehaviour
{
    [SerializeField]
    private RectTransform fader;

    [SerializeField]
    private TMP_Text pauseText;

    [SerializeField]
    private Toggle soundToggle;

    private bool isPaused = false;
        
    private void Start() {
        //fader only on first level, the others will be shrink animation
        if (OpenDoor.level == 1)
        {
            fader.gameObject.SetActive(true);
            LeanTween.alpha(fader, 0f, 1f).setOnComplete(() => {
                fader.gameObject.SetActive(false);
            });
        }
        // if on Main Menu
        if (soundToggle != null)
            soundToggle.isOn = SoundManager.soundOn;        
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
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status) {
        if (status == SignInStatus.Success) {
            GameManager.isConnectedToGooglePlayServices = true;

            if (GameManager.isConnectedToGooglePlayServices)
            {
                Social.ShowLeaderboardUI();
            }
            else
            {
                PlayGamesPlatform.Instance.ManuallyAuthenticate(ManualAuthentication);
            }
        } 
        else 
        {
            Debug.Log("MenuActions.cs - Google Play Services failed. Trying manually.");
            PlayGamesPlatform.Instance.ManuallyAuthenticate(ManualAuthentication);
        }
    }

    internal void ManualAuthentication(SignInStatus status) {
        if (status == SignInStatus.Success)
        {
            GameManager.isConnectedToGooglePlayServices = true;
            Social.ShowLeaderboardUI();
        }
        else         
            Debug.Log("MenuActions.cs - Manual Google Play Services failed.");
    }
}
