using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ControlSLA : MonoBehaviour
{
    //attach scripts
    public LevelManagerSLA _levelManager;
    public ScoreSLA _scoreSLA;
    public InitializeGameSLA _initializeGameSLA;
    public DeathSLA _deathSLA;


    public bool stopUpdate;


    void Start()
    {
        // Set current Level and movespeed, load drones and spawn immunity
        stopUpdate = true;
        _initializeGameSLA.InitializeGame();
        //GameControl.currentLevel = 10;
    }

    //update when dead
    private void Update()
    {
        if (GameControl.dead && !stopUpdate)
        {
            _deathSLA.Death();

            //in case of highscore, save and 
            _scoreSLA.SetLevelHighScore();

            //change level
            _levelManager.EndLevel(3f);

            //dont repeat above once player dead
            stopUpdate = true;
        }
    }
}

