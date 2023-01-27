using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;


public class GameOver : MonoBehaviour
{
    [SerializeField]
    private RectTransform fader;

    [SerializeField]
    TMP_Text scoreText;

    [SerializeField]
    TMP_Text levelText;

    [SerializeField]
    TMP_Text newHighScoreText;

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

            if (GameManager.isConnectedToGooglePlayServices)
            {
                long oldHighScore = 0;

                PlayGamesPlatform.Instance.LoadScores(
                            GameManager.leaderboardID,
                            LeaderboardStart.PlayerCentered,
                            1,
                            LeaderboardCollection.Public,
                            LeaderboardTimeSpan.AllTime,
                            (data) =>
                            {
                                if (data.Valid)
                                    oldHighScore = data.PlayerScore.value;
                                else
                                    Debug.Log("GameOver.cs - Player high score data invalid.");                                    
                            });
                
                if (OpenDoor.score > oldHighScore)
                {
                    Social.ReportScore(OpenDoor.score, GameManager.leaderboardID, 
                        (success) => 
                        {
                            if (!success) 
                                Debug.Log("GameOver.cs - High score saved.");
                            else
                                Debug.Log("GameOver.cs - Problem saving high score.");
                        });
                    newHighScoreText.text = "New High Score!!!";
                }
                else
                    newHighScoreText.text = "";

            }
            else
                Debug.Log("GameOver.cs - Not connected to Google Play Services.");

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
