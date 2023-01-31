using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GoogleMobileAds.Api;

public class GameManager : MonoBehaviour
{
    public static bool isConnectedToGooglePlayServices = false;
    public const string leaderboardID = "CgkItorS3o0XEAIQAQ";
    public const string ANDROID_AD_KEY = "ca-app-pub-2506757271328786~7523485382";
    public const string IOS_AD_KEY = "ca-app-pub-2506757271328786/3923151722";
    public const string TEST_AD_KEY = "ca-app-pub-3940256099942544/5224354917";

    private void Awake() {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    void Start()
    {
        SignInToGooglePlayServices();
        
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
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
