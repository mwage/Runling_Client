using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManagerSLA : MonoBehaviour {

    //attach scripts
    public InGameMenuManagerSLA _inGameMenuManagerSLA;
    public Level1SLA _level1;
    public Level2SLA _level2;
    public Level3SLA _level3;
    public Level4SLA _level4;
    public Level5SLA _level5;
    public Level6SLA _level6;
    public Level7SLA _level7;
    public Level8SLA _level8;
    public Level9SLA _level9;
    public Level10SLA _level10;
    public Level11SLA _level11;
    public Level12SLA _level12;
    public Level13SLA _level13;
    public StopCoroutineSLA _stopCoroutineSLA;

    public ScoreSLA _score;
    public InitializeGameSLA _initializeGameSLA;

    public GameObject win;

    public int[] moveSpeedSLA;

    public void Awake()
    {
        // Set movementspeed for the different levels
        moveSpeedSLA = new int[] { 8, 9, 9, 10, 9, 10, 11, 11, 11, 10, 11, 11, 14};
    }
    
    //Spawn Drones according to what level is active
    public void LoadDrones(int level)
    {
        switch (GameControl.currentLevel)
        {
            case 1:
                _level1.Level1Drones();
                break;
            case 2:
                _level2.Level2Drones();
                break;
            case 3:
                _level3.Level3Drones();
                break;
            case 4:
                _level4.Level4Drones();
                break;
            case 5:
                _level5.Level5Drones();
                break;
            case 6:
                _level6.Level6Drones();
                break;
            case 7:
                _level7.Level7Drones();
                break;
            case 8:
                _level8.Level8Drones();
                break;
            case 9:
                _level9.Level9Drones();
                break;
            case 10:
                _level10.Level10Drones();
                break;
            case 11:
                _level11.Level11Drones();
                break;
            case 12:
                _level12.Level12Drones();
                break;
            case 13:
                _level13.Level13Drones();
                break;
            default:
                Debug.Log("Error: Couldn't load Level " + GameControl.currentLevel);
                SceneManager.LoadScene("MainMenu");
                break;
        }
    }


    
    //Load next level or end game
    public void EndLevel(float delay)
    {
        if (GameControl.currentLevel == GameControl.lastLevelSLA)
        {
            StartCoroutine(EndGameSLA(delay));
        }
        else
        {
            StartCoroutine(NextLevel(delay));
        }
    }

    //load in all but the last level
   IEnumerator NextLevel(float delay)
    {
        yield return new WaitForSeconds(delay);
        _score.highScore.SetActive(false);
        _stopCoroutineSLA.StopRespawn();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        }
        Destroy(_initializeGameSLA.newPlayer);
        _score.currentScoreText.text = "0";
        _initializeGameSLA.InitializeGame();
    }

    //load after the last level
    IEnumerator EndGameSLA(float delay)
    {                
        //check/set highscores
        _score.SetGameHighScore();
        _score.SetCombinedScore();
                
        //load win screen
        yield return new WaitForSeconds(delay);
        _score.highScore.SetActive(false);
        _inGameMenuManagerSLA.CloseMenus();
        win.gameObject.SetActive(true);
    }
}
