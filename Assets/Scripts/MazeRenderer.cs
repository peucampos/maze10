using Sys = System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using TMPro;

public class MazeRenderer : MonoBehaviour
{
    [SerializeField]
    private Transform horizontalWallPrefab = null;
   
    [SerializeField]
    private Transform verticalWallPrefab = null;

    [SerializeField]
    private GameObject characterObj = null;
    private Transform character = null;

    [SerializeField]
    private Transform doorPrefab = null;

    [SerializeField]
    private Transform[] floorPrefabs = null;

    [SerializeField]
    private Transform pickupPrefabs = null;
    
    [SerializeField]
    private TMP_Text level;
    
    [SerializeField]
    private TMP_Text score;

    [SerializeField]
    private TMP_Text time;

    [SerializeField]
    private GameObject adPanel;
    [SerializeField]
    private TMP_Text adButtonText;

    [SerializeField]
    private bool adsActive = false;

    public static bool noTime = false;
    public static Transform exitDoor = null;
    
    bool doorSpawned = false; //for corners where can be 2 doors
    private int width;
    private int height;
    private float size = 5;
    private bool adWatched = false;

    // Start is called before the first frame update
    void Start()
    {   
        width = height = OpenDoor.level;
        var maze = MazeGenerator.Generate(width, height);
        Draw(maze);
        
        LeanTween.scale(characterObj, new Vector3(1,1,1), 0.5f);
    }

    private void Update() 
    {
        if (OpenDoor.time > 0)
        {
            OpenDoor.time -= Time.deltaTime;
            time.text = OpenDoor.time.ToString("0");
            time.color = OpenDoor.time < 10 ? Color.red : Color.white;

            level.text = OpenDoor.level.ToString();

            score.text = OpenDoor.score.ToString();
        }
        else
        {
            if (adsActive)
            {
                if (adWatched)
                    SceneManager.LoadScene(2);      
                else
                {
                    adButtonText.text = ("Watch to get more seconds?");
                    adPanel.SetActive(true);
                    Time.timeScale = 0;
                }            
            }
            else
                SceneManager.LoadScene(2);
        }
            
    }

    public void AdForTimeBtn()
    {
        LoadAdForReward();
    }
    
    public void AdNoTyBtn()
    {
        SceneManager.LoadScene(2);
    }

    private RewardedAd rewardedAd;
    public void LoadAdForReward()
    {
        string adUnitId;
        #if UNITY_ANDROID
            adUnitId = GameManager.ANDROID_AD_KEY;
        #elif UNITY_IPHONE
            adUnitId = GameManager.IOS_AD_KEY;
        #else
            adUnitId = "unexpected_platform";
        #endif

        rewardedAd = new RewardedAd(adUnitId);
    
        // Called when the user should be rewarded for interacting with the ad.
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        
        // Called when an ad request has successfully loaded.
        rewardedAd.OnAdLoaded += (object sender, Sys.EventArgs args) => Debug.Log("HandleRewardedAdLoaded event received");
    
        // Called when an ad request failed to load.
        rewardedAd.OnAdFailedToLoad += (object sender, AdFailedToLoadEventArgs args) => Debug.Log("HandleRewardedAdFailedToLoad event received with message: "+ args);
        // Called when an ad is shown.
        rewardedAd.OnAdOpening += (object sender, Sys.EventArgs args) => Debug.Log("HandleRewardedAdOpening event received");
        // Called when an ad request failed to show.
        rewardedAd.OnAdFailedToShow += (object sender, AdErrorEventArgs args) =>  Debug.Log("HandleRewardedAdFailedToShow event received with message: " + args.Message);
        
        // Called when the ad is closed.
        rewardedAd.OnAdClosed += (object sender, Sys.EventArgs args) => Debug.Log("HandleRewardedAdClosed event received");

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        rewardedAd.LoadAd(request);

        if (rewardedAd.IsLoaded()) {
            rewardedAd.Show();
        }
    }

    void HandleUserEarnedReward(object sender, Reward args)
    {
        OpenDoor.time += (float)args.Amount;
        adPanel.SetActive(false);
        adWatched = true;
        Time.timeScale = 1;
    }

