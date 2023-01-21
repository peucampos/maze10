using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MazeRenderer : MonoBehaviour
{
    [SerializeField]
    private Transform horizontalWallPrefab = null;
   
    [SerializeField]
    private Transform verticalWallPrefab = null;

    [SerializeField]
    private Transform character = null;

    [SerializeField]
    private TMP_Text levelTime;

    private int width;
    private int height;
    public float size = 1;
    
    private int level = 3;
    private float time = 10;

    private bool doorSpawned = false;
    private Vector2 doorSpawnPos;
    private Vector2 charSpawnPos;

    // Start is called before the first frame update
    void Start()
    {   
        width = height = level;
        var maze = MazeGenerator.Generate(width, height);
        Draw(maze);
    }

    private void Update() 
    {
        if (Escaped())
        {
            level++;
            time += 10f;
            SceneManager.LoadScene(1);
        }
        else if (time > 0)
        {
            time -= Time.deltaTime;
            levelTime.text = time.ToString("0") + " - " + (level-2).ToString();
        }
        else
        {
            time = 10;
            level = 3;
            //SceneManager.LoadScene(2);            
        }
            
    }

    private void Draw(WallState[,] maze)
    {
        //randomly spawn char and door
        charSpawnPos = new Vector2(Random.Range(0, width-1), Random.Range(0, height-1) );
        doorSpawnPos = DefineDoorPosition();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var cell = maze[i,j];
                var position = new Vector3((-width/2 + i)*size, (-height/2 + j)*size, 1);
                
                // spawn char
                if (charSpawnPos.x == i && charSpawnPos.y == j)
                    character.position = new Vector3(position.x, position.y, character.position.z);

                if (cell.HasFlag(WallState.UP))
                {                    
                    if (!doorSpawned && j == height-1 && doorSpawnPos.x == i && doorSpawnPos.y == j)
                        doorSpawned = true;
                    else
                    {
                        var topWall = Instantiate(horizontalWallPrefab, transform) as Transform;
                        topWall.position = position + new Vector3(0,size/2,topWall.position.z);
                        topWall.localScale = new Vector3(size, topWall.localScale.y, topWall.localScale.z);
                    }
                }

                if (cell.HasFlag(WallState.LEFT))
                {                    
                    if (!doorSpawned && i == 0 && doorSpawnPos.x == i && doorSpawnPos.y == j)
                        doorSpawned = true;
                    else
                    {
                        var leftWall = Instantiate(verticalWallPrefab, transform) as Transform;
                        leftWall.position = position + new Vector3(-size/2,0,leftWall.position.z);
                        leftWall.localScale = new Vector3(leftWall.localScale.y, size, leftWall.localScale.z);
                    }
                }

                if (i == width - 1)
                {
                    if (cell.HasFlag(WallState.RIGHT))
                    {                        
                        if (!doorSpawned && doorSpawnPos.x == i && doorSpawnPos.y == j)
                            doorSpawned = true;
                        else
                        {
                            var rightWall = Instantiate(verticalWallPrefab, transform) as Transform;
                            rightWall.position = position + new Vector3(size/2,0,rightWall.position.z);
                            rightWall.localScale = new Vector3(rightWall.localScale.y, size, rightWall.localScale.z);                                     
                        }
                    }
                }

                if (j == 0)
                {
                    if (cell.HasFlag(WallState.DOWN))
                    {
                        if (!doorSpawned && doorSpawnPos.x == i && doorSpawnPos.y == j)
                            doorSpawned = true;
                        else
                        {
                            var bottomWall = Instantiate(horizontalWallPrefab, transform) as Transform;
                            bottomWall.position = position + new Vector3(0,-size/2,bottomWall.position.z);
                            bottomWall.localScale = new Vector3(size, bottomWall.localScale.y, bottomWall.localScale.z);
                        }
                    }
                }
            }
        }
    }

    bool Escaped()
    {
        Debug.Log(doorSpawnPos.x + " E " + doorSpawnPos.y + "|" + charSpawnPos.x + " C " + charSpawnPos.y);
        return false;
        // return (character.position.x < exitPosition.x &&
        //         character.position.x > exitPosition.x &&
        //         character.position.y < exitPosition.y &&
        //         character.position.y > exitPosition.y); 
    }
        
    Vector2 DefineDoorPosition()
    {
        Vector2 doorPos = new Vector2();

        switch (Random.Range(0,3))
        {
            case 1: // X=0 Y=random
                doorPos.x = 0;
                doorPos.y = Random.Range(0, height-1);
                break;
            case 2: // X=max Y=random
                doorPos.x = width-1;
                doorPos.y = Random.Range(0, height-1);;
                break;
            case 3: // X=random Y=0
                doorPos.x = Random.Range(0, width-1);
                doorPos.y = 0;                
                break;
            default: // X=random Y=max
                doorPos.x = Random.Range(0, width-1);
                doorPos.y = height-1;
                break;
        }

        return doorPos;
    }
}
