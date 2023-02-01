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
    TMP_Text debugText;

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

            var newScore = OpenDoor.score;

            if (GameManager.isConnectedToGooglePlayServices)
            {
                PlayGamesPlatform.Instance.LoadScores(
                            GameManager.leaderboardID,
                            LeaderboardStart.PlayerCentered,
                            1,
                            LeaderboardCollection.Public,
                            LeaderboardTimeSpan.AllTime,
                            (data) =>
                            {
                                if (data.Valid)
                                {
                                    var oldHighScore = data.PlayerScore.value;
                                    debugText.text += newScore + ">" + oldHighScore;
                                    if (oldHighScore > -1 && newScore > oldHighScore)
                                    {
                                        Social.ReportScore(newScore, GameManager.leaderboardID, 
                                            (success) => 
                                            {
                                                if (!success) 
                                                    debugText.text += "GameOver.cs - High score saved.";
                                                else
                                                    debugText.text += "GameOver.cs - Problem saving high score.";
                                            });
                                        newHighScoreText.text = "New High Score!!!";
                                    }
                                    else
                                    {
                                        newHighScoreText.text = "";
                                        debugText.text += "Score Lower.";
                                    }
                                    debugText.text += "Data Score Valid.";                                
                                }
                                else
                                    debugText.text += "GameOver.cs - Player high score data invalid.";                                    
                            });
            }
            else
                debugText.text += "GameOver.cs - Not connected to Google Play Services.";

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
