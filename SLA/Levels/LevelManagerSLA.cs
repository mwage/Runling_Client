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
    
    public ScoreSLA _score;
    public InitializeGameSLA _initializeGameSLA;

    public GameObject win;

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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        }

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