    void SpawnDoor(Vector3 position)
    {
        exitDoor = Instantiate(doorPrefab);
        var randXpos = Random.Range(0,1) == 0 ? -1f : 1f;
        var randYpos = Random.Range(0,1) == 0 ? -1f : 1f;
        if (OpenDoor.level == 1)
            exitDoor.position = new Vector3(position.x+(randXpos*2), position.y+(randXpos*2), exitDoor.position.z);
        else
            exitDoor.position = new Vector3(position.x+randXpos, position.y+randXpos, exitDoor.position.z);
        doorSpawned = true;
    }

    private void Draw(WallState[,] maze)
    {
        character = characterObj.transform;
        //randomly spawn door
        Position doorSpawnPos = DefineDoorPosition();
        //spawn char at the oposite side of the maze
        Position charSpawnPos = new Position { X = width-1-doorSpawnPos.X, Y =  height-1-doorSpawnPos.Y };

        Transform floorSelected = floorPrefabs[Random.Range(0, floorPrefabs.Length)];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var cell = maze[i,j];
                var position = new Vector3((-width/2 + i)*size, (-height/2 + j)*size, 0);
                
                // spawn char
                if (charSpawnPos.X == i && charSpawnPos.Y == j)
                    character.position = new Vector3(position.x, position.y, character.position.z);
                

                //spawn floor
                var floor = Instantiate(floorSelected, transform) as Transform;
                floor.position = new Vector3(position.x, position.y, floor.position.z);
                floor.localScale = new Vector3(size, size, floor.localScale.z);

                if (cell.HasFlag(WallState.UP))
                {
                    var topWall = Instantiate(horizontalWallPrefab, transform) as Transform;
                    topWall.position = position + new Vector3(0,size/2,topWall.position.z);
                    topWall.localScale = new Vector3(size, topWall.localScale.y, topWall.localScale.z);

                    if (!doorSpawned && j == height-1 && doorSpawnPos.X == i && doorSpawnPos.Y == j)
                        SpawnDoor(position);
                }

                if (cell.HasFlag(WallState.LEFT))
                {
                    var leftWall = Instantiate(verticalWallPrefab, transform) as Transform;
                    leftWall.position = position + new Vector3(-size/2,0,leftWall.position.z);
                    leftWall.localScale = new Vector3(leftWall.localScale.y, size, leftWall.localScale.z);

                    if (!doorSpawned && i == 0 && doorSpawnPos.X == i && doorSpawnPos.Y == j)
                        SpawnDoor(position);
                }

                if (i == width - 1)
                {
                    if (cell.HasFlag(WallState.RIGHT))
                    {
                        var rightWall = Instantiate(verticalWallPrefab, transform) as Transform;
                        rightWall.position = position + new Vector3(size/2,0,rightWall.position.z);
                        rightWall.localScale = new Vector3(rightWall.localScale.y, size, rightWall.localScale.z);

                        if (!doorSpawned && doorSpawnPos.X == i && doorSpawnPos.Y == j)
                            SpawnDoor(position);
                    }
                }

                if (j == 0)
                {
                    if (cell.HasFlag(WallState.DOWN))
                    {
                        var bottomWall = Instantiate(horizontalWallPrefab, transform) as Transform;
                        bottomWall.position = position + new Vector3(0,-size/2,bottomWall.position.z);
                        bottomWall.localScale = new Vector3(size, bottomWall.localScale.y, bottomWall.localScale.z);

                        if (!doorSpawned && doorSpawnPos.X == i && doorSpawnPos.Y == j)
                            SpawnDoor(position);
                    }
                }
            }
        }
    }

    Position DefineDoorPosition()
    {
        Position doorPos = new Position();

        switch (Random.Range(0,3))
        {
            case 1: // X=0 Y=random
                doorPos.X = 0;
                doorPos.Y = Random.Range(0, height-1);
                break;
            case 2: // X=max Y=random
                doorPos.X = width-1;
                doorPos.Y = Random.Range(0, height-1);;
                break;
            case 3: // X=random Y=0
                doorPos.X = Random.Range(0, width-1);
                doorPos.Y = 0;                
                break;
            default: // X=random Y=max
                doorPos.X = Random.Range(0, width-1);
                doorPos.Y = height-1;
                break;
        }

        return doorPos;
    }
}
