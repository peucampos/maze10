using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GameManager : MonoBehaviour
{
    public static bool isConnectedToGooglePlayServices = false;
    public const string leaderboardID = "CgkItorS3o0XEAIQAQ";

    private void Awake() {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    void Start()
    {
        SignInToGooglePlayServices();
    }

    void SignInToGooglePlayServices()
    {
        PlayGamesPlatform.Instance.Authenticate( 
            (status) => 
            {
                isConnectedToGooglePlayServices = (status == SignInStatus.Success);
            });
    }
}
